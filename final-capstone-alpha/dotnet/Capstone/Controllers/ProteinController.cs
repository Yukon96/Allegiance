using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Microsoft.AspNetCore.Authorization;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ProteinController : ControllerBase
    {
        private readonly IUserDao userDao;
        private readonly IProteinDao proteinDao;
        private readonly ICellDao cellDao;

        public ProteinController(IProteinDao proteinDao, IUserDao userDao, ICellDao cellDao)
        {
            this.userDao = userDao;
            this.proteinDao = proteinDao;
            this.cellDao = cellDao;
        }

        [HttpGet]
        public ActionResult<IList<Protein>> GetProteins()
        {
            IList<Protein> proteins = new List<Protein>();
            try
            {
                foreach (Protein protein in proteinDao.GetProteins())
                {
                    proteins.Add(protein);
                }
            }
            catch (DaoException)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
            return Ok(proteins);
        }

        [HttpGet("{id}")]
        public ActionResult GetProteinById(int id)
        {
            ProteinResponse protein = new ProteinResponse();
            try
            {
                Protein protein_base = proteinDao.GetProteinById(id);
                protein.ProteinId = protein_base.ProteinId;
                protein.SequenceName = protein_base.SequenceName;
                protein.ProteinSequence = protein_base.ProteinSequence;
                protein.Description = protein_base.Description;
                protein.UserId = protein_base.UserId;
                protein.FormatType = protein_base.FormatType;
                
                string[] blues = protein_base.Sequence1.Split(',');
                string[] greens = protein_base.Sequence2.Split(',');
                string[] yellows = protein_base.Sequence3.Split(',');

                protein.BlueSequence = new List<string>(blues);
                protein.GreenSequence = new List<string>(greens);
                protein.YellowSequence = new List<string>(yellows);

            }
            catch (DaoException)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
            return Ok(protein);
        }

        [HttpGet("name/{name}")]
        public ActionResult<IList<Protein>> GetProteinsBySequenceName(string name)
        {
            IList<Protein> proteins = new List<Protein>();
            try
            {
                foreach (Protein protein in proteinDao.GetProteinsBySequenceName(name))
                {
                    proteins.Add(protein);
                }
            }
            catch (DaoException)
            {
                return StatusCode(500, "An internal server error occurred.");
            }

            return Ok(proteins);
        }

        [HttpGet("user/{username}")]
        public ActionResult<IList<Protein>> GetProteinsByUsername(string username)
        {
            IList<Protein> proteins = new List<Protein>();
            try
            {
                foreach (Protein protein in proteinDao.GetProteinsByUsername(username))
                {
                    proteins.Add(protein);
                }
            }
            catch (DaoException)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
            return Ok(proteins);
        }

        [HttpPost]
        public ActionResult CreateProtein(RegisterProtein proteinParam)
        {
            int test = 7;
            ReturnUser user = userDao.GetUserByUsername(User.Identity.Name);
            Protein protein = null;
            try
            {
                protein = proteinDao.CreateProtein(proteinParam.SequenceName, proteinParam.ProteinSequence, proteinParam.Description, user.Username, user.UserId);
            }
            catch (DaoException)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
            return Created($"/protein/{protein.ProteinId}", protein);
        }

        [HttpPut]
        public ActionResult UpdateProtein(int proteinId, Protein proteinParam)
        {
            ReturnUser user = userDao.GetUserByUsername(User.Identity.Name);
            Protein protein = null;
            try
            {
                protein = proteinDao.UpdateProtein(proteinParam.ProteinId, proteinParam.SequenceName,
                    proteinParam.ProteinSequence, proteinParam.Description, user.UserId);
            }
            catch (DaoException)
            {
                return StatusCode(500, "An internal server error occurred.");
            }

            return Ok(protein);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProtein(int id)
        {
            bool result = false;
            try
            {
                result = proteinDao.DeleteProteinById(id);
            }
            catch (DaoException)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
            if (result == true)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
