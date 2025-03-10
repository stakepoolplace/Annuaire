using System.ComponentModel;
using System.Collections.Generic;

namespace Annuaire.Models
{
    public class InfoContact : INotifyPropertyChanged
    {
        private int _id;
        private string _typeInfo;
        private string _info;
        private int _contactId;
        private Contact _contact;

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string TypeInfo
        {
            get => _typeInfo;
            set
            {
                if (_typeInfo != value)
                {
                    _typeInfo = value;
                    OnPropertyChanged(nameof(TypeInfo));
                }
            }
        }

        public string Info
        {
            get => _info;
            set
            {
                if (_info != value)
                {
                    _info = value;
                    OnPropertyChanged(nameof(Info));
                }
            }
        }

        public int ContactId
        {
            get => _contactId;
            set
            {
                if (_contactId != value)
                {
                    _contactId = value;
                    OnPropertyChanged(nameof(ContactId));
                }
            }
        }

        public virtual Contact Contact
        {
            get => _contact;
            set
            {
                if (_contact != value)
                {
                    _contact = value;
                    OnPropertyChanged(nameof(Contact));
                    // Notifier les propriétés dépendantes
                    OnPropertyChanged(nameof(SocieteGroupLabel));
                    OnPropertyChanged(nameof(ContactGroupLabel));
                }
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
