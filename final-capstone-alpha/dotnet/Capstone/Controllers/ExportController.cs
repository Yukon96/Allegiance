using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
  [ApiController]
  [Authorize]
  [Route("controller")]
  public class ExportController : Controller
  {
    [HttpPost]
    public ActionResult ExportToFile()
    {
      return Ok();
    }
  }
}
