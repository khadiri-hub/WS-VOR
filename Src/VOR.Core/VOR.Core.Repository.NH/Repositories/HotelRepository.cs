using VOR.Core.Domain;
using VOR.Core.Contract;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class HotelRepository : Repository<Hotel, int>, IHotelRepository
    {
        public HotelRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public bool IsHotelSupprimable(decimal id)
        {
            return SessionFactory.GetBooleanByNamedQuery("IsHotelSupprimable", id, "Id");
        }
    }
}