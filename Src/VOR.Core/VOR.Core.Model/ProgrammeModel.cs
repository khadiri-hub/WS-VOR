
using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class ProgrammeModel : BaseModel<Programme, IProgrammeRepository>
    {
        public ProgrammeModel(IProgrammeRepository personRefRepository, IUnitOfWork unitOfWork)
            : base(personRefRepository, unitOfWork)
        {
        }

        public IList<Programme> GetProgrammeByEventID(int eventID)
        {
            return _repository.GetProgrammeByEventID(eventID);
        }

        public bool IsProgrammeSupprimable(decimal id)
        {
            return _repository.IsProgrammeSupprimable(id);
        }
    }
}