using Annuaire.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Annuaire.Services
{
    public interface IAnnuaireService
    {
        Task<List<InfoContact>> GetInfoContactsAsync();
        Task<List<Societe>> GetSocietesAsync();
        Task<Societe> AddSocieteAsync(Societe societe);
        Task UpdateSocieteAsync(Societe societe);
        Task DeleteSocieteAsync(int id);
        Task<List<Contact>> GetContactsAsync();
    }
}
