using System;
using System.Collections.ObjectModel;
using Annuaire.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Mvvm.DataAnnotations;

namespace Annuaire.ViewModels
{
    [GenerateViewModel]
    public partial class AddContactViewModel
    {
        public AddContactViewModel()
        {
            // Initialize collections
            Contacts = new ObservableCollection<Contact>();
            ContactInfos = new ObservableCollection<InfoContact>();
        }

        #region Properties

        // Société properties
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

        // Contact collections and selected items
        [GenerateProperty]
        ObservableCollection<Contact> _contacts;

        [GenerateProperty(OnChangedMethod = nameof(OnSelectedContactChanged))]
        Contact _selectedContact;

        // InfoContact collections and selected items
        [GenerateProperty]
        ObservableCollection<InfoContact> _contactInfos;

        [GenerateProperty]
        InfoContact _selectedContactInfo;

        #endregion

        #region Commands

        [GenerateCommand]
        public virtual void Save()
        {
            // To be implemented
            // Code to save the société and associated contacts
        }

        [GenerateCommand]
        public virtual void Cancel()
        {
            // Notify view to close
            RequestClose?.Invoke();
        }

        [GenerateCommand]
        public virtual void AddContact()
        {
            // Create and add a new contact
            var newContact = new Contact();
            newContact.SocieteId = (int) SocieteId;
            Contacts.Add(newContact);
            SelectedContact = newContact;
        }

        [GenerateCommand]
        public virtual void AddContactInfo()
        {
            if (SelectedContact == null) return;

            // Create and add a new contact info
            var newInfo = new InfoContact();
            newInfo.ContactId = SelectedContact.Id;
            ContactInfos.Add(newInfo);
            SelectedContactInfo = newInfo;
        }

        public virtual bool CanDeleteContact() { return SelectedContact != null; }

        [GenerateCommand(CanExecuteMethod = nameof(CanDeleteContact))]
        public virtual void DeleteContact()
        {
            if (SelectedContact != null)
            {
                // Remove the contact
                Contacts.Remove(SelectedContact);

                // Clear associated contact infos
                var infosToRemove = new ObservableCollection<InfoContact>();
                foreach (var info in ContactInfos)
                {
                    if (info.ContactId == SelectedContact.Id)
                    {
                        infosToRemove.Add(info);
                    }
                }

                foreach (var info in infosToRemove)
                {
                    ContactInfos.Remove(info);
                }

                SelectedContact = Contacts.Count > 0 ? Contacts[0] : null;
            }
        }

        public virtual bool CanDeleteContactInfo() => SelectedContactInfo != null;

        [GenerateCommand(CanExecuteMethod = nameof(CanDeleteContactInfo))]
        public virtual void DeleteContactInfo()
        {
            if (SelectedContactInfo != null)
            {
                ContactInfos.Remove(SelectedContactInfo);
                SelectedContactInfo = ContactInfos.Count > 0 ? ContactInfos[0] : null;
            }
        }

        #endregion

        #region Methods

        protected virtual void OnSelectedContactChanged()
        {
            if (SelectedContact != null)
            {
                LoadContactInfo(SelectedContact.Id);
            }
            else
            {
                ContactInfos.Clear();
            }
        }

        protected virtual void LoadContactInfo(int contactId)
        {
            // Clear existing items
            ContactInfos.Clear();

            // Load contact info for the selected contact
            // This would typically involve a service call or database query
            // For now, just a placeholder
        }

        #endregion

        #region Events

        // Event to request closing the window
        public event Action RequestClose;

        #endregion
    }


}