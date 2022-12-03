using VOR.Core.Domain;
using VOR.Core.Contract;
using VOR.Core.UnitOfWork;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using System;
using VOR.Utils;

namespace VOR.Core.Repository.NH.Repositories
{
    public class ChambreRepository : Repository<Chambre, int>, IChambreRepository
    {
        public ChambreRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public IList<Chambre> GetChambreByEventID(int eventID)
        {
            IList<Chambre> results = null;
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                ICriteria criteriaQuery = session.CreateCriteria(typeof(Chambre));
                criteriaQuery.Add(Restrictions.Eq("Evenement.ID", eventID));

                results = criteriaQuery.List<Chambre>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }

            return results;
        }

        public IList<Chambre> GetChambreByEventIDAndVilleID(int eventID, int villeID, int? agenceID)
        {
            Chambre chambre = null;
            Hotel hotel = null;

            var queryOver = SessionFactory.GetCurrentSession().QueryOver<Chambre>(() => chambre)
                .JoinAlias(() => chambre.Hotel, () => hotel)
                .Where(Restrictions.Eq("Evenement.ID", eventID))
                .And(Restrictions.Eq("hotel.Ville.ID", villeID));

            if(agenceID.HasValue)
                return queryOver.And(Restrictions.Eq("chambre.Agence.ID", agenceID)).List();

            return queryOver.List();
        }

        public IList<Chambre> GetChambreByListEventIDAndVilleID(List<int> eventIDs, int? villeID)
        {
            Chambre chambre = null;
            Hotel hotel = null;

            try
            {
                if(villeID != null)
                    return SessionFactory.GetCurrentSession().QueryOver<Chambre>(() => chambre)
                         .JoinAlias(() => chambre.Hotel, () => hotel)
                         .And(Restrictions.Eq("hotel.Ville.ID", villeID))
                         .WhereRestrictionOn(n => n.Evenement.ID).IsIn(eventIDs)
                         .List();
                else
                    return SessionFactory.GetCurrentSession().QueryOver<Chambre>(() => chambre)
                         .JoinAlias(() => chambre.Hotel, () => hotel)
                         .WhereRestrictionOn(n => n.Evenement.ID).IsIn(eventIDs)
                         .List();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }
        }

        public bool IsChambreMakkahSupprimable(decimal id)
        {
            return SessionFactory.GetBooleanByNamedQuery("IsChambreMakkahSupprimable", id, "Id");
        }

        public bool IsChambreMedineSupprimable(decimal id)
        {
            return SessionFactory.GetBooleanByNamedQuery("IsChambreMedineSupprimable", id, "Id");
        }

        public bool DeletePelerinsChambreMakkah(IList<int> lstIdPelerin)
        {
            ISession session = SessionFactory.GetCurrentSession();
            try
            {
                IQuery query = session.GetNamedQuery("DeletePelerinsChambreMakkah");
                query.SetParameterList("lstIdPelerin", lstIdPelerin);
                query.UniqueResult();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return false;
            }
        }

        public bool DeletePelerinsChambreMedine(IList<int> lstIdPelerin)
        {
            ISession session = SessionFactory.GetCurrentSession();
            try
            {
                IQuery query = session.GetNamedQuery("DeletePelerinsChambreMedine");
                query.SetParameterList("lstIdPelerin", lstIdPelerin);
                query.UniqueResult();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return false;
            }
        }

        public IList<Chambre> GetChambreByEventEnCoursAndVilleID(int villeID)
        {
            Chambre chambre = null;
            Hotel hotel = null;
            Evenement evenement = null;

            return SessionFactory.GetCurrentSession().QueryOver<Chambre>(() => chambre)
                    .JoinAlias(() => chambre.Hotel, () => hotel)
                    .JoinAlias(() => chambre.Evenement, () => evenement)
                    .Where(Restrictions.Eq("evenement.EnCours", true))
                    .And(Restrictions.Eq("hotel.Ville.ID", villeID))
                    .List();
        }

        public bool isNumeroChambreExist(string numeroChambre, int chambreID)
        {
            Chambre chambre = null;
            Evenement evenement = null;

            IList<Chambre> lstChambre = SessionFactory.GetCurrentSession().QueryOver<Chambre>(() => chambre)
                    .JoinAlias(() => chambre.Evenement, () => evenement)
                    .Where(Restrictions.Eq("evenement.EnCours", true))
                    .And(Restrictions.Eq("chambre.Numero", numeroChambre))
                    .And(!Restrictions.Eq("chambre.ID", chambreID))
                    .List();

            if (lstChambre != null && lstChambre.Count > 0)
                return true;
            else
                return false;
        }
    }
}