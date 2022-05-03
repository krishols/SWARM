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
    public class GradeTypeController : BaseController, iBaseController<GradeType>
    {
        public GradeTypeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)

        {
            
        }
        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                GradeType itm = await _context.GradeTypes.Where(x => int.Parse(x.GradeTypeCode) == KeyValue).FirstOrDefaultAsync();
                _context.GradeTypes.Remove(itm);
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
            List<GradeType> lst = await _context.GradeTypes.OrderBy(x => x.GradeTypeCode).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            GradeType itm = await _context.GradeTypes.Where(x => int.Parse(x.GradeTypeCode) == KeyValue).FirstOrDefaultAsync();
            return Ok(itm);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeType _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist = await _context.GradeTypes.Where(x => x.GradeTypeCode == _Item.GradeTypeCode).FirstOrDefaultAsync();
                if (exist != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Grade Type already exists.");

                }
                exist = new GradeType();
                exist.SchoolId = _Item.SchoolId;
                exist.GradeTypeCode = _Item.GradeTypeCode;
                _context.GradeTypes.Add(exist);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.GradeTypeCode);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }



        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeType _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist = await _context.GradeTypes.Where(x => x.GradeTypeCode == _Item.GradeTypeCode).FirstOrDefaultAsync();
                if (exist != null)
                {

                    await this.Post(_Item);
                    return Ok();
                }
                exist = new GradeType();
                exist.SchoolId = _Item.SchoolId;
                exist.GradeTypeCode = _Item.GradeTypeCode;
                _context.GradeTypes.Update(exist);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.GradeTypeCode);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
