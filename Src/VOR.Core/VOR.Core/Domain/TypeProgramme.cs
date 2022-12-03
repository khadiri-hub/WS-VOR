namespace VOR.Core.Domain
{
    public class TypeProgramme : BaseObject<int>
    {
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
    }
}
