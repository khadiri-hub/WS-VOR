
using System;
using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Model;
using VOR.Core.UnitOfWork;

namespace VOR.Core
{
    public class RefVilleModel : BaseModel<RefVille, IRefVilleRepository, int>
    {
        public RefVilleModel(IRefVilleRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public IList<RefVille> GetVilleByRegionID(int regionID)
        {
            return _repository.GetVilleByRegionID(regionID);
        }
    }
}