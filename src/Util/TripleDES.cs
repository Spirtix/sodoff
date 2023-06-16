using System;
using System.Security.Cryptography;
using System.Text;

namespace sodoff.Util;

public static class TripleDES {
    public static void NotEmpty(string str, string paramName) {
        TripleDES.NotEmpty(str, paramName, null);
    }

    public static void NotEmpty(string str, string paramName, string message) {
        if (str == null || str.Trim().Length <= 0) {
            throw new ArgumentException(message, paramName);
        }
    }

    public static string EncryptUnicode(string plaintext, string key) {
        NotEmpty(key, "key");
        if (string.IsNullOrEmpty(plaintext)) {
            return null;
        }
        ICryptoTransform cryptoTransform = TripleDES.CreateProviderUnicode(key).CreateEncryptor();
        byte[] bytes = Encoding.Unicode.GetBytes(plaintext);
        return Convert.ToBase64String(cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length));
    }

    public static string DecryptUnicode(string ciphertext, string key) {
        NotEmpty(key, "key");
        if (string.IsNullOrEmpty(ciphertext)) {
            return null;
        }
        string text;
        try {
            ICryptoTransform cryptoTransform = TripleDES.CreateProviderUnicode(key).CreateDecryptor();
            byte[] array = Convert.FromBase64String(ciphertext);
            byte[] array2 = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
            text = Encoding.Unicode.GetString(array2, 0, array2.Length);
        } catch {
            text = null;
        }
        return text;
    }

    private static TripleDESCryptoServiceProvider CreateProviderUnicode(string key) {
        TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
        MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
        tripleDESCryptoServiceProvider.Key = md5CryptoServiceProvider.ComputeHash(Encoding.Unicode.GetBytes(key));
        tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
        return tripleDESCryptoServiceProvider;
    }

    public static string EncryptASCII(string plaintext, string key) {
        NotEmpty(key, "key");
        if (string.IsNullOrEmpty(plaintext)) {
            return null;
        }
        ICryptoTransform cryptoTransform = CreateProviderASCII(key).CreateEncryptor();
        byte[] bytes = Encoding.UTF8.GetBytes(plaintext);
        return Convert.ToBase64String(cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length));
    }

    public static string DecryptASCII(string ciphertext, string key) {
        NotEmpty(key, "key");
        if (string.IsNullOrEmpty(ciphertext)) {
            return null;
        }
        string text;
        try {
            ICryptoTransform cryptoTransform = CreateProviderASCII(key).CreateDecryptor();
            byte[] array = Convert.FromBase64String(ciphertext);
            array = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
            text = Encoding.UTF8.GetString(array, 0, array.Length);
        } catch {
            text = null;
        }
        return text;
    }

    private static TripleDESCryptoServiceProvider CreateProviderASCII(string key) {
        TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
        MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
        tripleDESCryptoServiceProvider.Key = md5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(key));
        tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
        return tripleDESCryptoServiceProvider;
    }
}
