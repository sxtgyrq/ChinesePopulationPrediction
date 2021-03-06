﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EccCore
{
    public class PublicKeyF
    {
        //  System.Security.Cryptography.Ri
        private static readonly RIPEMD160Managed ripemd160 = new RIPEMD160Managed();
        //  static RIPEMD160 myRIPEMD160 = RIPEMD160Managed.Create();
        private static readonly SHA256 sha256 = new SHA256Managed();

        internal static string GetAddressOfcompressed(BigInteger[] publicKey)
        {
            var publicKeyArray1 = HexToByteArray.BigIntegerTo32ByteArray(publicKey[0]);
            HexToByteArray.ChangeDirection(ref publicKeyArray1);
            // var publicKeyArray2 = HexToByteArray.BigIntegerTo32ByteArray(publicKey[1]);

            byte[] resultAdd;
            if (publicKey[1].IsEven)
                resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x02 }, publicKeyArray1);
            else
                resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x03 }, publicKeyArray1);

           // Console.WriteLine($"压缩公钥为{ Hex.BytesToHex(resultAdd)}");
            //   Console.WriteLine($"压缩公钥为{ Calculate.Encode(resultAdd)}");

            var step3 = ripemd160.ComputeHash(sha256.ComputeHash(resultAdd));

            var step4 = Calculate.BiteSplitJoint(new byte[] { 0x00 }, step3);

            var step5 = sha256.ComputeHash(sha256.ComputeHash(step4));

            var step6 = Calculate.BiteSplitJoint(step4, new byte[] { step5[0], step5[1], step5[2], step5[3] });

            return Calculate.Encode(step6);
        }

        internal static string GetAddressOfUncompressed(BigInteger[] publicKey)
        {
            var publicKeyArray1 = HexToByteArray.BigIntegerTo32ByteArray(publicKey[0]);
            HexToByteArray.ChangeDirection(ref publicKeyArray1);
            var publicKeyArray2 = HexToByteArray.BigIntegerTo32ByteArray(publicKey[1]);
            HexToByteArray.ChangeDirection(ref publicKeyArray2);
            //    var array = HexToByteArray.BigIntegerTo32ByteArray(privateKey);

            var resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x04 }, publicKeyArray1);
            resultAdd = Calculate.BiteSplitJoint(resultAdd, publicKeyArray2);
           // Console.WriteLine($"非压缩公钥为{ Hex.BytesToHex(resultAdd)}");


            var step3 = ripemd160.ComputeHash(sha256.ComputeHash(resultAdd));

            var step4 = Calculate.BiteSplitJoint(new byte[] { 0x00 }, step3);

            var step5 = sha256.ComputeHash(sha256.ComputeHash(step4));

            var step6 = Calculate.BiteSplitJoint(step4, new byte[] { step5[0], step5[1], step5[2], step5[3] });

            return Calculate.Encode(step6);
        }
    }
}
