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

        public DevExpress.Xpf.Grid.GridControl MainGrid { get; set; }

        [GenerateProperty]
        string _Status;

        [GenerateProperty]
        string _UserName;

        [GenerateProperty]
        string _WindowTitle = windowTitle;

        [GenerateProperty]
        ObservableCollection<Societe> _Societes = new();

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

        // Pour la sélection de société
        [GenerateProperty]
        string _SocieteSearchText;

        public async Task RefreshGrid()
        {

            if (MainGrid != null)
            {
                var infoContacts = await _service.GetInfoContactsNoTrackingAsync();
                InfoContacts = new ObservableCollection<InfoContact>(infoContacts);
                Search();

            } 


        }



        [GenerateCommand]
        void Login() => Status = "User: " + UserName;
        bool CanLogin() => !string.IsNullOrEmpty(UserName);

        [GenerateCommand]
        public async Task LoadData()
        {
            var societes = await _service.GetSocietesAsync();
            Societes = new ObservableCollection<Societe>(societes);

            var infoContacts = await _service.GetInfoContactsAsync();
            InfoContacts = new ObservableCollection<InfoContact>(infoContacts);
            FilteredInfoContacts = new ObservableCollection<InfoContact>(infoContacts);

            // Forcer le rafrachissement des groupes (mis en cache)
            MainGrid?.RefreshData();

            Status = "Données chargées";
        }

        [GenerateCommand]
        public void Search()
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
            string nomSociete = SocieteSearchText;

            var addContactWindow = new AddContact(_service, this); // Passer le service au constructeur de AddContact
            var viewModel = (AddContactViewModel)addContactWindow.DataContext; // Récupérer le ViewModel créé
            viewModel.Nom = nomSociete;

            Status = "Création de contact pour " + nomSociete;
            IsSelectSocieteVisible = false;
            addContactWindow.ShowDialog();
        }

        [GenerateCommand]
        void ClearSociete()
        {
            SelectedSociete = null;
            SocieteSearchText = string.Empty;
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

                var addContactWindow = new AddContact(_service, this, societe);

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