using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class TypeAlerteModel : BaseModel<TypeAlerte, ITypeAlerteRepository>
    {
        public TypeAlerteModel(ITypeAlerteRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}