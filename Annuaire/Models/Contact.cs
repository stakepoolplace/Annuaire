using DevExpress.Mvvm.CodeGenerators;
using System.Collections.Generic;
using System.ComponentModel;

namespace Annuaire.Models
{
    [GenerateViewModel]
    public partial class Contact
    {
        [GenerateProperty]
        private int _id;

        [GenerateProperty]
        private int _societeId;

        [GenerateProperty]
        private string _civilite;

        [GenerateProperty]
        private string _nom;

        [GenerateProperty]
        private string _prenom;

        [GenerateProperty]
        private string _fonction;

        [GenerateProperty]
        private Societe _societe;

        [GenerateProperty]
        private ICollection<InfoContact> _infos;

    }
}
