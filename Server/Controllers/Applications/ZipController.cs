using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWARM.Server.Controllers.Applications
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZipController : BaseController, iBaseController<Zipcode>
    {
        public ZipController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)

        {

        }
        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Zipcode itm = await _context.Zipcodes.Where(x => int.Parse(x.Zip) == KeyValue).FirstOrDefaultAsync();
                _context.Zipcodes.Remove(itm);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return Ok();
            }
            catch (Exception e)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Zipcode> lst = await _context.Zipcodes.OrderBy(x => x.Zip).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Zipcode itm = await _context.Zipcodes.Where(x => int.Parse(x.Zip) == KeyValue).FirstOrDefaultAsync();
            return Ok(itm);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Zipcode _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist = await _context.Zipcodes.Where(x => x.Zip == _Item.Zip).FirstOrDefaultAsync();
                if (exist != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Zip already exists.");

                }
                exist = new Zipcode();
                exist.Zip = _Item.Zip;
                _context.Zipcodes.Add(exist);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.Zip);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }



        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Zipcode _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist = await _context.Zipcodes.Where(x => x.Zip == _Item.Zip).FirstOrDefaultAsync();
                if (exist != null)
                {

                    await this.Post(_Item);
                    return Ok();
                }

                exist = new Zipcode();
                exist.Zip = _Item.Zip;
                _context.Zipcodes.Update(exist);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.Zip);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
