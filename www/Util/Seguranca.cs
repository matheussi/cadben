//https://forums.asp.net/t/1118844.aspx
namespace cadben.www.seguranca
{
    using System;
    using System.IO;
    using System.Web;
    using System.Text;
    using System.Collections;
    using System.Web.Configuration;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Collections.Specialized;

    public class QueryString : NameValueCollection
    {
        private string document;
        public string Document
        {
            get
            {
                return document;
            }
        }
        public QueryString()
        {
        }
        public QueryString(NameValueCollection clone)
            : base(clone)
        {
        }
        //################################################## ###############################################
        //This Class Has been used to get the URl from the address browser of the page
        // http://www.hanusoftware.com
        //################################################## ###############################################
        //this method has been used to get the current URL of the page 
        public static QueryString FromCurrent()
        {

            //returns the current url from the address bar
            return FromUrl(HttpContext.Current.Request.Url.AbsoluteUri);

        }
        /// <summary>
        /// This method has been used to divide the Address URl into characters chunks 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static QueryString FromUrl(string url)
        {
            //it breaks the address URL in array with separator of ? mark
            //this line breaks the Querystring and page 
            string[] parts = url.Split("?".ToCharArray());
            //instantiate the class object
            QueryString qs = new QueryString();
            //assign the page address to the variable
            qs.document = parts[0];
            //if there is any data in array
            if (parts.Length == 1)
                return qs;

            //breaks the QueryString into characters chunks with separator mark &
            string[] keys = parts[1].Split("&".ToCharArray());
            foreach (string key in keys)
            {
                //again breaks into chunks by + mark
                string[] part = key.Split("=".ToCharArray());
                if (part.Length == 1)
                    qs.Add(part[0], "");
                //adds the QueryString key and value pair to the assigned variable
                qs.Add(part[0], part[1]);
            }
            return qs;


        }
        /// <summary>
        /// This method clear all exceptions in the passed string
        /// </summary>
        /// <param name="except"></param>
        public void ClearAllExcept(string except)
        {
            //calls the method to clear except 
            ClearAllExcept(new string[] { except });

        }

        /// <summary>
        /// this is the usual method which has to call clear all exceptions
        /// </summary>
        /// <param name="except"></param>
        public void ClearAllExcept(string[] except)
        {
            //take an arrayList 
            ArrayList toRemove = new ArrayList();
            foreach (string s in this.AllKeys)
            {
                foreach (string e in except)
                {
                    if (s.ToLower() == e.ToLower())
                        if (!toRemove.Contains(s))
                            toRemove.Add(s);
                }
            }
            foreach (string s in toRemove)
                this.Remove(s);
        }

        /// <summary>
        /// this method adds the key value pairs in QueryString of the URL
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public override void Add(string name, string value)
        {
            //checks nullability of the name 
            if (this[name] != null)
                //if not null then assign value to it
                this[name] = value;
            else
                base.Add(name, value);
        }


        public override string ToString()
        {
            return ToString(false);
        }

        /// <summary>
        /// this ethod has been used to join all the characters array to the URL 
        /// </summary>
        /// <param name="includeUrl"></param>
        /// <returns></returns>
        public string ToString(bool includeUrl)
        {
            string[] parts = new string[this.Count];

            string[] keys = this.AllKeys;
            //for each keys breaks the URL into chunks
            for (int i = 0; i < keys.Length; i++)
                parts[i] = keys[i] + "=" + HttpContext.Current.Server.UrlEncode(this[keys[i]]);

            string url = String.Join("&", parts);

            if ((url != null || url != String.Empty) && !url.StartsWith("?"))
                url = "?" + url;

            if (includeUrl)
                url = this.document + url;

            return url;
        }
    }

    /************************************************************************************************/
    //https://www.codeproject.com/Articles/25719/Query-string-encryption-for-ASP-NET

    /// <summary>
    /// This class contains methods to en-/decrypt query strings.
    /// </summary>
    public static class CryptoQueryStringHandler
    {
        /// <summary>
        /// Encrypt query strings from string array.
        /// </summary>
        /// <param name="unencryptedStrings">Unencrypted query strings in the format 'param=value'.</param>
        /// <param name="key">Key, being used to encrypt.</param>
        /// <returns></returns>
        public static string EncryptQueryStrings(string[] unencryptedStrings, string key)
        {
            StringBuilder strings = new StringBuilder();

            foreach (string unencryptedString in unencryptedStrings)
            {
                if (strings.Length > 0) strings.Append("&");
                strings.Append(unencryptedString);
            }

            return string.Concat("request=", Encryption64.Encrypt(strings.ToString(), key));
        }

        /// <summary>
        /// Encrypt query strings from name value collection.
        /// </summary>
        /// <param name="unencryptedStrings">Unencrypted query strings.</param>
        /// <param name="key">Key, being used to encrypt.</param>
        /// <returns></returns>
        public static string EncryptQueryStrings(NameValueCollection unencryptedStrings, string key)
        {
            StringBuilder strings = new StringBuilder();

            foreach (string stringKey in unencryptedStrings.Keys)
            {
                if (strings.Length > 0) strings.Append("&");
                strings.Append(string.Format("{0}={1}", stringKey, unencryptedStrings[stringKey]));
            }

            return string.Concat("request=", Encryption64.Encrypt(strings.ToString(), key));
        }

        /// <summary>
        /// Decrypt query string.
        /// </summary>
        /// <param name="encryptedStrings">Encrypted query string.</param>
        /// <param name="key">Key, being used to decrypt.</param>
        /// <remarks>The query string object replaces '+' character with an empty character.</remarks>
        /// <returns></returns>
        public static string DecryptQueryStrings(string encryptedStrings, string key)
        {
            return Encryption64.Decrypt(encryptedStrings.Replace(" ", "+"), key);
        }
    }

    /// <summary>
    /// Http module that handles encrypted query strings.
    /// </summary>
    public class CryptoQueryStringUrlRemapper : IHttpModule
    {
        #region IHttpModule Members

        /// <summary>
        /// Initialize the http module.
        /// </summary>
        /// <param name="application">Application, that called this module.</param>
        public void Init(HttpApplication application)
        {
            // Attach the acquire request state event to catch the encrypted query string
            application.AcquireRequestState += application_AcquireRequestState;
        }

        public void Dispose()
        { }

        #endregion

        /// <summary>
        /// Event, that is called when the application acquires the request state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void application_AcquireRequestState(object sender, EventArgs e)
        {
            // Get http context from the caller.
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            // Check for encrypted query string
            string encryptedQueryString = context.Request.QueryString["request"];
            if (!string.IsNullOrEmpty(encryptedQueryString))
            {
                // Decrypt query strings
                string cryptoKey = WebConfigurationManager.AppSettings["CryptoKey"];
                string decryptedQueryString = CryptoQueryStringHandler.DecryptQueryStrings(encryptedQueryString, cryptoKey);
                context.Server.Transfer(context.Request.AppRelativeCurrentExecutionFilePath + "?" + decryptedQueryString);
            }
        }
    }

    public static class Encryption64
    {
        #region members

        private const string DEFAULT_KEY = "#kl?+@<z_-";

        #endregion

        public static string Encrypt(string stringToEncrypt, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream;

            // Check whether the key is valid, otherwise make it valid
            CheckKey(ref key);

            des.Key = HashKey(key, des.KeySize / 8);
            des.IV = HashKey(key, des.KeySize / 8);
            byte[] inputBytes = Encoding.UTF8.GetBytes(stringToEncrypt);

            cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(inputBytes, 0, inputBytes.Length);
            cryptoStream.FlushFinalBlock();

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static string Decrypt(string stringToDecrypt, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream;

            // Check whether the key is valid, otherwise make it valid
            CheckKey(ref key);

            des.Key = HashKey(key, des.KeySize / 8);
            des.IV = HashKey(key, des.KeySize / 8);
            byte[] inputBytes = Convert.FromBase64String(stringToDecrypt);

            cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(inputBytes, 0, inputBytes.Length);
            cryptoStream.FlushFinalBlock();

            Encoding encoding = Encoding.UTF8;
            return encoding.GetString(memoryStream.ToArray());
        }

        /// <summary>
        /// Make sure the used key has a length of exact eight characters.
        /// </summary>
        /// <param name="keyToCheck">Key being checked.</param>
        private static void CheckKey(ref string keyToCheck)
        {
            keyToCheck = keyToCheck.Length > 8 ? keyToCheck.Substring(0, 8) : keyToCheck;
            if (keyToCheck.Length < 8)
            {
                for (int i = keyToCheck.Length; i < 8; i++)
                {
                    keyToCheck += DEFAULT_KEY[i];
                }
            }
        }

        /// <summary>
        /// Hash a key.
        /// </summary>
        /// <param name="key">Key being hashed.</param>
        /// <param name="length">Length of the output.</param>
        /// <returns></returns>
        private static byte[] HashKey(string key, int length)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            // Hash the key
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] hash = sha1.ComputeHash(keyBytes);

            // Truncate hash
            byte[] truncatedHash = new byte[length];
            Array.Copy(hash, 0, truncatedHash, 0, length);
            return truncatedHash;
        }
    }
}