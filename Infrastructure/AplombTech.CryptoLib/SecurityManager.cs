using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.CryptoLib
{
    public static class SecurityManager
    {
        static SecurityManager()
        {
            RandomString = "bf%Sn2K=DjXRwFr4";// (bravo - foxtrot - Percentage - SIERRA - november - Two - KILO - Equals - DELTA - juliet - X-RAY - ROMEO - whiskey - FOXTROT - romeo - Four) //CryptLib.GenerateRandomIV(16);
            CypherKey = CryptLib.getHashSha256("J#&Vh9V&", 32); //(JULIET - Hash - Ampersand - VICTOR - hotel - Nine - VICTOR - Ampersand)
        }

        public static string Encrypt(string plainText)
        {
            return new CryptLib().encrypt(plainText, CypherKey, RandomString);
        }

        public static string Decrypt(string cypherText)
        {
            return new CryptLib().decrypt(cypherText, CypherKey, RandomString);
        }

        #region General properties
        private static string RandomString { get; set; }
        private static string CypherKey { get; set; }
        #endregion
    }
}
