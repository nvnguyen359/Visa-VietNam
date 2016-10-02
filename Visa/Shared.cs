using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Visa
{
    public static class Shared
    {
        private static byte[] EncryptData(string data)
        {
            var md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var encoder = new System.Text.UTF8Encoding();
            byte[] hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }
      
        public static string MahoaMd5(string data)
        {
            return BitConverter.ToString(EncryptData(data)).Replace("-", "").ToLower();
        }

        public static object ParseIntobject(string number)
        {
            try
            {
                return Convert.ToInt32(number);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static string InvoiceNumberVisa(string head,int lastNumber)
        {
            var time = DateTime.UtcNow.AddHours(7);
            var hs = time.Year.ToString();
            var nam = hs[2]+""+ hs[3];
            var thang = time.Month;
            var t = thang.ToString();
            if (thang < 10) { t = "0" + thang; }
            else
            {
                t = thang.ToString();
            }
            var n = lastNumber.ToString();
            if (lastNumber < 10) n = "0" + lastNumber;
            return head + "-" + nam + t +"-" +n;
        }
        public static int ParseInt(string number)
        {
            try
            {
                return int.Parse(number);
            }
            catch (Exception)
            {
              
                return 0;
            }
        }
        public static long Parselong(string number)
        {
            try
            {
                return long.Parse(number);
            }
            catch (Exception)
            {

                return 0;
            }
        }
        public static double ParseDouble(string number)
        {
            try
            {
                return double.Parse(number);
            }
            catch (Exception)
            {

                return double.NaN;
            }
        }

        public static string Add0(int t)
        {
            if (t < 10)
            {
                return "0" + t;
            }
            return t.ToString();
        }
    }
}