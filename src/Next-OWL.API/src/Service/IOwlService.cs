using System.Linq;
using System.Threading.Tasks;
using Next_OWL.Models.Output;

namespace Next_OWL.Service
{
    public interface IOwlService
    {
        Task<IOrderedEnumerable<Game>> GetFuture(int count = 10);
        Task<Game> GetNext();
    }
}