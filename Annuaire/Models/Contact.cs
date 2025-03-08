using System.Collections.Generic;

namespace Annuaire.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public int SocieteId { get; set; }
        public string Civilite { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Fonction { get; set; }
        public virtual Societe Societe { get; set; }
        public virtual ICollection<InfoContact> Infos { get; set; }
    }
}
