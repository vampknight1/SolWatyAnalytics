using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolWatyAnalytics.Api.Repo;
using SolWatyAnalytics.BLL.Models;
using SQLitePCL;

namespace SolWatyAnalytics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerRepo _customerRepo;

        public CustomerController(CustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }

        [HttpGet]
        public async Task<ActionResult<Customer>> GetCustomers()
        {
            try
            {
                return Ok(await _customerRepo.FindAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the DB !");
                throw;
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            try
            {
                var result = await _customerRepo.FindByID(id);

                if (result == null)
                {
                    return NotFound($"Customer with ID = {id} not found !");
                }

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customers)
        {
            //return Ok(await _customerRepo.Add(customers));
            try
            {
                if(customers == null)
                {
                    return BadRequest();
                }

                var createdObj = await _customerRepo.Add(customers);

                return CreatedAtAction(nameof(GetCustomer), new { id = createdObj.Id }, createdObj);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving data from DB");
                throw;
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Customer>> PutCustomer(int id, Customer customers)
        {
            try
            {
                if(id != customers.Id)
                {
                    return BadRequest("Customer ID mismatch !");
                }

                var targetObj = await _customerRepo.FindByID(id);

                if(targetObj == null)
                {
                    return NotFound($"Customer with ID = {id} not found !");
                }

                return await _customerRepo.Update(customers);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating Data");
                throw;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            try
            {
                var targetObj = await _customerRepo.FindByID(id);

                if (targetObj == null)
                {
                    return NotFound($"Customer with ID = {id} not found !");
                }

                return await _customerRepo.Delete(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting Data");
                throw;
            }
        }

        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Customer>>> Search (string name)
        {
            try
            {
                var result = await _customerRepo.Search(name);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound($"Customer with this criteria = {name} not found !!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving data from DB");
                throw;
            }
        }
    }
}