using NHibernate.Dialect;
using NHibernate.Dialect.Function;

namespace VOR.Core.Repository.NH
{
    public class GestourDialect : MsSql2008Dialect
    {
        public GestourDialect()
        {
            RegisterFunction("GetDate", new StandardSQLFunction("GetDate"));
            RegisterFunction("dbo.DecryptPwd", new StandardSQLFunction("dbo.DecryptPwd"));
            RegisterFunction("dbo.EncryptPwd", new StandardSQLFunction("dbo.EncryptPwd"));
            RegisterFunction("dbo.sf_RemoveExtraChars", new StandardSQLFunction("dbo.sf_RemoveExtraChars"));
        }
    }
}