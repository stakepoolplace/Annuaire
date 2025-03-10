using Annuaire.Data;
using Annuaire.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Annuaire.Services
{
    public class AnnuaireService : IAnnuaireService
    {
        private readonly AnnuaireDbContext _context;

        public AnnuaireService()
        {
            _context = new AnnuaireDbContext();
            try
            {
                // Test de connexion
                _context.Database.CanConnect();
                System.Diagnostics.Debug.WriteLine("Connexion à la base de données réussie");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur de connexion : {ex.Message}");
            }
        }

        public async Task<List<InfoContact>> GetInfoContactsAsync()
            => await _context.InfoContacts.Include(s => s.Contact).ThenInclude(c => c.Societe).ToListAsync();

        public async Task<List<InfoContact>> GetInfoContactsNoTrackingAsync()
            => await _context.InfoContacts.AsNoTracking().Include(s => s.Contact).ThenInclude(c => c.Societe).ToListAsync();


        // Opérations Societe
        public async Task<List<Societe>> GetSocietesAsync()
            => await _context.Societes.AsNoTracking().Include(s => s.Contacts).ToListAsync();

        public async Task<Societe> GetSocieteByIdAsync(int id)
        {
            return await _context.Societes
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Societe> AddSocieteAsync(Societe societe)
        {
            _context.Societes.Add(societe);
            await _context.SaveChangesAsync();
            return societe;
        }

        /*        public async Task UpdateSocieteAsync(Societe societe)
                {
                    _context.Societes.Update(societe);
                    await _context.SaveChangesAsync();
                }*/

        public async Task UpdateSocieteAsync(Societe societe)
        {
            try
            {
                var local = _context.Set<Societe>()
                    .Local
                    .FirstOrDefault(entry => entry.Id.Equals(societe.Id));

                // Si l'entité est déjà suivie localement
                if (local != null)
                {
                    // Détacher l'entité existante
                    _context.Entry(local).State = EntityState.Detached;
                }

                // Marquer l'entité comme modifiée sans l'attacher
                _context.Entry(societe).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur dans UpdateSocieteAsync : {ex.Message}");
                throw;
            }
        }


        public async Task DeleteSocieteAsync(int id)
        {
            var societe = await _context.Societes.FindAsync(id);
            if (societe != null)
            {
                _context.Societes.Remove(societe);
                await _context.SaveChangesAsync();
            }
        }

        // Opérations Contact avec inclusion des infos
        public async Task<List<Contact>> GetContactsAsync()
        {
            var contacts = await _context.Contacts
                .Include(c => c.Societe)
                .Include(c => c.Infos)
                .ToListAsync();

            // Placez un point de débogage ici pour vérifier le contenu de 'contacts'
            return contacts;
        }

        public async Task<List<Contact>> GetContactsBySocieteId(int societeId)
        {
            return await _context.Contacts
                .Where(c => c.SocieteId == societeId)
                .Include(c => c.Infos)
                .ToListAsync();
        }

        public async Task<List<InfoContact>> GetInfoContactsByContactId(int contactId)
        {
            return await _context.InfoContacts
                .Where(i => i.ContactId == contactId)
                .ToListAsync();
        }



        public async Task<Contact> AddContactAsync(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact;
        }


        public async Task UpdateContactAsync(Contact contact)
        {

            try
            {
                var local = _context.Set<Contact>()
                .Local
                    .FirstOrDefault(entry => entry.Id.Equals(contact.Id));

                // Si l'entité est déjà suivie localement
                if (local != null)
                {
                    // Détacher l'entité existante
                    _context.Entry(local).State = EntityState.Detached;
                }

                // Marquer l'entité comme modifiée sans l'attacher
                _context.Entry(contact).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur dans UpdateContactAsync : {ex.Message}");
                throw;
            }
        }


        public async Task<InfoContact> AddInfoContactAsync(InfoContact infoContact)
        {
            _context.InfoContacts.Add(infoContact);
            await _context.SaveChangesAsync();
            return infoContact;
        }


        public async Task UpdateInfoContactAsync(InfoContact infoContact)
        {
            try
            {
                var local = _context.Set<InfoContact>()
                .Local
                    .FirstOrDefault(entry => entry.Id.Equals(infoContact.Id));

                // Si l'entité est déjà suivie localement
                if (local != null)
                {
                    // Détacher l'entité existante
                    _context.Entry(local).State = EntityState.Detached;
                }

                // Marquer l'entité comme modifiée sans l'attacher
                _context.Entry(infoContact).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur dans UpdateInfoContactAsync : {ex.Message}");
                throw;

            }
        }


        public async Task DeleteInfoContactAsync(int infoId)
        {
            var info = await _context.InfoContacts.FindAsync(infoId);
            if (info != null)
            {
                _context.InfoContacts.Remove(info);
                await _context.SaveChangesAsync();
            }

        }

        public void DetachEntity<T>(T entity) where T : class
        {
            var entry = _context.Entry(entity);
            if (entry != null)
            {
                entry.State = EntityState.Detached;
            }
        }




    }
}
