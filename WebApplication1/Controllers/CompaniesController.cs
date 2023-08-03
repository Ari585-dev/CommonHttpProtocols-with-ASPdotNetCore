using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly DbConfig config;
        private readonly IService service;
        private readonly TransientService tservice;
        private readonly ScopedService sservice;
        private readonly SingletonService stservice;
        private readonly ILogger<CompaniesController> logger;

        public CompaniesController(DbConfig config, IService service, TransientService tservice, ScopedService sservice, SingletonService stservice,
            ILogger<CompaniesController>logger)
        {
            this.config = config;
            this.service = service;
            this.tservice = tservice;
            this.sservice = sservice;
            this.stservice = stservice;
            this.logger = logger;
        }

        [HttpGet("GUID")]
        public ActionResult getGuids()
        {
            return Ok(new
            {
                CompaniesControllerTransient = tservice.Guid,
                ServiceA_t=service.getTransient(),
                CompaniesControllerScoped = sservice.Guid,
                ServiceA_sc = service.getScoped(),
                CompaniesControllerSingletone = stservice.Guid,
                ServiceA_sn = service.getSingletone(),

            });
        }
       

        [HttpGet]
        public async Task<ActionResult<List<Company>>> Get()
        {
            logger.LogInformation("Estamos obteniendo los autores");
            service.MakeWork();
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

