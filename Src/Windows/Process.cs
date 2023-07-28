using System.Diagnostics;

using Vanara.PInvoke;

namespace Xylia;

public static class ProcessEx
{
	public static void ClearMemory()
	{
		GC.Collect();
		GC.WaitForPendingFinalizers();

		if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			Kernel32.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, SizeT.MinValue, SizeT.MinValue);
	}

	public static string GetProcessUsedMemory()
	{
		double Memory = Process.GetCurrentProcess().WorkingSet64;

		if (Memory < 1024) return Memory.ToString("0") + " Bit";
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
}