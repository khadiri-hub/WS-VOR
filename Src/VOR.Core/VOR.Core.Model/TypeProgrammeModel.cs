
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Model;
using VOR.Core.UnitOfWork;

namespace VOR.Core
{
    public class TypeProgrammeModel : BaseModel<TypeProgramme, ITypeProgrammeRepository, int>
    {
        public TypeProgrammeModel(ITypeProgrammeRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}