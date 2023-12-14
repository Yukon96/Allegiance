using System;
using System.Collections.Generic;
using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class GeneratorController
    {
        private readonly ICellDao cellDao;
        private readonly IProteinDao proteinDao;

        public GeneratorController(ICellDao cellDao, IProteinDao proteinDao)
        {
            this.cellDao = cellDao;
            this.proteinDao = proteinDao;
        }

        [HttpGet]
        public ActionResult<List<Cell>> getCells()
        {
            List<Cell> cells = new List<Cell>();
            try
            {
                foreach (Cell cell in cellDao.getCells())
                {
                    cells.Add(cell);
                }
            }
            catch (DaoException)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return cells;
        }

        [HttpGet("{id}")]
        public ActionResult<List<Cell>> getFastetCells(int id)
        {
           List<Cell> cells = new List<Cell>();
           try
           {
               Protein protein = proteinDao.GetProteinById(id);
               string letters = protein.ProteinSequence;
               List<Cell> cellList = cellDao.getFastestCells(letters);

           } catch (DaoException ex)
           {
                Console.WriteLine(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
           }

           return cells;
        }

        [HttpPatch("{id}")]
        public ActionResult<ProteinResponse> updateProteinSequence(int id)
        {
            ProteinResponse protein = null;
            try
            {
                Protein protein_base = proteinDao.GetProteinById(id);

                string letters = protein_base.ProteinSequence;
                List<Cell> cellList = cellDao.getFastestCells(letters);
                

                //Blue
                Cell blue_cell_1 = cellList[0];
                Cell blue_cell_2 = cellList[1];
                Cell blue_cell_3 = cellList[2];

                //Green
                Cell green_cell_1 = cellList[3];
                Cell green_cell_2 = cellList[4];
                Cell green_cell_3 = cellList[5];

                //Yellow
                Cell yellow_cell_1 = cellList[6];
                Cell yellow_cell_2 = cellList[7];
                Cell yellow_cell_3 = cellList[8];

                protein_base.Sequence1 = cell_name(protein_base.ProteinSequence, blue_cell_1, blue_cell_2, blue_cell_3);
                protein_base.Sequence2 = cell_name(protein_base.ProteinSequence, green_cell_1, green_cell_2, green_cell_3);
                  protein_base.Sequence3 = cell_name(protein_base.ProteinSequence, yellow_cell_1, yellow_cell_2, yellow_cell_3);
                
                
                

               protein = proteinDao.OptimizeProtein(protein_base);
                
                
            }
            catch (DaoException ex)
            {
                Console.WriteLine(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return protein;
        }

        private string cell_name(String sequince, Cell cell_1, Cell cell_2, Cell cell_3)
        {
          string cell_1_prefix = cell_1.LetterY + cell_1.LetterX;
          string cell_2_prefix = cell_2.LetterY + cell_2.LetterX;
          string cell_3_prefix = cell_3.LetterY + cell_3.LetterX;

          string sub_str = sequince.Substring(2);
          return $"{cell_1_prefix}{sub_str},{cell_2_prefix}{sub_str},{cell_3_prefix}{sub_str}";
        }
    }
}
