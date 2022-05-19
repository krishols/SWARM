using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Models;
using SWARM.Shared;
using SWARM.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using SWARM.Server.Controllers.Base;
namespace SWARM.Server.Controllers.Enroll
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : BaseController, iBaseController<Enrollment>
    {
        public EnrollmentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(string KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Enrollment itmCourse = await _context.Enrollments.Where(x => x.GuidId == KeyValue).FirstOrDefaultAsync();
                _context.Remove(itmCourse);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Enrollment> lstCourses = await _context.Enrollments.OrderBy(x => x.StudentGuidId).ToListAsync();
            return Ok(lstCourses);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(string KeyValue)
        {
            Section itmCourse = await _context.Sections.Where(x => x.GuidId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmCourse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Enrollment _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {

                var existCourse = await _context.Enrollments.Where(x => x.GuidId == _Item.GuidId).FirstOrDefaultAsync();

                if (existCourse != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Enrollment already exists.");
                }

                existCourse = new Enrollment();
                existCourse.GuidId = _Item.GuidId;
                existCourse.StudentGuidId = _Item.StudentGuidId;
                existCourse.SectionGuidId = _Item.SectionGuidId;
                _context.Enrollments.Add(existCourse);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.StudentGuid);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Enrollment _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existCourse = await _context.Enrollments.Where(x => x.GuidId == _Item.GuidId).FirstOrDefaultAsync();

                if (existCourse != null)
                {
                    await this.Post(_Item);
                    return Ok();
                }
                existCourse = new Enrollment();
                existCourse.GuidId = _Item.GuidId;
                existCourse.SectionGuidId = _Item.StudentGuidId;
                existCourse.SectionGuidId = _Item.SectionGuidId;
                _context.Enrollments.Add(existCourse);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.StudentGuidId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


    }
}
