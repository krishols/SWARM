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
    public class GradeController : BaseController, iBaseController<Grade>
    {
        public GradeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)

        {

        }
        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Grade itm = await _context.Grades.Where(x => x.SectionId == KeyValue).FirstOrDefaultAsync();
                _context.Grades.Remove(itm);
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
            List<Grade> lst = await _context.Grades.OrderBy(x => x.SectionId).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Grade itm = await _context.Grades.Where(x => x.SectionId == KeyValue).FirstOrDefaultAsync();
            return Ok(itm);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Grade _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist = await _context.Grades.Where(x => x.SectionId == _Item.SectionId).FirstOrDefaultAsync();
                if (exist != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Grade already exists.");

                }
                exist = new Grade();
                exist.SectionId = _Item.SectionId;
                exist.StudentId = _Item.StudentId;
                exist.SchoolId = _Item.SchoolId;
                exist.GradeTypeCode = _Item.GradeTypeCode;
                exist.GradeTypeWeight = _Item.GradeTypeWeight;
                exist.NumericGrade = _Item.NumericGrade;
                _context.Grades.Add(exist);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.SectionId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }



        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Grade _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist = await _context.Grades.Where(x => x.SectionId == _Item.SectionId).FirstOrDefaultAsync();
                if (exist != null)
                {

                    await this.Post(_Item);
                    return Ok();
                }
                exist = new Grade();
                exist.SectionId = _Item.SectionId;
                exist.StudentId = _Item.StudentId;
                exist.SchoolId = _Item.SchoolId;
                exist.GradeTypeCode = _Item.GradeTypeCode;
                exist.GradeTypeWeight = _Item.GradeTypeWeight;
                exist.NumericGrade = _Item.NumericGrade;
                _context.Grades.Update(exist);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.SectionId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
