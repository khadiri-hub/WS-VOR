#region Using directives

using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using VOR.Utils;

#endregion Using directives

public class QueryStringManager : ICloneable
{
    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    public QueryStringManager()
    {
        if (HttpContext.Current != null &&
            HttpContext.Current.Handler != null &&
            HttpContext.Current.Handler is System.Web.UI.Page)
        {
            _internalCurrentPage = HttpContext.Current.Handler as System.Web.UI.Page;
            FromUrl(_internalCurrentPage);
        }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public QueryStringManager(System.Web.UI.Page currentPage)
    {
        FromUrl(currentPage);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public QueryStringManager(string url)
    {
        FromUrl(url);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public QueryStringManager(Uri uri)
    {
        FromUrl(uri.AbsoluteUri);
    }

    // ------------------------------------------------------------------

    #endregion Constructors

    #region Public Properties

    public string this[string index]
    {
        get
        {
            return _internalQS[index];
        }
        set
        {
            _internalQS[index] = value;
        }
    }

    public string CompleteUrl
    {
        get
        {
            return BaseUrl + Make();
        }
        set
        {
            FromUrl(value);
        }
    }

    public string BaseUrl
    {
        get
        {
            return _internalBaseUrl;
        }
        set
        {
            _internalBaseUrl = value;
        }
    }

    public QueryStringParametersCollection Parameters
    {
        get
        {
            QueryStringParametersCollection col = new QueryStringParametersCollection();
            foreach (string s in _internalQS)
            {
                QueryStringElement qse = new QueryStringElement();
                qse.Name = s;
                qse.Value = _internalQS[s];
                col.Add(qse);
            }
            return col;
        }
    }

    // ------------------------------------------------------------------

    #endregion Public Properties

    #region Public Methods

    static public string AppendQueryString(string url, string qs)
    {
        string result = url.TrimEnd('?', '&');

        if (result.IndexOf("?") >= 0)
        {
            return url + "&" + qs;
        }
        else
        {
            return url + "?" + qs;
        }
    }

    public bool HasParameter(string parameterName)
    {
        if (parameterName == null ||
            parameterName.Trim().Length <= 0)
        {
            return false;
        }
        else
        {
            parameterName = parameterName.Trim();
            string v = this[parameterName];

            if (v == null || v.Trim().Length <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public void RedirectResponse()
    {
        HttpContext.Current.Response.Redirect(string.Concat(this.BaseUrl, this.Make()));
    }

    public void SetParameter(string name, string val)
    {
        _internalQS[name] = val;
    }

    public void RemoveParameter(string name)
    {
        _internalQS.Remove(name);
    }

    public void RemoveCompleteUrlParameters()
    {
        _internalQS.Clear();
    }

    public string GetParameter(string name)
    {
        string result = _internalQS[name];

        if (result == null || result.Length == 0)
        {
            if (_internalCurrentPage != null &&
                _internalCurrentPage.Request != null &&
                _internalCurrentPage.Request.Form != null)
            {
                result = _internalCurrentPage.Request.Form[name];
            }

            //// try session, also.
            //if (result == null || result.Length == 0)
            //{
            //    if (_internalCurrentPage != null &&
            //        _internalCurrentPage.Session != null)
            //    {
            //        object o = _internalCurrentPage.Session[name];
            //        if (o != null)
            //        {
            //            result = o.ToString();
            //        }
            //    }
            //}

            //// Try cookies, also.
            //if (result == null || result.Length == 0)
            //{
            //    if (_internalCurrentPage != null &&
            //        _internalCurrentPage.Request != null &&
            //        _internalCurrentPage.Request.Cookies != null)
            //    {
            //        HttpCookie c = _internalCurrentPage.Request.Cookies[name];
            //        if (c != null)
            //        {
            //            result = c.Value;
            //        }
            //    }
            //}
        }

        if (result == null)
        {
            result = string.Empty;
        }

        return result;
    }

    /// <summary>
    /// Retourne la valeur contenue dans l'url associée à la clé en paramètre, ou la valeur par défaut du type
    /// </summary>
    /// <typeparam name="T">Type attendu pour la valeur</typeparam>
    /// <param name="name">Clé de la valeur</param>
    /// <returns></returns>
    public T GetParameter<T>(string name)
    {
        return GetParameter<T>(name, false);
    }

    /// <summary>
    /// Retourne la valeur contenue dans l'url associée à la clé en paramètre, ou la valeur par défaut du type.
    /// </summary>
    /// <typeparam name="T">Type attendu pour la valeur</typeparam>
    /// <param name="name">Clé de la valeur</param>
    /// <param name="safeMode">Permet de préciser si on souhaite catcher l'erreur de conversion ou non. ATTENTION cela a des conséquences sur les perfs. False par défaut</param>
    /// <returns></returns>
    public T GetParameter<T>(string name, bool safeMode)
    {
        string value = _internalQS[name];

        if (string.IsNullOrEmpty(value))
        {
            if (_internalCurrentPage != null &&
                _internalCurrentPage.Request != null &&
                _internalCurrentPage.Request.Form != null)
            {
                value = _internalCurrentPage.Request.Form[name];
            }
        }

        if (typeof(T) == typeof(string))    // Si on veut un string, on sait qu'on aura aucun mal à caster
            return (T)(object)value;        // un string en string

        if (value == null)          // Si on n'a pas trouvé la clé ou qu'elle n'a pas de valeur
            return default(T);      // on renvoit la valeur par défaut du type

        if (safeMode)   // Si le safeMode est enclenché, on utilise un try catch pour éviter les erreurs de conversion
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }
        else    // Sinon on leve normalement l'erreur si elle se produit
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }

    #endregion Public Methods

    #region Reading from an URL

    public void FromUrl(System.Web.UI.Page page)
    {
        if (page != null)
        {
            _internalCurrentPage = page;
            FromUrl(_internalCurrentPage.Request.RawUrl);
        }
    }

    public void FromUrl(Uri uri)
    {
        if (uri != null)
        {
            FromUrl(uri.AbsoluteUri);
        }
    }

    public void FromUrl(string url)
    {
        if (url != null)
        {
            _internalQS.Clear();

            // store the part before, too.
            int qPos = url.IndexOf("?");
            if (qPos >= 0)
            {
                BaseUrl = url.Substring(0, qPos - 0);
                url = url.Substring(qPos + 1);
            }
            else
            {
                BaseUrl = url;
            }

            if (url.Length > 0 && url.Substring(0, 1) == "?")
            {
                url = url.Substring(1);
            }

            // break the values.
            string[] pairs = url.Split('&');
            foreach (string pair in pairs)
            {
                string a = string.Empty;
                string b = string.Empty;

                string[] singular = pair.Split('=');

                int j = 0;
                foreach (string one in singular)
                {
                    if (j == 0)
                    {
                        a = one;
                    }
                    else
                    {
                        b = one;
                    }

                    j++;
                }

                // store.
                SetParameter(a, HttpUtility.UrlDecode(b));
            }
        }
    }

    #endregion Reading from an URL

    #region Make

    public string Make()
    {
        string result = "?";

        foreach (string name in _internalQS)
        {
            string val = _internalQS[name];

            if (val != null && val.Length > 0)
                result += name + "=" + HttpUtility.UrlEncode(val) + "&";
        }

        //return result;
        return result.TrimEnd('?', '&');
    }

    public string Make(string name1, string value1)
    {
        return Make(name1, value1,
                    string.Empty, string.Empty,
                    string.Empty, string.Empty,
                    string.Empty, string.Empty,
                    string.Empty, string.Empty);
    }

    public string Make(string name1, string value1, string name2, string value2)
    {
        return Make(name1, value1,
                    name2, value2,
                    string.Empty, string.Empty,
                    string.Empty, string.Empty,
                    string.Empty, string.Empty);
    }

    public string Make(string name1, string value1, string name2, string value2,
                       string name3, string value3)
    {
        return Make(
            name1, value1,
            name2, value2,
            name3, value3,
            string.Empty, string.Empty,
            string.Empty, string.Empty);
    }

    public string Make(
        string name1,
        string value1,
        string name2,
        string value2,
        string name3,
        string value3,
        string name4,
        string value4)
    {
        return Make(
            name1, value1,
            name2, value2,
            name3, value3,
            name4, value4,
            string.Empty, string.Empty);
    }

    public string Make(
        string name1,
        string value1,
        string name2,
        string value2,
        string name3,
        string value3,
        string name4,
        string value4,
        string name5,
        string value5)
    {
        string old5 = GetParameter(name5);
        string old4 = GetParameter(name4);
        string old3 = GetParameter(name3);
        string old2 = GetParameter(name2);
        string old1 = GetParameter(name1);

        SetParameter(name5, value5);
        SetParameter(name4, value4);
        SetParameter(name3, value3);
        SetParameter(name2, value2);
        SetParameter(name1, value1);

        string result = Make();

        SetParameter(name5, old5);
        SetParameter(name4, old4);
        SetParameter(name3, old3);
        SetParameter(name2, old2);
        SetParameter(name1, old1);

        return result;
    }

    #endregion Make

    #region ICloneable member

    // ------------------------------------------------------------------

    /// <summary>
    /// Makes a deep copy.
    /// </summary>
    public object Clone()
    {
        QueryStringManager dst = new QueryStringManager();

        dst._internalCurrentPage = this._internalCurrentPage;
        dst.BaseUrl = this.BaseUrl;

        // Clone.
        foreach (string key in this._internalQS.Keys)
        {
            dst._internalQS[key] = this._internalQS[key];
        }

        return dst;
    }

    // ------------------------------------------------------------------

    #endregion ICloneable member

    #region Private helper

    // ------------------------------------------------------------------

    /// <summary>
    /// The URL that comes before the actual name-value pair parameters.
    /// </summary>
    private string _internalBaseUrl = string.Empty;

    /// <summary>
    /// The page that is currently loaded.
    /// </summary>
    private Page _internalCurrentPage = null;

    /// <summary>
    /// The collection to store the name-value pairs.
    /// </summary>
    private NameValueCollection _internalQS = new NameValueCollection();

    // ------------------------------------------------------------------

    #endregion Private helper
}