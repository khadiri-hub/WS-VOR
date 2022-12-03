using NHibernate;

namespace VOR.Core.Repository.NH.SessionStorage
{
    public interface ISessionStorageContainer
    {
        ISession GetCurrentSession();

        void Store(ISession session);
    }
}