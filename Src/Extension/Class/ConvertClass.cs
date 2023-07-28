using System.Runtime.InteropServices;

namespace Xylia.Extension.Class;
public static class ConvertClass
{
	public static IntPtr ToIntPtr<T>(this T instance)
	{
		int size = Marshal.SizeOf(instance);
		IntPtr intPtr = Marshal.AllocHGlobal(size);
		Marshal.StructureToPtr(instance, intPtr, true);
		return intPtr;
	}

	public static T ToStruct<T>(this IntPtr info) => (T)Marshal.PtrToStructure(info, typeof(T));
}