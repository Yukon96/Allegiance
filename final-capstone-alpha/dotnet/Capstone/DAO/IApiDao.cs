using Capstone.Models;
using System.Threading.Tasks;

namespace Capstone.DAO
{
  public interface IApiDao
  {
    public Task<Protein> NCBIApiGetProteinSequence(string id);

    public Task<string> NCBIApiGetProteinID(string name);

    public Task<Protein> RCSBApiGetProteinSequence(string id);

    public Task<string> RCSBApiGetProteinID(string name);
  }
}
