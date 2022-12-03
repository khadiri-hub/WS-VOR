using VOR.Core.UnitOfWork;
using VOR.Core.Contract;
using VOR.Core.Domain;
using NHibernate;
using System;
using VOR.Utils;
using System.Collections.Generic;
using NHibernate.Criterion;
using VOR.Core.Domain.Vues;

namespace VOR.Core.Repository.NH.Repositories
{
    public class PelerinRepository : Repository<Pelerin, int>, IPelerinRepository
    {
        public PelerinRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)

        { }

        public IList<VuePelerin> GetPelerinByEventIDAndAgenceID(int? eventID, int? agenceID, int? chambreID, int? statutPelerinID, int? motifStatutPelerinID)
        {
            IList<VuePelerin> results = null;
            ISession s = SessionFactory.GetCurrentSession();

            try
            {
                IQuery query = s.GetNamedQuery("GetPelerinByEventIDAndAgenceID");
                query = query.SetParameter("eventID", eventID);
                query = query.SetParameter("agenceID", agenceID);
                query = query.SetParameter("chambreID", chambreID);
                query = query.SetParameter("statutPelerinID", statutPelerinID);
                query = query.SetParameter("motifStatutPelerinID", motifStatutPelerinID);
                results = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(VuePelerin))).List<VuePelerin>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }

            return results;
        }

        public bool SetBadgeToDownload(bool toDownload, IList<int> lstIdPelerin)
        {
            ISession session = SessionFactory.GetCurrentSession();
            try
            {
                IQuery query = session.GetNamedQuery("SetBadgeToDownload");
                query.SetParameterList("lstIdPelerin", lstIdPelerin);
                query.SetBoolean("toDownload", toDownload);
                query.UniqueResult();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return false;
            }
        }

        public IList<Pelerin> GetPelerinToDownloadBadge(int agenceID)
        {
            IList<Pelerin> results = null;
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                ICriteria criteriaQuery = session.CreateCriteria(typeof(Pelerin));
                criteriaQuery.Add(Restrictions.Eq("BadgeToDownload", true));
                criteriaQuery.Add(Restrictions.Eq("Agence.ID", agenceID));

                results = criteriaQuery.List<Pelerin>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }

            return results;
        }

        public void RemovePelerinPelerin(int pelerinID1, int pelerinID2)
        {
            ISession session = SessionFactory.GetCurrentSession();
            try
            {
                IQuery query = session.GetNamedQuery("RemovePelerinPelerin");
                query.SetInt32("pelerinID1", pelerinID1);
                query.SetInt32("pelerinID2", pelerinID2);
                query.ExecuteUpdate();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
            }
        }

        public bool isColorExist(List<int> lstIdPelerinInRelation, string couleur, int eventID)
        {
            IList<TPelerinPelerin> results = null;
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                IQuery query = session.GetNamedQuery("GetListColorByPelerinIDAndColor");
                query = query.SetParameterList("pelerinIDs", lstIdPelerinInRelation);
                query = query.SetString("couleur", couleur);
                query = query.SetInt32("eventID", eventID);
                results = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(TPelerinPelerin))).List<TPelerinPelerin>();

                if (results != null && results.Count > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return false;
            }
        }

        public bool IsPelerinExistByYear(string nomFR, string prenomFR, int dateCreationYear)
        {
            IList<Pelerin> results = null;
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                IQuery query = session.GetNamedQuery("IsPelerinExistByYear");
                query = query.SetString("nomFR", nomFR);
                query = query.SetString("prenomFR", prenomFR);
                query = query.SetInt32 ("dateCreationYear", dateCreationYear);
                results = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Pelerin))).List<Pelerin>();

                if (results != null && results.Count > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return false;
            }
        }

        public IList<Pelerin> GetPelerinsByChambreID(int eventID, int chambreID)
        {
            IList<Pelerin> results = null;
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                ICriteria criteriaQuery = session.CreateCriteria(typeof(Pelerin));
                criteriaQuery.Add(Restrictions.Eq("Chambre.ID", chambreID));
                criteriaQuery.Add(Restrictions.Eq("Evenement.ID", eventID));

                results = criteriaQuery.List<Pelerin>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }

            return results;
        }

        public IList<VuePelerin> GetPelerinSansMakkahChambreByEventID(int eventID, int? agenceID)
        {
            IList<VuePelerin> results = null;
            ISession s = SessionFactory.GetCurrentSession();

            try
            {
                IQuery query = s.GetNamedQuery("GetPelerinSansMakkahChambreByEventID");
                query = query.SetInt32("eventID", eventID);
                query = query.SetParameter("agenceID", agenceID);
                results = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(VuePelerin))).List<VuePelerin>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }

            return results;
        }

        public IList<VuePelerin> GetPelerinSansMedineChambreByEventID(int eventID, int? agenceID)
        {
            IList<VuePelerin> results = null;
            ISession s = SessionFactory.GetCurrentSession();

            try
            {
                IQuery query = s.GetNamedQuery("GetPelerinSansMedineChambreByEventID");
                query = query.SetInt32("eventID", eventID);
                query = query.SetParameter("agenceID", agenceID);
                results = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(VuePelerin))).List<VuePelerin>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }

            return results;
        }

        public void SetStatutPelerin(int? statutPelerinID, int? motifStatutPelerinID, IList<int> lstIdPelerin)
        {
            ISession session = SessionFactory.GetCurrentSession();
            try
            {
                IQuery query = session.GetNamedQuery("SetStatutPelerin");
                query.SetParameter("statutPelerinID", statutPelerinID);
                query.SetParameter("motifStatutPelerinID", motifStatutPelerinID);
                query.SetParameterList("lstIdPelerin", lstIdPelerin);

                query.ExecuteUpdate();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
            }
        }

        public IList<VuePelerin> GetPelerinSansMakkahChambreByEventEncours(List<int> lstEventIds, int? agenceID)
        {
            IList<VuePelerin> results = null;
            ISession s = SessionFactory.GetCurrentSession();

            try
            {
                IQuery query = s.GetNamedQuery("GetPelerinSansMakkahChambreByEventEncours");
                query = query.SetParameterList("lstEventIds", lstEventIds);
                query = query.SetParameter("agenceID", agenceID);
                results = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(VuePelerin))).List<VuePelerin>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }

            return results;
        }

        public IList<VuePelerin> GetPelerinSansMedineChambreByEventEncours(List<int> lstEventIds, int? agenceID)
        {
            IList<VuePelerin> results = null;
            ISession s = SessionFactory.GetCurrentSession();

            try
            {
                IQuery query = s.GetNamedQuery("GetPelerinSansMedineChambreByEventEncours");
                query = query.SetParameterList("lstEventIds", lstEventIds);
                query = query.SetParameter("agenceID", agenceID);
                results = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(VuePelerin))).List<VuePelerin>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }

            return results;
        }


        public IList<Pelerin> GetPelerinsByEventID(int? eventID)
        {
            IList<Pelerin> results = null;
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                ICriteria criteriaQuery = session.CreateCriteria(typeof(Pelerin));
                criteriaQuery.Add(Restrictions.Eq("Evenement.ID", eventID));

                results = criteriaQuery.List<Pelerin>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }

            return results;
        }

        public int GetNbrPelerinsByEventID(int? eventID)
        {
            int count = 0;
            IList<Pelerin> results = null;
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                ICriteria criteriaQuery = session.CreateCriteria(typeof(Pelerin));
                criteriaQuery.Add(Restrictions.Eq("Evenement.ID", eventID));

                results = criteriaQuery.List<Pelerin>();
                count = results == null ? 0 : results.Count;
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return count;
            }

            return count;
        }

        public int GetNbrPelerinsByEventIDs(List<int> eventIds)
        {
            try
            {
                IList<Pelerin> result = null;

                ISession s = SessionFactory.GetCurrentSession();

                IQuery query = s.GetNamedQuery("GetNbrPelerinsByEventIds");
                query = query.SetParameterList("eventIds", eventIds);
                result = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Pelerin))).List<Pelerin>();

                if (result != null)
                    return result.Count;

                return 0;
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return 0;
            }
        }

        public IList<Pelerin> GetListPelerinsByEventIds(List<int> eventIds)
        {
            try
            {
                IList<Pelerin> result = null;

                ISession s = SessionFactory.GetCurrentSession();

                IQuery query = s.GetNamedQuery("GetListPelerinsByEventIds");
                query = query.SetParameterList("eventIds", eventIds);
                result = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Pelerin))).List<Pelerin>();

                if (result != null)
                    return result;

                return null;
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }
        }


    }
}
