using System;
using System.Collections.Generic;
using VOR.Core.Domain;
using VOR.Core.Domain.Vues;

namespace VOR.Core.Contract
{
    public interface ICalendrierRepository : IRepository<Calendrier, int>
    {
        IList<Calendrier> GetListJoursInvitation(int num_evenement);

        IList<Calendrier> GetListJoursTransport(int num_evenement);

        void UpdateJoursAccred(IList<DateTime> sem, int idAccred);

        IList<Calendrier> GetAllByDate(List<DateTime> dt, int eventID);

        IList<int> GetAllIdByDate(List<DateTime> dt, int eventID);

        IList<Calendrier> GetListJoursAcces(int num_evenement);

        IList<Calendrier> GetListJours(int num_evenement);

        IList<Calendrier> GetJoursTournoisByEvtAndAcces(int eventID, int dlpId);

        Calendrier GetByNumJournee(int numJournee, int eventId);

        IList<Calendrier> GetListByIds(List<int> ids);

        IList<Calendrier> GetListJoursBetweenNumJournee(int numJourneeDebut, int numJourneeFin, int eventId);
    }
}