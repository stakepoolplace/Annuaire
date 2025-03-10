using DevExpress.Mvvm.CodeGenerators;
using System.Collections.Generic;

namespace Annuaire.Models
{
    [GenerateViewModel]
    public partial class Societe
    {
        [GenerateProperty]
        private int _id;

        [GenerateProperty]
        private string _nom;

        [GenerateProperty]
        private string _adresse;

        [GenerateProperty]
        private string _adresse2;

        [GenerateProperty]
        private string _codePostal;

        [GenerateProperty]
        private string _ville;

        [GenerateProperty]
        private string _telStandard;

        [GenerateProperty]
        private ICollection<Contact> _contacts;
    }
}
