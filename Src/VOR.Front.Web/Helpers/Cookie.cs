#region Using directives

using System;
using System.Web;
using VOR.Utils;

#endregion Using directives

public class Cookie
{
    private readonly string _cookieName;
    private readonly string _domaine;
    private readonly int? _expire;

    public Cookie(string cookieName)
        : this(cookieName, null)
    {
    }

    public Cookie(string cookieName, int? expireDuration)
        : this(cookieName, expireDuration, null)
    {
    }

    public Cookie(string cookieName, int? expireDuration, string domaineName)
    {
        this._cookieName = cookieName;
        this._domaine = domaineName;
        this._expire = expireDuration;
    }

    /// <summary>
    /// Removes the cookie by clearing it out and expiring it immediately.
    /// </summary>
    /// <returns>Void</returns>
    public void Remove()
    {
        HttpCookie Cookie = HttpContext.Current.Request.Cookies[this._cookieName];

        if (Cookie != null)
        {
            Cookie.Expires = DateTime.Now.AddDays(-2);
            HttpContext.Current.Response.Cookies.Add(Cookie);
        }
    }

    public void EnsureCookieIntegrity()
    {
        HttpCookie cookie = HttpContext.Current.Request.Cookies[_cookieName];

        if (cookie != null && string.IsNullOrEmpty(HttpContext.Current.Response.Cookies[_cookieName].Value))
        {
            HttpContext.Current.Response.Cookies[_cookieName].Value = cookie.Value;
            HttpContext.Current.Response.Cookies[_cookieName].Domain = this._domaine;
            HttpContext.Current.Request.Cookies[_cookieName].Value = cookie.Value;
            HttpContext.Current.Request.Cookies[_cookieName].Domain = this._domaine;
        }
    }

    public void WriteCookieEntry(string key, string value)
    {
        string encKey = this.Hash(key);
        // For security, all '=' are removed
        encKey = encKey.Replace("=", string.Empty);

        EnsureCookieIntegrity();

        string encValue = (string.IsNullOrEmpty(value)) ? null : this.Encrypt(value);
        HttpCookie newCookie = new HttpCookie(_cookieName);
        newCookie[encKey] = encValue;
        if (this._expire != null)
            newCookie.Expires = DateTime.Now.AddDays((int)this._expire);

        HttpContext.Current.Response.Cookies[_cookieName].Domain = this._domaine;
        HttpContext.Current.Response.Cookies[_cookieName][encKey] = encValue;
        if (this._expire != null)
            HttpContext.Current.Response.Cookies[_cookieName].Expires = DateTime.Now.AddDays((int)this._expire);
    }

    public string ReadCookieEntry(string key)
    {
        string encKey = this.Hash(key);
        // For security, all '=' are removed
        encKey = encKey.Replace("=", String.Empty);

        EnsureCookieIntegrity();

        string encValue = HttpContext.Current.Response.Cookies[_cookieName][encKey];

        if (string.IsNullOrEmpty(encValue))
        {
            encValue = HttpContext.Current.Request.Cookies[_cookieName][encKey];
        }
        return this.Decrypt(encValue);
    }

    public T? ReadCookieEntry<T>(string key) where T : struct
    {
        string cookieEntry = ReadCookieEntry(key);

        try
        {
            if (!string.IsNullOrEmpty(cookieEntry))
            {
                return (T?)Convert.ChangeType(cookieEntry, typeof(T?));
            }
        }
        catch (Exception ex)
        {
            Logger.Current.Error("Erreur ReadCookieEntry", ex);
        }
        return null;
    }

    protected string Encrypt(string original)
    {
        // Encrypt the string
        return SymmetricAlgorithm.GetInstance().Encrypt(original);
    }

    protected string Decrypt(string encrypted)
    {
        // Decrypt the string
        return SymmetricAlgorithm.GetInstance().Decrypt(encrypted);
    }

    protected string Hash(string original)
    {
        // Encrypt the string
        return HashAlgorithm.GetInstance().Hash(original);
    }
}