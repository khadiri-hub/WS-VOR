
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Model;
using VOR.Core.UnitOfWork;

namespace VOR.Core
{
    public class TypePersonneModel : BaseModel<TypePersonne, ITypePersonneRepository, int>
    {
        public TypePersonneModel(ITypePersonneRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}