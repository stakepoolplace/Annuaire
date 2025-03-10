using Annuaire.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Annuaire.Services
{
    public interface IAnnuaireService
    {
        Task<List<InfoContact>> GetInfoContactsAsync();
        Task<List<InfoContact>> GetInfoContactsNoTrackingAsync();
        Task<List<Societe>> GetSocietesAsync();
        Task<Societe> AddSocieteAsync(Societe societe);
        Task UpdateSocieteAsync(Societe societe);
        Task DeleteSocieteAsync(int id);
        Task<List<Contact>> GetContactsAsync();
        Task<List<Contact>> GetContactsBySocieteId(int societeId);
        Task<List<InfoContact>> GetInfoContactsByContactId(int contactId);
        Task<Contact> AddContactAsync(Contact contact);
        Task UpdateContactAsync(Contact contact);
        Task<InfoContact> AddInfoContactAsync(InfoContact infoContact);
        Task UpdateInfoContactAsync(InfoContact infoContact);
        Task DeleteInfoContactAsync(int infoId);
        void DetachEntity<T>(T entity) where T : class;
        Task<Societe> GetSocieteByIdAsync(int societeId);
    }
}
