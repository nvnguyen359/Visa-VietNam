using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Visa
{
    public static class myClass
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

        public static List<int> ListNumberInt(int begin, int end,int step)
        {
            List<int> l= new List<int>();
            for (int i = begin; i <= end; i+=step)
            {
                l.Add(i);
            }
            return l;
        }

      
        public static List<string> DsTypeOfvisa()
        {
            using (VisaDataContext mydata= new VisaDataContext())
            {
                return mydata.TTypeofvisas.Select(j => j.name).Distinct().ToList();
            }
        }
    }
}