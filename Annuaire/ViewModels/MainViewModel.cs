using DevExpress.Mvvm.CodeGenerators;
using Annuaire.Services;
using Annuaire.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using DevExpress.Data.Browsing;
using System.Windows.Input;
using System.Windows;
using Annuaire.Views;
using DevExpress.Data.Svg;

namespace Annuaire.ViewModels
{
    [GenerateViewModel]
    public partial class MainViewModel
    {
        private const string windowTitle = "Annuaire 0.0.1";
        private readonly AnnuaireService _service;



        public MainViewModel(IAnnuaireService service)
        {
            _service = (AnnuaireService)service;
            _ = LoadData();

            // Initialiser l'historique de recherche
            SearchHistory = new ObservableCollection<string>();
        }

        [GenerateProperty]
        string _Status;

        [GenerateProperty]
        string _UserName;

        [GenerateProperty]
        string _WindowTitle = windowTitle;

        [GenerateProperty]
        ObservableCollection<Societe> _Societes = new();

        [GenerateProperty]
        ObservableCollection<Contact> _Contacts = new();

        [GenerateProperty]
        ObservableCollection<InfoContact> _InfoContacts = new();

        [GenerateProperty]
        ObservableCollection<InfoContact> _FilteredInfoContacts = new();

        [GenerateProperty]
        string _SearchText;

        [GenerateProperty]
        ObservableCollection<string> _SearchHistory = new();

        [GenerateProperty]
        bool _IsSelectSocieteVisible; // Pour contrôler l'affichage de la zone "Choisir une société"

        [GenerateProperty]
        Societe _SelectedSociete; // Société sélectionnée dans la ComboBox

        [GenerateCommand]
        void Login() => Status = "User: " + UserName;
        bool CanLogin() => !string.IsNullOrEmpty(UserName);

        [GenerateCommand]
        async Task LoadData()
        {
            var societes = await _service.GetSocietesAsync();
            Societes = new ObservableCollection<Societe>(societes);

            var contacts = await _service.GetContactsAsync();
            Contacts = new ObservableCollection<Contact>(contacts);

            var infoContacts = await _service.GetInfoContactsAsync();
            InfoContacts = new ObservableCollection<InfoContact>(infoContacts);
            FilteredInfoContacts = new ObservableCollection<InfoContact>(infoContacts);

            Status = "Données chargées";
        }

        [GenerateCommand]
        void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredInfoContacts = new ObservableCollection<InfoContact>(InfoContacts);
                return;
            }

            // Ajouter à l'historique s'il n'existe pas déjà
            if (!SearchHistory.Contains(SearchText))
            {
                SearchHistory.Add(SearchText);
            }

            // Filtrer les données
            var searchLower = SearchText.ToLower();
            var filtered = InfoContacts.Where(i =>
                (i.Info?.ToLower().Contains(searchLower) == true) ||
                (i.TypeInfo?.ToLower().Contains(searchLower) == true) ||
                (i.Contact?.Nom?.ToLower().Contains(searchLower) == true) ||
                (i.Contact?.Societe?.Nom?.ToLower().Contains(searchLower) == true)
            ).ToList();

            FilteredInfoContacts = new ObservableCollection<InfoContact>(filtered);
            Status = $"Recherche : {filtered.Count} résultat(s)";
        }

        [GenerateCommand]
        void ClearSearch()
        {
            SearchText = string.Empty;
            FilteredInfoContacts = new ObservableCollection<InfoContact>(InfoContacts);
            Status = "Recherche effacée";
        }




        [GenerateCommand]
        void ShowSelectSocietePopup()
        {
            IsSelectSocieteVisible = true;
        }


        // Dans la commande AddNewContact existante, remplacer ou compléter :
        [GenerateCommand]
        void AddNewContact()
        {
            Status = "Création d'un nouveau contact...";
            ShowSelectSocietePopup();
        }


        [GenerateCommand]
        void AddNewSociete()
        {
            // Récupérer le texte saisi dans le ComboBoxEdit
            string nomSociete = SearchText; // ou la propriété liée au ComboBoxEdit

            var addContactViewModel = new AddContactViewModel();
            // Initialiser avec le nom de la société
            addContactViewModel.Nom = nomSociete;

            Status = "Création de contact pour " + nomSociete;

            var addContactWindow = new AddContact
            {
                DataContext = addContactViewModel
            };

            IsSelectSocieteVisible = false; // Fermer l'overlay
            addContactWindow.ShowDialog();

        }

        [GenerateCommand]
        void ClearSociete()
        {
            SelectedSociete = null;
            SearchText = string.Empty;
            Status = "Champ initialisé.";

        }


        [GenerateCommand]
        async void ConfirmSocieteSelection()
        {
            if (SelectedSociete == null) return;

            // Charger les données complètes de la société
            var societe = await _service.GetSocieteByIdAsync(SelectedSociete.Id);

            if (societe != null) // Ajout d'une vérification
            {
                // Ajouter un log pour déboguer
                System.Diagnostics.Debug.WriteLine($"Société chargée : {societe.Nom}, {societe.Adresse}, {societe.Ville}");

                var addContactWindow = new AddContact(
                    societe.Id,
                    societe.Nom,
                    societe.Adresse,
                    societe.Adresse2,
                    societe.CodePostal,
                    societe.Ville,
                    societe.TelStandard
                );

                Status = "Création de contact pour societe id: " + societe.Id;
                IsSelectSocieteVisible = false;
                addContactWindow.ShowDialog();
            }
            else
            {
                Status = "Erreur : Impossible de charger les données de la société";
            }
        }


        [GenerateCommand]
        void CancelSocieteSelection()
        {
            IsSelectSocieteVisible = false;
            Status = "Création de contact annulée.";
        }

    }
}