using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly DbConfig config;

        public CompaniesController(DbConfig config)
        {
            this.config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Company>>> Get()
        {
            return await config.Companies.Include(x=>x.Games).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Company company)
        {
            config.Add(company);
            await config.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Company company, int id) 
         { 
            if(company.Id != id)
            {
                return BadRequest("Los ID no coinciden");
            }

            var exist = await config.Companies.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            config.Update(company);
            await config.SaveChangesAsync();
            return Ok();



        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await config.Companies.AnyAsync(x => x.Id == id);

            if (!exist) 
            {
                return NotFound();            
            }

            config.Remove(new Company() { Id = id });
            await config.SaveChangesAsync();
            return Ok();
        }




    }
    }

