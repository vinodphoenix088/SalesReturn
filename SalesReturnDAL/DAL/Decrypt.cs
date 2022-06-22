﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Globalization;
using System.Configuration;
using System.IO;

namespace AssetMgmtSys.DAL
{
    public class DecryptFunction
    {
        public Byte[] lbtVector = { 240, 3, 45, 29, 0, 76, 173, 59 };
        public string encryptionKey = "NetCommKey";

        public TextInfo textInfo;
        public CultureInfo cultureInfo;
        //ListItem li;


        /***********************************Function for decrypt the data***************************/

        public  string Decrypt1(string encryptedValue)
        {
            Byte[] buffer;
            TripleDESCryptoServiceProvider desProvider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();

            try
            {
                buffer = Convert.FromBase64String(encryptedValue.Replace("ncl", "+").Replace("vvk", "=").Replace("sis", "/"));

                desProvider.Key = md5Provider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(encryptionKey));
                desProvider.IV = lbtVector;
                return Encoding.ASCII.GetString(desProvider.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length)).ToString();
            }
            finally
            {
                desProvider.Clear();
                md5Provider.Clear();
                desProvider = null;
                md5Provider = null;
            }
        }

        public string Decrypt(string pass)

    }
}