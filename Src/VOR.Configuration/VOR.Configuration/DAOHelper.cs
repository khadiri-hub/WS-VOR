using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace VOR.Configuration
{
    /// <summary>
    /// Class définissant les propriétés de connection à la base de données
    /// </summary>
    public class DAOHelper
    {
        protected static log4net.ILog Log = log4net.LogManager.GetLogger(typeof(DAOHelper));

        #region ConnectionString

        /// <summary>
        /// Connection string
        /// </summary>
        private static string _ConnectionString;

        /// <summary>
        /// Obtient la chaîne de connexion à la base de données
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                // if not initialized yet
                if (_ConnectionString == null)
                {
                    // look into the configuration
                    object raw = ConfigurationManager.ConnectionStrings["GESTOUR"];

                    // if found
                    if (raw != null)

                        // convert to string
                        _ConnectionString = raw.ToString();

                    // if not found
                    else

                        // log an error
                        Log.Error("ConnectionString not found");
                }

                return _ConnectionString;
            }
        }

        #endregion ConnectionString

        #region Tools

        /// <summary>
        /// Créé un paramètre d'entrée à partir du paramètre donné
        /// </summary>
        /// <param name="name">Le nom du paramètre</param>
        /// <param name="value">la valeur d'entrée</param>
        /// <returns>Un SqlParameter</returns>
        public static SqlParameter CreateInputParameter(string name, object value)
        {
            // valeur par défaut
            object input = DBNull.Value;

            // type par défaut
            SqlDbType? type = null;

            // si on a une valeur
            if (value != null)
            {
                // si la valeur est un entier
                if (value is Int32)
                {
                    type = SqlDbType.Int;
                    input = UIntToDB((int)value);
                }

                //si la valeur est entier short
                else if (value is short)
                {
                    type = SqlDbType.SmallInt;
                    input = value;
                }

                //si la valeur est petit entier (0 - 255)
                else if (value is byte)
                {
                    type = SqlDbType.TinyInt;
                    input = value;
                }

                // si la valeur est un décimal
                else if (value is decimal)
                {
                    type = SqlDbType.Decimal;
                    input = UDecimalToDB((decimal)value);
                }

                // si la valeur est une chaîne
                else if (value is string)
                {
                    type = SqlDbType.NVarChar;
                    input = StringToDB(value.ToString());
                }

                // si la valeur est un DateTime
                else if (value is DateTime)
                {
                    type = SqlDbType.DateTime;
                    input = DateTimeToDB((DateTime)value);
                }

                // si la valeur est un Guid
                else if (value is Guid)
                {
                    type = SqlDbType.UniqueIdentifier;
                    input = GuidToDB((Guid)value);
                }

                // si la valeur est un booléen nullable
                else if (value is bool?)
                {
                    type = SqlDbType.Bit;

                    // si on a une valeur
                    if (value != null)

                        // on l'utilise
                        input = value;
                }

                // si la valeur est un bool
                else if (value is bool)
                {
                    type = SqlDbType.Bit;
                    input = value;
                }

                //si la valeur est un tableau d'octet
                else if (value is byte[])
                {
                    type = SqlDbType.Binary;
                    input = value;
                }

                // si le type est inconnu
                else

                    // log
                    Log.Error("DataHelper.CreateInputParameter:" + name);
            }

            // on créé le paramètre
            SqlParameter retour = new SqlParameter();

            //retour.LocaleId = 1036; // french
            // on défini ses paramètres
            retour.ParameterName = name;
            retour.Direction = ParameterDirection.Input;
            retour.Value = input;
            // si on a un type
            if (type != null)

                // on en met un, sinon par besoin : la valeur est DSBNull
                retour.SqlDbType = type.Value;

            return retour;
        }

        /// <summary>
        /// Créé un paramètre de sortie
        /// </summary>
        /// <param name="name">Le nom du paramètre</param>
        /// <param name="type">Le type du paramètre</param>
        /// <returns>Un SqlParameter</returns>
        public static SqlParameter CreateOutputParameter(string name, SqlDbType type)
        {
            // on créé le paramètre
            SqlParameter retour = new SqlParameter();

            // on défini ses paramètres
            retour.ParameterName = name;
            retour.Direction = ParameterDirection.Output;
            retour.SqlDbType = type;

            // si c'est une nvarchar
            if (type == SqlDbType.NVarChar)

                // on défini une taille par défaut
                retour.Size = -1;
            // si c'est un décimal
            else if (type == SqlDbType.Decimal)

                // on défini une précision par défaut
                retour.Scale = 2;

            return retour;
        }

        /// <summary>
        /// Créé un paramètre de sortie
        /// </summary>
        /// <param name="name">Le nom du paramètre</param>
        /// <param name="size">Le taille du paramètre</param>
        /// <returns>Un SqlParameter</returns>
        public static SqlParameter CreateOutputStringParameter(string name, int size)
        {
            // on créé le paramètre
            SqlParameter retour = new SqlParameter();

            // on défini ses paramètres
            retour.ParameterName = name;
            retour.Size = size;
            retour.Direction = ParameterDirection.Output;
            retour.SqlDbType = SqlDbType.NVarChar;

            return retour;
        }

        /// <summary>
        /// Convert a string a to DB value
        /// </summary>
        /// <param name="input">the input string</param>
        /// <returns>an object</returns>
        public static object StringToDB(string input)
        {
            object retour = DBNull.Value;

            // Mis en commentaire ... plus besoin de mettre un '' pour les proc stockées
            //if (!string.IsNullOrEmpty(input))
            //    retour = input.Replace("'", "''");

            return input;
        }

        /// <summary>
        /// Convert a positive int a to DB value
        /// </summary>
        /// <param name="input">the input int</param>
        /// <returns>an object</returns>
        public static object UIntToDB(int input)
        {
            object retour = DBNull.Value;

            if (input >= 0)
                retour = input;

            return retour;
        }

        /// <summary>
        /// Convert a positive decimal a to DB value
        /// </summary>
        /// <param name="input">the input decimal</param>
        /// <returns>an object</returns>
        public static object UDecimalToDB(decimal input)
        {
            object retour = DBNull.Value;

            if (input >= 0)
                retour = input;

            return retour;
        }

        /// <summary>
        /// Convert a Guid a to DB value
        /// </summary>
        /// <param name="input">the input Guid</param>
        /// <returns>an object</returns>
        public static object GuidToDB(Guid input)
        {
            object retour = DBNull.Value;

            if (input != Guid.Empty)
                retour = input;

            return retour;
        }

        /// <summary>
        /// Convert a DateTime a to DB value
        /// </summary>
        /// <param name="input">the input DateTime</param>
        /// <returns>an object</returns>
        public static object DateTimeToDB(DateTime input)
        {
            object retour = DBNull.Value;

            if (input != DateTime.MinValue)
                retour = input;

            return retour;
        }

        /// <summary>
        /// Convert a DB value to a string
        /// </summary>
        /// <param name="value">the input value</param>
        /// <returns>a string</returns>
        public static string DBToString(object value)
        {
            //object retour = DBNull.Value;

            //if (value != null)
            //    retour = value.ToString().Replace("''", "'");

            return value.ToString();
        }

        #endregion Tools
    }
}