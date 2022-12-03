namespace VOR.Core.UnitOfWork
{
    public interface IUnitOfWorkRepository
    {
        void PersistCreationOf(IAggregateRoot entity);

        void PersistUpdateOf(IAggregateRoot entity);

        void PersistDeletionOf(IAggregateRoot entity);
    }
}