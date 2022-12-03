using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using VOR.Core.Contract;
using VOR.Core.Repository.NH;
using VOR.Core.Repository.NH.Repositories;
using VOR.Core.UnitOfWork;
using System;
using Castle.MicroKernel.ModelBuilder;
using Castle.MicroKernel;
using Castle.Core;
using Castle.MicroKernel.SubSystems.Configuration;

namespace VOR.Core.Model
{
    public static class Bootstrapper
    {
        public static IWindsorContainer Start()
        {
            var container = new WindsorContainer();
            container.Register(
                Component.For<IUnitOfWork>().ImplementedBy<NHUnitOfWork>().LifestylePerWebRequest(),
                Component.For<PersonneModel>(),
                Component.For<IPersonneRepository>().ImplementedBy<PersonneRepository>().LifestylePerWebRequest(),
                Component.For<PnrModel>(),
                Component.For<IPnrRepository>().ImplementedBy<PnrRepository>().LifestylePerWebRequest(),
                Component.For<ProgrammeModel>(),
                Component.For<IProgrammeRepository>().ImplementedBy<ProgrammeRepository>().LifestylePerWebRequest(),
                Component.For<PelerinModel>(),
                Component.For<IPelerinRepository>().ImplementedBy<PelerinRepository>().LifestylePerWebRequest(),
                Component.For<HotelModel>(),
                Component.For<IHotelRepository>().ImplementedBy<HotelRepository>().LifestylePerWebRequest(),
                Component.For<EvenementModel>(),
                Component.For<IEvenementRepository>().ImplementedBy<EvenementRepository>().LifestylePerWebRequest(),
                Component.For<EtatCivilModel>(),
                Component.For<IEtatCivilRepository>().ImplementedBy<EtatCivilRepository>().LifestylePerWebRequest(),
                Component.For<TypeChambreModel>(),
                Component.For<ITypeChambreRepository>().ImplementedBy<TypeChambreRepository>().LifestylePerWebRequest(),
                Component.For<ChambreModel>(),
                Component.For<IChambreRepository>().ImplementedBy<ChambreRepository>().LifestylePerWebRequest(),
                Component.For<TypePersonneModel>(),
                Component.For<ITypePersonneRepository>().ImplementedBy<TypePersonneRepository>().LifestylePerWebRequest(),
                Component.For<SexeModel>(),
                Component.For<ISexeRepository>().ImplementedBy<SexeRepository>().LifestylePerWebRequest(),
                Component.For<CompAerienneModel>(),
                Component.For<ICompAerienneRepository>().ImplementedBy<CompAerienneRepository>().LifestylePerWebRequest(),
                Component.For<AgenceModel>(),
                Component.For<IAgenceRepository>().ImplementedBy<AgenceRepository>().LifestylePerWebRequest(),
                Component.For<VilleModel>(),
                Component.For<IVilleRepository>().ImplementedBy<VilleRepository>().LifestylePerWebRequest(),
                Component.For<VolModel>(),
                Component.For<IVolRepository>().ImplementedBy<VolRepository>().LifestylePerWebRequest(),
                Component.For<UtilisateurModel>(),
                Component.For<IUtilisateurRepository>().ImplementedBy<UtilisateurRepository>().LifestylePerWebRequest(),
                Component.For<TypeUtilisateurModel>(),
                Component.For<ITypeUtilisateurRepository>().ImplementedBy<TypeUtilisateurRepository>().LifestylePerWebRequest(),
                Component.For<TypeAgenceModel>(),
                Component.For<ITypeAgenceRepository>().ImplementedBy<TypeAgenceRepository>().LifestylePerWebRequest(),
                Component.For<ITypeProgrammeRepository>().ImplementedBy<TypeProgrammeRepository>().LifestylePerWebRequest(),
                Component.For<TypeProgrammeModel>(),
                Component.For<IRecuRepository>().ImplementedBy<RecuRepository>().LifestylePerWebRequest(),
                Component.For<RecuModel>(),
                Component.For<IRefRegionRepository>().ImplementedBy<RefRegionRepository>().LifestylePerWebRequest(),
                Component.For<RefRegionModel>(),
                Component.For<IRefVilleRepository>().ImplementedBy<RefVilleRepository>().LifestylePerWebRequest(),
                Component.For<RefVilleModel>(),



                Component.For<IAlerteRepository>().ImplementedBy<AlerteRepository>().LifestylePerWebRequest(),
                Component.For<AlerteModel>(),


                Component.For<ITypeAlerteRepository>().ImplementedBy<TypeAlerteRepository>().LifestylePerWebRequest(),
                Component.For<TypeAlerteModel>(),
                Component.For<IVisaRepository>().ImplementedBy<VisaRepository>().LifestylePerWebRequest(),
                Component.For<VisaModel>(),
                Component.For<IStatutPelerinRepository>().ImplementedBy<StatutPelerinRepository>().LifestylePerWebRequest(),
                Component.For<StatutPelerinModel>(),
                Component.For<IMotifStatutPelerinRepository>().ImplementedBy<MotifStatutPelerinRepository>().LifestylePerWebRequest(),
                Component.For<MotifStatutPelerinModel>(),


                Component.For<ITypePelerinRepository>().ImplementedBy<TypePelerinRepository>().LifestylePerWebRequest(),
                Component.For<PelerinTypeModel>(),


                  Component.For<IVaccinRepository>().ImplementedBy<VaccinRepository>().LifestylePerWebRequest(),
                Component.For<VaccinModel>()
            );

            VOR.Core.Repository.NH.SessionFactory.InitSession();
            return container;
        }
    }
}