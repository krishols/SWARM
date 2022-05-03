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
    public class SchoolController : BaseController, iBaseController<School>
    {
        public SchoolController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)

        {

        }
        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                School itm = await _context.Schools.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
                _context.Schools.Remove(itm);
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
            List<School> lst = await _context.Schools.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            School itm = await _context.Schools.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
            return Ok(itm);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] School _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist = await _context.Schools.Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();
                if (exist != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "School already exists.");

                }
                exist = new School();
                exist.SchoolId = _Item.SchoolId;
                _context.Schools.Add(exist);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.SchoolId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }



        [HttpPut]
        public async Task<IActionResult> Put([FromBody] School _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist = await _context.Schools.Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();
                if (exist != null)
                {

                    await this.Post(_Item);
                    return Ok();
                }
                exist = new School();
                exist.SchoolId = _Item.SchoolId;
                _context.Schools.Update(exist);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.SchoolId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
