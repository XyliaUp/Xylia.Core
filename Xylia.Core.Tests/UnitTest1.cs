using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Xylia.Extension;

namespace Xylia.Core.Tests;

[TestClass]
public class UnitTest1
{
	[TestMethod]
	public void TestMethod1()
	{
		byte[] bytes = new byte[] { 0 ,0 , 0 ,0 ,0 ,2 ,255 };

		Debug.WriteLine(bytes.ToHex(false));
	}
}