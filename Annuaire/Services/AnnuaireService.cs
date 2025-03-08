using Annuaire.Data;
using Annuaire.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            => await _context.InfoContacts.Include(s => s.Contact).ToListAsync();

        // Opérations Societe
        public async Task<List<Societe>> GetSocietesAsync()
            => await _context.Societes.Include(s => s.Contacts).ToListAsync();

        public async Task<Societe> AddSocieteAsync(Societe societe)
        {
            _context.Societes.Add(societe);
            await _context.SaveChangesAsync();
            return societe;
        }

        public async Task UpdateSocieteAsync(Societe societe)
        {
            _context.Societes.Update(societe);
            await _context.SaveChangesAsync();
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
    }
}
