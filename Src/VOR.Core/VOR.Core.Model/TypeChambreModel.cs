using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class TypeChambreModel : BaseModel<TypeChambre, ITypeChambreRepository>
    {
        public TypeChambreModel(ITypeChambreRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public bool IsTypeChambreSupprimable(decimal id)
        {
            return _repository.IsTypeChambreSupprimable(id);
        }
    }
}