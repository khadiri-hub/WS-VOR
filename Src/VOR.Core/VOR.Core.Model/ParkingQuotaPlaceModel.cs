﻿using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class ParkingQuotaPlaceModel : BaseModel<ParkingQuotaPlace, IParkingQuotaPlaceRepository>
    {
        public ParkingQuotaPlaceModel(IParkingQuotaPlaceRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

       
    }
}