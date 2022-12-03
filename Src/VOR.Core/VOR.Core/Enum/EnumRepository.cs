using System;

namespace VOR.Core.Enum
{
    public class EnumRepository
    {
        public enum IntervenantDroit : int
        {
            CoordinateurFFT = 1,
            DemandeurFFT = 2,
            ReferentFFT = 3,
            Prestataire = 4,
        }

        public enum TitreAccesTypeEnum
        {
            ParkingQuotidien = 1,
            Bracelet = 2,
            Badge = 3,
            ParkingPermanent = 4
        }

        public enum TitreAccesSupportEnum
        {
            EBillet = 1,
            Bracelet = 2,
            Sticker = 3
        }
    }
}