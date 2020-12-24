using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolWatyAnalytics.Api.Models;
using SolWatyAnalytics.Api.Repo;
using SolWatyAnalytics.BLL.Models;

namespace SolWatyAnalytics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly AreaRepo _areaRepo;

        public AreaController(AreaRepo areaRepo)
        {
            _areaRepo = areaRepo;
        }

        // GET: api/Area
        [HttpGet]
        public async Task<ActionResult<Area>> GetAreas()
        {
            try
            {
                return Ok(await _areaRepo.FindAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the DB !");
                throw;
            }
        }

        // GET: api/Area/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Area>> GetArea(int id)
        {
            try
            {
                var result = await _areaRepo.FindByID(id);

                if (result == null)
                {
                    return NotFound($"Area with ID = {id} not found !");
                }

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
                throw;
            }
        }

        // PUT: api/Areas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut()]
        public async Task<ActionResult<Area>> PutArea(Area area)
        {
            try
            {
                var targetObj = await _areaRepo.FindByID(area.Id);

                if (targetObj == null)
                {
                    return NotFound($"Area with ID = {area.Id} not found !");
                }

                return await _areaRepo.Update(area);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating Data");
                throw;
            }
        }

        // POST: api/Areas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult> PostArea(Area area)
        {
            try
            {
                if (area == null)
                {
                    return BadRequest();
                }

                var createdObj = await _areaRepo.Add(area);

                return CreatedAtAction(nameof(GetArea), new { id = createdObj.Id }, createdObj);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving data from DB");
                throw;
            }
        }

        // DELETE: api/Areas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Area>> DeleteArea(int id)
        {
            try
            {
                var targetObj = await _areaRepo.FindByID(id);

                if (targetObj == null)
                {
                    return NotFound($"Area with ID = {id} not found !");
                }

                return await _areaRepo.Delete(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting Data");
                throw;
            }
        }

        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Area>>> Search(string name)
        {
            try
            {
                var result = await _areaRepo.Search(name);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound($"Area with this criteria = {name} not found !!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving data from DB");
                throw;
            }
        }

        private bool AreaExists(int id)
        {
            return _areaRepo.ObjExists(id);
        }
    }
}