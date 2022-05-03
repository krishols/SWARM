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
    public class InstructorController : BaseController, iBaseController<Instructor>
    {
        public InstructorController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)

        {

        }
        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Instructor itm = await _context.Instructors.Where(x => x.InstructorId == KeyValue).FirstOrDefaultAsync();
                _context.Instructors.Remove(itm);
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
            List<Instructor> lst = await _context.Instructors.OrderBy(x => x.InstructorId).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Instructor itm = await _context.Instructors.Where(x => x.InstructorId == KeyValue).FirstOrDefaultAsync();
            return Ok(itm);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Instructor _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist = await _context.Instructors.Where(x => x.InstructorId == _Item.InstructorId).FirstOrDefaultAsync();
                if (exist != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Instructor already exists.");

                }
                exist = new Instructor();
                exist.InstructorId = _Item.InstructorId;
                exist.LastName= _Item.LastName;
                exist.SchoolId = _Item.SchoolId;
                _context.Instructors.Add(exist);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.InstructorId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }



        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Instructor _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist = await _context.Instructors.Where(x => x.InstructorId == _Item.InstructorId).FirstOrDefaultAsync();
                if (exist != null)
                {

                    await this.Post(_Item);
                    return Ok();
                }
                exist = new Instructor();
                exist.InstructorId = _Item.InstructorId;
                exist.LastName = _Item.LastName;
                exist.SchoolId = _Item.SchoolId;
                _context.Instructors.Update(exist);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.InstructorId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

     
    }
}
