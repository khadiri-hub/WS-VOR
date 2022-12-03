
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Model;
using VOR.Core.UnitOfWork;

namespace VOR.Core
{
    public class TypeVehiculeModel : BaseModel<TypeVehicule, ITypeVehiculeRepository, int>
    {
        public TypeVehiculeModel(ITypeVehiculeRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}