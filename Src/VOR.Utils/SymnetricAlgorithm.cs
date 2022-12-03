using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VOR.Utils
{
    /// <summary>
    ///
    /// </summary>
    public class SymmetricAlgorithm
    {
        #region Private Static Members

        private static SymmetricAlgorithm _symmetric = null;

        #endregion Private Static Members

        #region Static Constructor

        /// <summary>
        /// Initializes the <see cref="SymmetricAlgorithm"/> class.
        /// </summary>
        static SymmetricAlgorithm()
        {
            _symmetric = new SymmetricAlgorithm();
        }

        #endregion Static Constructor

        #region Public Static Methods

        /// <summary>
        /// Gets an instance of the Symmetric algorithm.
        /// </summary>
        /// <returns>The instance</returns>
        public static SymmetricAlgorithm GetInstance()
        {
            return _symmetric;
        }

        #endregion Public Static Methods

        #region Private Members

        private byte[] _key;
        private byte[] _iv;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricAlgorithm"/> class.
        /// </summary>
        public SymmetricAlgorithm()
        {
            // 32 caracteres
            _key = Encoding.ASCII.GetBytes("OuiOuiKeyB5RYHKfgpGbAFIRoJlg54dG");
            // 16 caracteres
            _iv = Encoding.ASCII.GetBytes("OuiOuiVectorrIMl");
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Encrypts the specified string with the symmetric algorithm.
        /// </summary>
        /// <param name="original">The original string.</param>
        /// <returns>The encrypted string</returns>
        public string Encrypt(string original)
        {
            // Declare the streams used to encrypt to an in memory array of bytes.
            MemoryStream msEncrypt = null;
            CryptoStream csEncrypt = null;
            StreamWriter swEncrypt = null;

            // Declare the RijndaelManaged object used to encrypt the data.
            RijndaelManaged aesAlg = null;

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                msEncrypt = new MemoryStream();
                csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                swEncrypt = new StreamWriter(csEncrypt);

                //Write all data to the stream.
                swEncrypt.Write(original);
            }
            finally
            {
                if (swEncrypt != null)
                    swEncrypt.Close();
                if (csEncrypt != null)
                    csEncrypt.Close();
                if (msEncrypt != null)
                    msEncrypt.Close();

                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        /// <summary>
        /// Decrypts the specified encrypted string.
        /// </summary>
        /// <param name="encrypted">The encrypted string.</param>
        /// <returns>The decrypted string</returns>
        public string Decrypt(string encrypted)
        {
            // Declare the streams used to encrypt to an in memory array of bytes.
            MemoryStream msDecrypt = null;
            CryptoStream csDecrypt = null;
            StreamReader srDecrypt = null;

            // Declare the RijndaelManaged object used to encrypt the data.
            RijndaelManaged aesAlg = null;

            string original = String.Empty;

            try
            {
                byte[] cipherText = Convert.FromBase64String(encrypted);
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                msDecrypt = new MemoryStream(cipherText);
                csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                srDecrypt = new StreamReader(csDecrypt);

                // Read the decrypted bytes from the decrypting stream
                // and place them in a string.
                original = srDecrypt.ReadToEnd();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (srDecrypt != null)
                    srDecrypt.Close();
                if (csDecrypt != null)
                    csDecrypt.Close();
                if (msDecrypt != null)
                    msDecrypt.Close();

                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return original;
        }

        #endregion Public Methods
    }
}