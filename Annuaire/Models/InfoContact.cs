namespace Annuaire.Models
{
    public class InfoContact
    {
        public int Id { get; set; }
        public string TypeInfo { get; set; }
        public string Info { get; set; }
        public int ContactId { get; set; }
        public virtual Contact Contact { get; set; }

        public string SocieteGroupLabel
        {
            get
            {
                if (Contact?.Societe != null)
                    return $"{Contact.Societe.Nom}: {Contact.Societe.TelStandard}";
                return string.Empty;
            }
        }

        public string ContactGroupLabel
        {
            get
            {
                if (Contact != null)
                    return $"{Contact.Nom} {Contact.Prenom}: {Contact.Fonction}";
                return string.Empty;
            }
        }
    }
}
