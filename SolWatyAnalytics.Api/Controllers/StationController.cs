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
    public class StationController : ControllerBase
    {
        private readonly StationRepo _stationRepo;

        public StationController(StationRepo stationRepo)
        {
            _stationRepo = stationRepo;
        }

        // GET: api/Station
        [HttpGet]
        public async Task<ActionResult<Station>> GetStations()
        {
            try
            {
                return Ok(await _stationRepo.FindAllDapper());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the DB !");
                throw;
            }
        }

        // GET: api/Station/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Station>> GetStation(int id)
        {
            try
            {
                var result = await _stationRepo.FindByIDDapper(id);

                if (result == null)
                {
                    return NotFound($"Station with ID = {id} not found !");
                }

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
                throw;
            }
        }

        // PUT: api/Station/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut()]
        public async Task<ActionResult<Station>> PutStation(Station station)
        {
            try
            {
                var targetObj = await _stationRepo.FindByIDDapper(station.Id);

                if (targetObj == null)
                {
                    return NotFound($"Station with ID = {station.Id} not found !");
                }

                return await _stationRepo.Update(station);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating Data");
                throw;
            }
        }

        // POST: api/Station
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Station>> PostStation(Station station)
        {
            try
            {
                if (station == null)
                {
                    return BadRequest();
                }

                var createdObj = await _stationRepo.Add(station);

                return CreatedAtAction(nameof(GetStation), new { id = createdObj.Id }, createdObj);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving data from DB");
                throw;
            }
        }

        // DELETE: api/Station/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Station>> DeleteStation(int id)
        {
            try
            {
                var targetObj = await _stationRepo.FindByIDDapper(id);

                if (targetObj == null)
                {
                    return NotFound($"Station with ID = {id} not found !");
                }

                return await _stationRepo.Delete(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting Data");
                throw;
            }
        }

        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Customer>>> Search(string name)
        {
            try
            {
                var result = await _stationRepo.Search(name);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound($"Station with this criteria = {name} not found !!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving data from DB");
                throw;
            }
        }

        private bool AreaExists(int id)
        {
            return _stationRepo.ObjExists(id);
        }
    }
}