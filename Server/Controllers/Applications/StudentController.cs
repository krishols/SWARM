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
    public class StudentController : BaseController, iBaseController<Student>
    {
        public StudentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)

        {

        }
        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Student itm = await _context.Students.Where(x => x.StudentId == KeyValue).FirstOrDefaultAsync();
                _context.Students.Remove(itm);
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
            List<Student> lst = await _context.Students.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Student itm = await _context.Students.Where(x => x.StudentId == KeyValue).FirstOrDefaultAsync();
            return Ok(itm);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist = await _context.Students.Where(x => x.StudentId == _Item.StudentId).FirstOrDefaultAsync();
                if (exist != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Student already exists.");

                }
                exist = new Student();
                exist.StudentId = _Item.StudentId;
                exist.SchoolId = _Item.SchoolId;
                _context.Students.Add(exist);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.StudentId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }



        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Student _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist = await _context.Students.Where(x => x.StudentId == _Item.StudentId).FirstOrDefaultAsync();
                if (exist != null)
                {

                    await this.Post(_Item);
                    return Ok();
                }
                exist = new Student();
                exist.StudentId = _Item.StudentId;
                exist.SchoolId = _Item.SchoolId;
                _context.Students.Update(exist);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.StudentId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
