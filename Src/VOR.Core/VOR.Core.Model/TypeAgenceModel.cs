
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Model;
using VOR.Core.UnitOfWork;

namespace VOR.Core
{
    public class TypeAgenceModel : BaseModel<TypeAgence, ITypeAgenceRepository, int>
    {
        public TypeAgenceModel(ITypeAgenceRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}