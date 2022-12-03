using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Domain.Vues;
using VOR.Core.UnitOfWork;
using VOR.Utils;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace VOR.Core.Repository.NH.Repositories
{
    public class CalendrierRepository : Repository<Calendrier, int>, ICalendrierRepository
    {
        public CalendrierRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public IList<Calendrier> GetListJours(int num_evenement)
        {
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                var lstCals = from cals in session.Query<Calendrier>()
                              where cals.EvenementID == num_evenement
                              select cals;

                return lstCals.ToList<Calendrier>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error("Exception Calendrier GetListJours", ex);
                return null;
            }
        }

        public IList<Calendrier> GetListJoursInvitation(int num_evenement)
        {
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                var lstCals = from cals in session.Query<Calendrier>()
                              where cals.EvenementID == num_evenement && cals.EstInvitation
                              select cals;

                return lstCals.ToList<Calendrier>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error("Exception Calendrier GetListJours", ex);
                return null;
            }
        }

        public IList<Calendrier> GetListJoursAcces(int num_evenement)
        {
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                var lstCals = from cals in session.Query<Calendrier>()
                              where cals.EvenementID == num_evenement && cals.EstAcces
                              select cals;

                return lstCals.ToList<Calendrier>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error("Exception Calendrier GetListJours", ex);
                return null;
            }
        }

        public IList<Calendrier> GetListJoursTransport(int num_evenement)
        {
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                var lstCals = from cals in session.Query<Calendrier>()
                              where cals.EvenementID == num_evenement && cals.EstTransport
                              select cals;

                return lstCals.ToList<Calendrier>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error("Exception Calendrier GetListJoursTransport", ex);
                return null;
            }
        }

        public IList<Calendrier> GetByPerId(int perID, int eventID)
        {
            IList<Calendrier> results = null;

            ISession s = SessionFactory.GetCurrentSession();
            try
            {
                string hql = "select accred.JourList from Accreditation accred where accred.EvtID = :eventID and accred.PerID =  :perID";
                results = s.CreateQuery(hql)
                               .SetParameter("eventID", eventID)
                               .SetParameter("perID", perID)
                               .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Calendrier))).List<Calendrier>();
                return results;
            }
            catch (Exception ex)
            {
                Logger.Current.Error("Erreur GetById", ex);
            }

            return results;
        }

        public void UpdateJoursAccred(IList<DateTime> sem, int idAccred)
        {
        }

        public IList<Calendrier> GetAllByDate(List<DateTime> dt, int eventID)
        {
            IList<Calendrier> cals = new List<Calendrier>();

            try
            {
                cals = SessionFactory.GetCurrentSession().CreateCriteria(typeof(Calendrier))
                    .Add(Restrictions.Eq("EvenementID", eventID))
                    .Add(Restrictions.In("DateJour", dt))
                    .List<Calendrier>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error("Erreur GetAllByDate", ex);
            }

            return cals;
        }

        public IList<int> GetAllIdByDate(List<DateTime> dt, int eventID)
        {
            IList<int> listId = new List<int>();

            try
            {
                IList<Calendrier> cals = new List<Calendrier>();
                cals = SessionFactory.GetCurrentSession().CreateCriteria(typeof(Calendrier))
                    .Add(Restrictions.Eq("EvenementID", eventID))
                    .Add(Restrictions.In("DateJour", dt))
                    .List<Calendrier>();

                foreach (Calendrier cal in cals)
                {
                    listId.Add(cal.ID);
                }
            }
            catch (Exception ex)
            {
                Logger.Current.Error("Erreur GetAllByDate", ex);
            }

            return listId;
        }

        public IList<Calendrier> GetJoursTournoisByEvtAndAcces(int eventID, int dlpID)
        {
            IList<Calendrier> results = new List<Calendrier>();

            try
            {
                IQuery query = SessionFactory.GetCurrentSession().GetNamedQuery("GetJoursTournoisByEvtAndAcces");
                query.SetParameter("eventId", eventID);
                query.SetParameter("dlpId", dlpID);

                results = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Calendrier))).List<Calendrier>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error("Erreur GetJoursTournoisByEvtAndAcces", ex);
            }

            return results;
        }

        public Calendrier GetByNumJournee(int numJournee, int eventId)
        {
            try
            {
               return SessionFactory.GetCurrentSession().QueryOver<Calendrier>()
                    .Where(c => c.NumJournee == numJournee && c.EvenementID == eventId).List().FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                throw;
            }
        }

        public IList<Calendrier> GetListByIds(List<int> ids)
        {
            List<Calendrier> results = null;
            ISession session = SessionFactory.GetCurrentSession();
            try
            {
                var cals = from cal in session.Query<Calendrier>().OrderBy(p => p.ID)
                           where (ids.Contains(cal.ID))
                           select cal;

                results = cals.ToList<Calendrier>();
                return results;
            }
            catch (Exception ex)
            {
                Logger.Current.Error("Calendrier GetListByIds", ex);
                throw;
            }
        }

        public IList<Calendrier> GetListJoursBetweenNumJournee(int numJourneeDebut, int numJourneeFin, int eventId)
        {
            IList<Calendrier> semaines = new List<Calendrier>();

            try
            {
                IQuery query = SessionFactory.GetCurrentSession().GetNamedQuery("GetListJoursBetweenNumJournee");
                query.SetInt32 ("numJourneeDebut", numJourneeDebut);
                query.SetInt32("numJourneeFin", numJourneeFin);
                query.SetInt32("eventId", eventId);

                return query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Calendrier)))
                    .List<Calendrier>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }
        }
    }
}