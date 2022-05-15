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
using SWARM.Server.Controllers.Enroll;

namespace SWARM.Server.Controllers.Sect
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : BaseController, iBaseController<Section>
    {
        public SectionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(string KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                List<Enrollment> secEnrolls = await _context.Enrollments.Where(x=> x.SectionGuidId == KeyValue).ToListAsync();
                if (secEnrolls != null)
                {
                    for (int i = 0; i < secEnrolls.Count(); i++)
                    {
                        _context.Remove(secEnrolls[i]);
                        await _context.SaveChangesAsync();
                    }
                }

                Section itmCourse = await _context.Sections.Where(x => x.GuidId == KeyValue).FirstOrDefaultAsync();
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
            List<Section> lstCourses = await _context.Sections.OrderBy(x => x.SectionNo).ToListAsync();
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
        public async Task<IActionResult> Post([FromBody] Section _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {

                var existCourse = await _context.Sections.Where(x => x.GuidId == _Item.GuidId).FirstOrDefaultAsync();

                if (existCourse != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Section already exists.");
                }

                existCourse = new Section();
                existCourse.GuidId = _Item.GuidId;
                existCourse.SectionNo = _Item.SectionNo;
                existCourse.CourseGuidId = _Item.CourseGuidId;
                _context.Sections.Add(existCourse);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.SectionNo);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Section _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existCourse = await _context.Sections.Where(x => x.GuidId == _Item.GuidId).FirstOrDefaultAsync();

                if (existCourse != null)
                {
                    await this.Post(_Item);
                    return Ok();
                }
                existCourse = new Section();
                existCourse.GuidId = _Item.GuidId;
                existCourse.SectionNo = _Item.SectionNo;
                existCourse.CourseGuidId = _Item.CourseGuidId;
                _context.Sections.Update(existCourse);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.SectionNo);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


    }
}
