using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Annuaire.Models;
using Annuaire.Services;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Mvvm.DataAnnotations;
using Annuaire.Views;

namespace Annuaire.ViewModels
{
    [GenerateViewModel]
    public partial class AddContactViewModel
    {
        private readonly IAnnuaireService _service;
        private readonly MainViewModel _mainViewModel;
        private List<int> _deletedContactInfoIds;
        public ObservableCollection<TypeInfoItem> TypeInfoItems { get; set; }
        public ObservableCollection<CiviliteItem> CiviliteItems { get; set; }

        public AddContactViewModel(IAnnuaireService service, MainViewModel mainViewModel)
        {
            _service = service;
            _mainViewModel = mainViewModel;
            // Initialize collections
            Contacts = new ObservableCollection<Contact>();
            ContactInfos = new ObservableCollection<InfoContact>();
            // Initialiser la liste des types d'info
            TypeInfoItems = new ObservableCollection<TypeInfoItem>
            {
                new TypeInfoItem { Text = "Email", Value = "Email" },
                new TypeInfoItem { Text = "Téléphone", Value = "Téléphone" }
            };
            CiviliteItems = new ObservableCollection<CiviliteItem>
            {
                new CiviliteItem { Text = "M.", Value = "M." },
                new CiviliteItem { Text = "Mme", Value = "Mme" }
            };
        }

        #region Properties

        // Société properties
        [GenerateProperty(OnChangedMethod = nameof(OnSelectedSocieteChanged))]
        Societe _selectedSociete;

        [GenerateProperty(OnChangedMethod = nameof(OnSelectedContactChanged))]
        Contact _selectedContact;

        [GenerateProperty]
        int _societeId;

        [GenerateProperty]
        string _nom;

        [GenerateProperty]
        string _adresse;

        [GenerateProperty]
        string _adresse2;

        [GenerateProperty]
        string _codePostal;

        [GenerateProperty]
        string _ville;

        [GenerateProperty]
        string _standard;

        [GenerateProperty]
        ObservableCollection<Contact> _contacts;

        [GenerateProperty]
        ObservableCollection<InfoContact> _contactInfos;

        [GenerateProperty]
        InfoContact _selectedContactInfo;

        #endregion

        #region Commands

        public virtual bool CanDeleteContactInfo() => SelectedContactInfo != null;

        [GenerateCommand(CanExecuteMethod = nameof(CanDeleteContactInfo))]
        public virtual void DeleteContactInfo()
        {
            if (SelectedContactInfo != null)
            {
                // Stocker l'info dans une variable locale avant de la retirer
                var infoASupprimer = SelectedContactInfo;

                // Supprimer l'info de la collection
                ContactInfos.Remove(infoASupprimer);

                // Si c'était une info existante (Id != 0), la marquer pour suppression
                if (infoASupprimer.Id != 0)
                {
                    _deletedContactInfoIds ??= new List<int>();
                    _deletedContactInfoIds.Add(infoASupprimer.Id);
                }

                SelectedContactInfo = ContactInfos.FirstOrDefault();
            }
        }


        [GenerateCommand]
        public virtual async void Save()
        {
            try
            {
                // 1. Sauvegarder ou mettre à jour la société
                var societe = new Societe
                {
                    Id = SocieteId,
                    Nom = Nom,
                    Adresse = Adresse,
                    Adresse2 = Adresse2,
                    CodePostal = CodePostal,
                    Ville = Ville,
                    TelStandard = Standard
                };

                if (SocieteId == 0)
                {
                    // Nouvelle société
                    societe = await _service.AddSocieteAsync(societe);
                    SocieteId = societe.Id;
                }
                else
                {
                    // Mise à jour société existante
                    await _service.UpdateSocieteAsync(societe);
                }

                // 2. Sauvegarder les contacts
                foreach (var contact in Contacts)
                {
                    contact.SocieteId = SocieteId;
                    if (contact.Id == 0)
                    {
                        // Nouveau contact
                        var savedContact = await _service.AddContactAsync(contact);
                        // Mettre à jour l'ID du contact après la sauvegarde
                        contact.Id = savedContact.Id;
                    }
                    else
                    {
                        // Mise à jour contact existant
                        await _service.UpdateContactAsync(contact);
                    }
                }

                // 3. Sauvegarder les infos contacts

                foreach (var info in ContactInfos)
                {
                    if (info.Id == 0)
                    {
                        // Nouvelle info contact
                        info.ContactId = SelectedContact.Id;
                        info.Contact = SelectedContact;
                        await _service.AddInfoContactAsync(info);
                    }
                    else
                    {
                        // Mise à jour info contact existante
                        await _service.UpdateInfoContactAsync(info);
                    }
                }
                // Supprimer les infos contacts marquées pour suppression
                if (_deletedContactInfoIds?.Any() == true)
                {
                    foreach (var infoId in _deletedContactInfoIds)
                    {
                        // Ajouter cette méthode dans IAnnuaireService et l'implémenter
                        await _service.DeleteInfoContactAsync(infoId);
                    }
                }

                RequestClose?.Invoke();

                // 4. Rafraîchir les données du MainViewModel
                if (_mainViewModel != null)
                {

                    await _mainViewModel.RefreshGrid(); // Nouvelle méthode

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                // Gérer les erreurs (vous pouvez implémenter votre propre gestion d'erreurs)
                MessageBox.Show($"Erreur lors de la sauvegarde : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [GenerateCommand]
        public virtual void Cancel()
        {
            System.Diagnostics.Debug.WriteLine("Cancel");
            // Fermer la fenêtre sans sauvegarder
            RequestClose?.Invoke();
        }


        [GenerateCommand]
        public virtual void AddContact()
        {
            var newContact = new Contact
            {
                SocieteId = SocieteId
            };
            Contacts.Add(newContact);
            SelectedContact = newContact;
        }

        [GenerateCommand]
        public virtual void AddContactInfo()
        {
            if (SelectedContact == null) return;

            var newInfo = new InfoContact
            {
                ContactId = SelectedContact.Id
            };
            ContactInfos.Add(newInfo);
            SelectedContactInfo = newInfo;
        }

        public virtual bool CanDeleteContact() => SelectedContact != null;

        [GenerateCommand(CanExecuteMethod = nameof(CanDeleteContact))]
        public virtual void DeleteContact()
        {
            if (SelectedContact != null)
            {
                Contacts.Remove(SelectedContact);
                RemoveContactInfos(SelectedContact.Id);
                SelectedContact = Contacts.Count > 0 ? Contacts[0] : null;
            }
        }



        #endregion

        #region Methods

        protected virtual async void OnSelectedSocieteChanged()
        {
            if (SelectedSociete == null)
            {
                ClearFields();
                return;
            }

            SocieteId = SelectedSociete.Id;
            Nom = SelectedSociete.Nom;
            Adresse = SelectedSociete.Adresse;
            Adresse2 = SelectedSociete.Adresse2;
            CodePostal = SelectedSociete.CodePostal;
            Ville = SelectedSociete.Ville;
            Standard = SelectedSociete.TelStandard;

            var contacts = await _service.GetContactsBySocieteId(SelectedSociete.Id);
            Contacts = new ObservableCollection<Contact>(contacts);
        }

        protected virtual async void OnSelectedContactChanged()
        {
            if (SelectedContact == null)
            {
                ContactInfos.Clear();
                SelectedContactInfo = null;
                return;
            }

            // Ne charger les infos que si la collection est vide ou si on n'a pas encore chargé pour ce contact
            if (!ContactInfos.Any() || !ContactInfos.Any(i => i.ContactId == SelectedContact.Id))
            {
                var infos = await _service.GetInfoContactsByContactId(SelectedContact.Id);
                ContactInfos = new ObservableCollection<InfoContact>(infos);
            }
            else
            {
                // Filtrer les infos existantes pour ne montrer que celles du contact sélectionné
                var contactInfos = ContactInfos.Where(i => i.ContactId == SelectedContact.Id).ToList();
                ContactInfos = new ObservableCollection<InfoContact>(contactInfos);
            }

            SelectedContactInfo = ContactInfos.FirstOrDefault();
        }

        private void ClearFields()
        {
            Nom = string.Empty;
            Adresse = string.Empty;
            Adresse2 = string.Empty;
            CodePostal = string.Empty;
            Ville = string.Empty;
            Standard = string.Empty;
            Contacts.Clear();
            ContactInfos.Clear();
        }

        private void RemoveContactInfos(int contactId)
        {
            var infosToRemove = ContactInfos.Where(i => i.ContactId == contactId).ToList();
            foreach (var info in infosToRemove)
            {
                ContactInfos.Remove(info);
            }
        }

        #endregion

        #region Events

        public event Action RequestClose;

        #endregion
    }



}

