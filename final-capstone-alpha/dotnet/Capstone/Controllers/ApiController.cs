using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
  [Route("[controller]")]
  [ApiController]
  [Authorize]
  public class ApiController : Controller
  {
    private readonly IApiDao apiDao;
    public ApiController(IApiDao apiDao)
    {
      this.apiDao = apiDao;
    }
    [HttpGet("ncbi/{name}")]
     public ActionResult<Protein> GetNCBIApiProtein(string name)
    {
      Protein protein = new Protein();
      try
      {
        string id = apiDao.NCBIApiGetProteinID(name).Result;
        protein = apiDao.NCBIApiGetProteinSequence(id).Result;
        protein.SequenceName = name;
      }
      catch (DaoException)
      {
        return StatusCode(500, "An internal server error occurred.");
      }
      return Ok(protein);
    }
    [HttpGet("rcsb/{name}")]
    public ActionResult<Protein> GetRCSBApiProtein(string name)
    {
      string id = "";
      Protein protein = new Protein();
      try
      {
        id = apiDao.RCSBApiGetProteinID(name).Result;
        protein = apiDao.RCSBApiGetProteinSequence(id).Result;
        protein.SequenceName = name;
      }
      catch (DaoException)
      {
        return StatusCode(500, "An internal server error occurred.");
      }

      return Ok(protein);
    }
  }
}
