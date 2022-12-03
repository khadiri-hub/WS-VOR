using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class PelerinTypeModel : BaseModel<TypePelerin, ITypePelerinRepository>
    {
        public PelerinTypeModel(ITypePelerinRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}