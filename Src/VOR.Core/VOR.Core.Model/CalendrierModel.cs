using System;
using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Domain.Vues;
using VOR.Core.UnitOfWork;
using System.Web;
using System.Web.Caching;
using VOR.Core.Model;

namespace VOR.Core.Model
{
    public class CalendrierModel : CacheableModel<Calendrier, ICalendrierRepository, int>
    {
        public CalendrierModel(ICalendrierRepository calRepository, IUnitOfWork unitOfWork)
            : base(calRepository, unitOfWork, new TimeSpan(1, 0, 0), "Calendrier")
        {
        }

        public IList<Calendrier> GetListJoursAcces()
        {
            // ex: key = ListOfCalendrier::EstAcces:1:EventID:25;
            var key = "ListOf" + _cacheKey + "::EstAcces:1:EventID:" + Bootstrapper.EvenementNumCourant;
            var cache = HttpRuntime.Cache;
            IList<Calendrier> results = (IList<Calendrier>)cache[key];
            if (results != null)
                return results;
            else
            {
                results = _repository.GetListJoursAcces(Bootstrapper.EvenementNumCourant);
                cache.Insert(key, results, null, DateTime.Now.Add(_cacheDuration), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
            return results;
        }

        public IList<Calendrier> GetListJours()
        {
            var key = "ListOf" + _cacheKey + "::EventID=" + Bootstrapper.EvenementNumCourant;
            var cache = HttpRuntime.Cache;
            IList<Calendrier> results = (IList<Calendrier>)cache[key];
            if (results != null)
                return results;
            else
            {
                results = _repository.GetListJours(Bootstrapper.EvenementNumCourant);
                cache.Insert(key, results, null, DateTime.Now.Add(_cacheDuration), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
            return results;
        }

        public IList<Calendrier> GetListJoursAcces(int num_evenement)
        {
            return this._repository.GetListJoursAcces(num_evenement);
        }

        public IList<Calendrier> GetListJours(int num_evenement)
        {
            return this._repository.GetListJours(num_evenement);
        }

        public IList<Calendrier> GetJoursTournoisByEvtAndAcces(int dlpId)
        {
            return _repository.GetJoursTournoisByEvtAndAcces(Bootstrapper.EvenementNumCourant, dlpId);
        }

        public Calendrier GetByNumJournee(int numJournee)
        {
            return _repository.GetByNumJournee(numJournee, Bootstrapper.EvenementNumCourant);
        }

        public IList<Calendrier> GetListByIds(List<int> ids)
        {
            return _repository.GetListByIds(ids);
        }

        public IList<Calendrier> GetListJoursBetweenNumJournee(int numJourneeDebut, int numJourneeFin, int eventId)
        {
            return _repository.GetListJoursBetweenNumJournee(numJourneeDebut, numJourneeFin, eventId);
        }
    }
}