using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace sodoff.Util;
public static class Md5 {
    public static string GetMd5Hash(string input) {
        byte[] array = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < array.Length; i++) {
            stringBuilder.Append(array[i].ToString("x2"));
        }
        return stringBuilder.ToString();
    }
}
