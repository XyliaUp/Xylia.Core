using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;

namespace Xylia.Extension
{
	public static class lib
	{
		public static void Elevate()
		{
			// 如果不是以administrator身份运行
			if (!IsAdministrator())
			{
				// 以administrator身is份，重启自己
				ProcessStartInfo proc = new();
				proc.UseShellExecute = true;
				proc.WorkingDirectory = Environment.CurrentDirectory;
				proc.FileName = Application.ExecutablePath;
				proc.Verb = "runas";
				try
				{
					Process.Start(proc);
					Application.Exit();  // 退出
				}
				catch
				{
					// 如果用户拒绝提升权限，直接退出
				}
			}
			else MessageBox.Show("正以administrator运行当前进程", "UAC");

		}

		public static bool IsAdministrator()
		{
			WindowsIdentity current = WindowsIdentity.GetCurrent();
			WindowsPrincipal windowsPrincipal = new(current);
			return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
		}


		#region 延迟函数
		public static bool Delay(int delayTime)
		{
			DateTime now = DateTime.Now;
			int s;
			do
			{
				TimeSpan spand = DateTime.Now - now;
				s = spand.Seconds;
				Application.DoEvents();
			}

			while (s < delayTime);
			return true;
		}
		#endregion



		#region Process
		[DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
		public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

		public static void ClearMemory()
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();

			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
		}



		public static string GetProcessUsedMemory()
		{
			double Memory = Process.GetCurrentProcess().WorkingSet64;

			if (Memory < 1024) return Memory.ToString("0") + " 字节";
			if ((Memory /= 1024.0) < 1000) return Memory.ToString("0") + " Kb";
			if ((Memory /= 1024.0) < 1000) return Memory.ToString("0.0") + " MB";

			return (Memory /= 1024.0).ToString("0.00") + " GB";
		}

		public static double GetProcessUsedCPU() => GetUsedProcess(Process.GetCurrentProcess().ProcessName);

		public static double GetUsedProcess(string processName)
		{
			try
			{
				int interval = 1000;
				var prevCpuTime = TimeSpan.Zero;
				while (true)
				{
					Process[] myproceexe = Process.GetProcessesByName(processName);
					if (myproceexe.Count() > 0)
					{
						var value = (myproceexe[0].TotalProcessorTime - prevCpuTime).TotalMilliseconds / interval / Environment.ProcessorCount * 100;
						prevCpuTime = myproceexe[0].TotalProcessorTime;
						return value;
					}
				}
			}
			catch
			{

			}

			return -1;
		}
		#endregion
	}
}
