using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EccCore
{
    public class SecretFile
    {
        public static string SecretFileF()
        {
            SHA256 sha256 = new SHA256Managed();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyyMMddhhssmm") + "aayrq1111111111111www.nyrq123.comsssssssss222222"));
            var privateKey = Bytes32.ConvetToBigInteger(hash);
            var privateByte = HexToBigInteger.BigIntegerToByteArray(privateKey);
            var resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x80 }, privateByte);
            resultAdd = Calculate.BiteSplitJoint(resultAdd, new byte[] { 0x01 });
            byte[] chechHash = Calculate.GetCheckSum(resultAdd);
            resultAdd = Calculate.BiteSplitJoint(resultAdd, chechHash);
            var privateKey1 = Calculate.Encode(resultAdd);
            return privateKey1;
        }
    }
    public class Bytes32
    {
        internal static BigInteger ConvetToBigInteger(byte[] hash1)
        {
            BigInteger result = 0;
            for (var i = 0; i < hash1.Length; i++)
            {
                result = result * 256;
                var item = Convert.ToInt32(hash1[i]);
                result += item;

            }
            return result % Secp256k1.q;
        }
    }
    public class Secp256k1
    {
        public static System.Numerics.BigInteger p = HexToBigInteger.inputHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F");
        public static System.Numerics.BigInteger a = HexToBigInteger.inputHex("0");
        public static System.Numerics.BigInteger b = HexToBigInteger.inputHex("7");
        public static System.Numerics.BigInteger x = HexToBigInteger.inputHex("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798");
        public static System.Numerics.BigInteger y = HexToBigInteger.inputHex("483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8");
        public static System.Numerics.BigInteger q = HexToBigInteger.inputHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141");
    }
    public class HexToBigInteger
    {
        public static BigInteger inputHex(string hexInput)
        {
            hexInput = hexInput.ToLower();
            BigInteger result = new BigInteger(0);
            for (var i = 0; i < hexInput.Length; i++)
            {
                result = result * 16;
                var charIndex = hexInput[i];
                switch (charIndex)
                {
                    case '0':
                        {
                            result += 0;
                        }; break;
                    case '1':
                        {
                            result += 1;
                        }; break;
                    case '2':
                        {
                            result += 2;
                        }; break;
                    case '3':
                        {
                            result += 3;
                        }; break;
                    case '4':
                        {
                            result += 4;
                        }; break;
                    case '5':
                        {
                            result += 5;
                        }; break;
                    case '6':
                        {
                            result += 6;
                        }; break;
                    case '7':
                        {
                            result += 7;
                        }; break;
                    case '8':
                        {
                            result += 8;
                        }; break;
                    case '9':
                        {
                            result += 9;
                        }; break;
                    case 'a':
                        {
                            result += 10;
                        }; break;
                    case 'b':
                        {
                            result += 11;
                        }; break;
                    case 'c':
                        {
                            result += 12;
                        }; break;
                    case 'd':
                        {
                            result += 13;
                        }; break;
                    case 'e':
                        {
                            result += 14;
                        }; break;
                    case 'f':
                        {
                            result += 15;
                        }; break;
                    default:
                        {
                            throw new Exception(charIndex.ToString());
                        }
                }
            }
            return result;
        }

        public static byte[] BigIntegerToByteArray(BigInteger inputV)
        {
            List<byte> result = new List<byte>();
            do
            {
                var v = HexToBigInteger.getInt(inputV % 256);
                result.Add(Convert.ToByte(v));
                inputV = inputV / 256;
            } while (!inputV.IsZero);

            //while (result.Count < 32)
            //{
            //    result.Insert(0, 0);
            //}

            return result.ToArray();

        }

        public static int getInt(BigInteger bigInteger)
        {
            int result = 0;
            int baseInt = 1;
            do
            {
                if (bigInteger.IsEven)
                {
                    result += 0;
                }
                else
                {
                    result += baseInt;
                }
                bigInteger = bigInteger / 2;
                baseInt = baseInt * 2;
            } while (!bigInteger.IsZero);
            return result;
        }
    }
}
