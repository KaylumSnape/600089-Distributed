﻿using System;

namespace DistSysAcwClient.Class
{
    internal class Converters
    {
        public static byte[] HexStringToBytes(string hexString)
        {
            hexString = hexString.Replace("-", "");
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++) bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return bytes;
        }
    }
}