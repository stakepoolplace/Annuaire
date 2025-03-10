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
using Annuaire.Services;

namespace Annuaire.Views
{


    public partial class AddContact : Window
    {
        private AddContactViewModel _viewModel;
        private IAnnuaireService _service;


        private void InitializeViewModel(IAnnuaireService service, MainViewModel mainViewModel)
        {
            _service = service;
            _viewModel = new AddContactViewModel(_service, mainViewModel);
            _viewModel.RequestClose += ViewModel_RequestClose;
            DataContext = _viewModel;

        }


        public AddContact(IAnnuaireService service, MainViewModel mainViewModel)
        {
            InitializeComponent();
            InitializeViewModel(service, mainViewModel);
        }

        public AddContact(IAnnuaireService service, MainViewModel mainViewModel, Societe selectedSociete = null)
        {
            InitializeComponent();
            InitializeViewModel(service, mainViewModel);


            if (selectedSociete != null)
            {
                _viewModel.SelectedSociete = selectedSociete;
                //LoadContactsAsync(selectedSociete.Id);

            }
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
