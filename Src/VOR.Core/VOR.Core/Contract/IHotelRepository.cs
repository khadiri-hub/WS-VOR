using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IHotelRepository : IRepository<Hotel, int>
    {
        bool IsHotelSupprimable(decimal id);
    }
}