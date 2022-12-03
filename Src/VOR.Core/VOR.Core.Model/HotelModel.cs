using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class HotelModel : BaseModel<Hotel, IHotelRepository>
    {
        public HotelModel(IHotelRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public bool IsHotelSupprimable(decimal id)
        {
            return _repository.IsHotelSupprimable(id);
        }
    }
}