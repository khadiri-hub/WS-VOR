﻿using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IParkingQuotaRepository : IRepository<ParkingQuota, int>
    {
    }
}