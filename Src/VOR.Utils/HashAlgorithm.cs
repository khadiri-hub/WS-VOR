using System;
using System.Security.Cryptography;
using System.Text;

namespace VOR.Utils
{
    /// <summary>
    /// Provides simple method to use an Hash algorithm
    /// </summary>
    public class HashAlgorithm
    {
        #region Private Static Members

        private static HashAlgorithm _symmetric = null;

        #endregion

        #region Static Constructor

        /// <summary>
        /// Initializes the <see cref="HashAlgorithm"/> class.
        /// </summary>
        static HashAlgorithm()
        {
            _symmetric = new HashAlgorithm();
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets an instance of the Hash algorithm.
        /// </summary>
        /// <returns>The instance</returns>
        public static HashAlgorithm GetInstance()
        {
            return _symmetric;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Hashes the specified string with the default hash algorithm.
        /// </summary>
        /// <param name="original">The original string.</param>
        /// <returns>The hashed string</returns>
        public string Hash(string original)
        {
            SHA256 shaM = new SHA256Managed();
            byte[] unhashedByteArray = Encoding.Unicode.GetBytes(original);
            byte[] hashedByteArray = shaM.ComputeHash(unhashedByteArray);
            return Convert.ToBase64String(hashedByteArray);
        }

        #endregion
    }
}
