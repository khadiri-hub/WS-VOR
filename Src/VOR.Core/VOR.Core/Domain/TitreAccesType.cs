namespace VOR.Core.Domain
{
    public class TitreAccesType : BaseObject<int>
    {
        public virtual string Nom { get; set; }
        public virtual int? Prefixe { get; set; }
    }
}