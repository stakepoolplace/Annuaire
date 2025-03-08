using System.Collections.Generic;

namespace Annuaire.Models
{
    public class Societe
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string Adresse2 { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string TelStandard { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
