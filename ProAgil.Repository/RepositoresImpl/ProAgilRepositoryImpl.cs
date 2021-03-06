using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain.Model;
using ProAgil.Repository.Data;
using ProAgil.Repository.Repositories;

namespace ProAgil.Repository.RepositoresImpl
{
    public class ProAgilRepositoryImpl : IProAgilRepository
    {

        private readonly DataContext _context;

        public ProAgilRepositoryImpl(DataContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedeSociais);

            if (includePalestrantes)
            {
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking().OrderByDescending(c => c.DataEvento);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedeSociais);

            if (includePalestrantes)
            {
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking().OrderByDescending(c => c.DataEvento).Where(c => c.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedeSociais);

            if (includePalestrantes)
            {
                query = query.Include(pe => pe.PalestrantesEventos).ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking().OrderByDescending(c => c.DataEvento).Where(c => c.EventoId == EventoId);

            return await query.FirstOrDefaultAsync();
        }

        //PALESTRANTES


        public async Task<Palestrante> GetPalestrantesAsync(int PalestranteId, bool includeEventos)
        {
          
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(c => c.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(pe => pe.PalestrantesEventos).ThenInclude(e => e.Evento);
            }

            query = query.OrderBy(p => p.Nome).Where(p => p.Id == PalestranteId);

            return await query.FirstOrDefaultAsync();
        }      
        public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(c => c.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(pe => pe.PalestrantesEventos).ThenInclude(e => e.Evento);
            }

            query = query.AsNoTracking().Where(p => p.Nome.ToLower().Contains(name.ToLower()));

            return await query.ToArrayAsync();
        }
    }
}