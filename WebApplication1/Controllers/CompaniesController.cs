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

        [HttpGet("{id:int}/{param2=company}")] //De esta maneraé la variable "param2" tiene por defecto el valor de "company"
        public async Task<ActionResult<Company>> GetbyId(int id, string param2)
        {
            var companies= await config.Companies.Include(x => x.Games).FirstOrDefaultAsync(y => y.Id == id);

            if (companies==null)
            {
                return NotFound();
            }

            return companies;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Company>> GetbyName(string name)
        {
            var companies = await config.Companies.Include(x => x.Games).FirstOrDefaultAsync(y => y.Name.Contains(name));

            if (companies == null)
            {
                return NotFound();
            }

            return companies;
        }

        [HttpGet("first")]
        public async Task<ActionResult<Company>> FirstCompany()
        {
            return await config.Companies.Include(x => x.Games).FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Company company)
        {

            var sameNameCompany = await config.Companies.AnyAsync(x => x.Name == company.Name);

            if(sameNameCompany)
            {
                return BadRequest($"The company {company.Name} already exist.");
            }
                

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

