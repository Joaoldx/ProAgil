using System.Threading.Tasks;
using ProAgil.Domain.Model;

namespace ProAgil.Repository.Repositories
{
    public interface IProAgilRepository
    {

        // GERAL
         void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T: class;

         void Delete<T>(T entity) where T: class;

         Task<bool> SaveChangesAsync();

         //EVENTOS
         Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool includePalestrantes);
         
         Task<Evento[]> GetAllEventosAsync(bool includePalestrantes);

         Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes);

         // PALESTRANTES
         Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool includeEventos);

         Task<Palestrante> GetPalestrantesAsync(int PalestranteId, bool includeEventos);
    }
}