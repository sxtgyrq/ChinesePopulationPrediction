using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EccCore
{
    public class Address
    {
        public static bool ValidateBitcoinAddress(string address)
        {
            //if (address.Length < 26 || address.Length > 35) throw new Exception("wrong length");
            if (address.Length < 26 || address.Length > 35) return false;

            byte[] decoded;
            bool success;
            decoded = DecodeBase58(address, out success);
            if (!success) { return false; }
            var d1 = Hash(decoded.SubArray(0, 21));
            var d2 = Hash(d1);
            if (!decoded.SubArray(21, 4).SequenceEqual(d2.SubArray(0, 4))) return false;
            return true;
        }
        const string Alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        const int Size = 25;
        private static byte[] DecodeBase58(string input, out bool s)
        {
            var output = new byte[Size];
            foreach (var t in input)
            {
                var p = Alphabet.IndexOf(t);
                if (p == -1)
                {
                    s = false;
                    return new byte[] { };
                };
                var j = Size;
                while (--j > 0)
                {
                    p += 58 * output[j];
                    output[j] = (byte)(p % 256);
                    p /= 256;
                }
                if (p != 0)
                {
                    s = false;
                    return new byte[] { };
                };
            }
            s = true;
            return output;
        }
        private static byte[] Hash(byte[] bytes)
        {
            var hasher = new SHA256Managed();
            return hasher.ComputeHash(bytes);
        }


    }
    public static class ArrayExtensions
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}
