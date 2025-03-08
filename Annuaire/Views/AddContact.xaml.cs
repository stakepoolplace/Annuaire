using DevExpress.Mvvm.CodeGenerators;
using Annuaire.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Annuaire.ViewModels;

namespace Annuaire.Views
{
    public partial class AddContact : Window
    {

        private AddContactViewModel _viewModel;

        public AddContact()
        {
            InitializeComponent();

            // Create the ViewModel
            _viewModel = new AddContactViewModel();

            // Subscribe to the close request event
            _viewModel.RequestClose += ViewModel_RequestClose;

            // Set the DataContext
            DataContext = _viewModel;
        }

        public AddContact(int societeId, string nomSociete) : this()
        {
            // Initialize with existing société data if provided
            _viewModel.SocieteId = societeId;
            _viewModel.Nom = nomSociete;

            // Additional initialization can be added here if needed
            this.Title = $"Contacts pour {nomSociete}";
        }

        public AddContact(int societeId, string nomSociete, string adresse, string adresse2, string codePostal, string ville, string standard) : this()
        {
            // Initialize with existing société data if provided
            _viewModel.SocieteId = societeId;
            _viewModel.Nom = nomSociete;
            _viewModel.Adresse = adresse;
            _viewModel.Adresse2 = adresse2;
            _viewModel.CodePostal = codePostal;
            _viewModel.Ville = ville;
            _viewModel.Standard = standard;

            // Additional initialization can be added here if needed
            this.Title = $"Contacts pour {nomSociete}";
        }


        private void ViewModel_RequestClose()
        {
            // Close the window when requested by the ViewModel
            this.Close();
        }

        protected override void OnClosed(System.EventArgs e)
        {
            // Unsubscribe from events to prevent memory leaks
            if (_viewModel != null)
            {
                _viewModel.RequestClose -= ViewModel_RequestClose;
            }

            base.OnClosed(e);
        }
    }
}
