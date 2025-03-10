using Annuaire.Data;
using Annuaire.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annuaire.Services
{
    public class AnnuaireService : IAnnuaireService
    {
        // Le DbContext n'est pas conservé comme champ privé :
        // on le crée et le détruit au sein de chaque méthode.
        public AnnuaireService()
        {
            try
            {
                using (var testContext = new AnnuaireDbContext())
                {
                    testContext.Database.CanConnect();
                    System.Diagnostics.Debug.WriteLine("Connexion à la base de données réussie");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur de connexion : {ex.Message}");
            }
        }

        public async Task<List<InfoContact>> GetInfoContactsAsync()
        {
            using (var context = new AnnuaireDbContext())
            {
                return await context.InfoContacts
                                    .Include(s => s.Contact)
                                    .ThenInclude(c => c.Societe)
                                    .ToListAsync();
            }
        }

        public async Task<List<InfoContact>> GetInfoContactsNoTrackingAsync()
        {
            using (var context = new AnnuaireDbContext())
            {
                return await context.InfoContacts
                                    .AsNoTracking()
                                    .Include(s => s.Contact)
                                    .ThenInclude(c => c.Societe)
                                    .ToListAsync();
            }
        }

        // Opérations Societe
        public async Task<List<Societe>> GetSocietesAsync()
        {
            using (var context = new AnnuaireDbContext())
            {
                return await context.Societes
                                    .AsNoTracking()
                                    .Include(s => s.Contacts)
                                    .ToListAsync();
            }
        }

        public async Task<Societe> GetSocieteByIdAsync(int id)
        {
            using (var context = new AnnuaireDbContext())
            {
                return await context.Societes
                                    .FirstOrDefaultAsync(s => s.Id == id);
            }
        }

        public async Task<Societe> AddSocieteAsync(Societe societe)
        {
            using (var context = new AnnuaireDbContext())
            {
                context.Societes.Add(societe);
                await context.SaveChangesAsync();
                return societe;
            }
        }

        public async Task UpdateSocieteAsync(Societe societe)
        {
            try
            {
                using (var context = new AnnuaireDbContext())
                {
                    var local = context.Set<Societe>()
                                       .Local
                                       .FirstOrDefault(entry => entry.Id.Equals(societe.Id));

                    if (local != null)
                    {
                        context.Entry(local).State = EntityState.Detached;
                    }

                    context.Entry(societe).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur dans UpdateSocieteAsync : {ex.Message}");
                throw;
            }
        }

        public async Task DeleteSocieteAsync(int id)
        {
            using (var context = new AnnuaireDbContext())
            {
                var societe = await context.Societes.FindAsync(id);
                if (societe != null)
                {
                    context.Societes.Remove(societe);
                    await context.SaveChangesAsync();
                }
            }
        }

        // Opérations Contact
        public async Task<List<Contact>> GetContactsAsync()
        {
            using (var context = new AnnuaireDbContext())
            {
                return await context.Contacts
                                    .Include(c => c.Societe)
                                    .Include(c => c.Infos)
                                    .ToListAsync();
            }
        }

        public async Task<List<Contact>> GetContactsBySocieteId(int societeId)
        {
            using (var context = new AnnuaireDbContext())
            {
                return await context.Contacts
                                    .Where(c => c.SocieteId == societeId)
                                    .Include(c => c.Infos)
                                    .ToListAsync();
            }
        }

        public async Task<List<InfoContact>> GetInfoContactsByContactId(int contactId)
        {
            using (var context = new AnnuaireDbContext())
            {
                return await context.InfoContacts
                                    .Where(i => i.ContactId == contactId)
                                    .ToListAsync();
            }
        }

        public async Task<Contact> AddContactAsync(Contact contact)
        {
            using (var context = new AnnuaireDbContext())
            {
                context.Contacts.Add(contact);
                await context.SaveChangesAsync();
                return contact;
            }
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            try
            {
                using (var context = new AnnuaireDbContext())
                {
                    var local = context.Set<Contact>()
                                       .Local
                                       .FirstOrDefault(entry => entry.Id.Equals(contact.Id));

                    if (local != null)
                    {
                        context.Entry(local).State = EntityState.Detached;
                    }

                    context.Entry(contact).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur dans UpdateContactAsync : {ex.Message}");
                throw;
            }
        }

        // Opérations InfoContact
        public async Task<InfoContact> AddInfoContactAsync(InfoContact infoContact)
        {
            using (var context = new AnnuaireDbContext())
            {
                context.InfoContacts.Add(infoContact);
                await context.SaveChangesAsync();
                return infoContact;
            }
        }

        public async Task UpdateInfoContactAsync(InfoContact infoContact)
        {
            try
            {
                using (var context = new AnnuaireDbContext())
                {
                    var local = context.Set<InfoContact>()
                                       .Local
                                       .FirstOrDefault(entry => entry.Id.Equals(infoContact.Id));

                    if (local != null)
                    {
                        context.Entry(local).State = EntityState.Detached;
                    }

                    context.Entry(infoContact).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur dans UpdateInfoContactAsync : {ex.Message}");
                throw;
            }
        }

        public async Task DeleteInfoContactAsync(int infoId)
        {
            using (var context = new AnnuaireDbContext())
            {
                var info = await context.InfoContacts.FindAsync(infoId);
                if (info != null)
                {
                    context.InfoContacts.Remove(info);
                    await context.SaveChangesAsync();
                }
            }
        }

        // Méthode de détachement, si jamais vous en avez besoin.
        public void DetachEntity<T>(T entity) where T : class
        {
            using (var context = new AnnuaireDbContext())
            {
                var entry = context.Entry(entity);
                if (entry != null)
                {
                    entry.State = EntityState.Detached;
                }
            }
        }
    }
}
