using System.Linq;
using System.Threading.Tasks;
using Next_OWL.Models.Output;

namespace Next_OWL.Services
{
    public interface IOwlService
    {
        Task<IOrderedEnumerable<Game>> GetFutureGames();
        Task<Game> GetNextGame();
    }
}