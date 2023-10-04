using System.Security.Cryptography;
using System.Text;

namespace Xylia.Extension.Common;
public static partial class Files
{
    public static string GetFileSHA1(this FileInfo FileInfo)
    {
        if (FileInfo is null) return null;


        SHA1 mySHA1 = SHA1.Create();
        FileStream myFileStream = new FileStream(FileInfo.FullName, FileMode.Open, FileAccess.Read);
        byte[] data1 = new byte[myFileStream.Length];
        myFileStream.Read(data1, 0, data1.Length);
        myFileStream.Close();

        byte[] data2 = mySHA1.ComputeHash(data1);
        StringBuilder myBuilder = new StringBuilder();

        for (int i = 0; i < data2.Length; i++)
        {
            myBuilder.Append(data2[i].ToString("x2"));
        }
        return myBuilder.ToString();
    }

    public static bool IsUsing(this FileInfo FileInfo)
    {
        bool inUse = true;

        FileStream fs = null;

        try
        {
            fs = new FileStream(FileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.None);
            inUse = false;
        }
        catch
        {

        }
        finally
        {
            if (fs != null) fs.Close();
        }

        return inUse;
    }
}