namespace Annuaire.Models
{
    public class InfoContact
    {
        public int Id { get; set; }
        public string TypeInfo { get; set; }
        public string Info { get; set; }
        public int ContactId { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
