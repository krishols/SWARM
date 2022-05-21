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
namespace SWARM.Server.Controllers.Crse
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : BaseController, iBaseController<CourseDTO>
    { 
    public CourseController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Course> lstCourses = await _context.Courses.OrderBy(x => x.CourseNo).ToListAsync();
            return Ok(lstCourses);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(string KeyValue)
        {
            Course itmCourse = await _context.Courses.Where(x => x.GuidId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmCourse);
        }

        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(string KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                List<Section> courseSecs = await _context.Sections.Where(x => x.CourseGuidId == KeyValue).ToListAsync();
                if (courseSecs != null)
                {
                    for (int j = 0; j < courseSecs.Count(); j++)
                    {
                        List<Enrollment> secEnrolls = await _context.Enrollments.Where(x => x.SectionGuidId == courseSecs[j].GuidId).ToListAsync();
                        if (secEnrolls != null)
                        {
                            for (int i = 0; i < secEnrolls.Count(); i++)
                            {
                                _context.Remove(secEnrolls[i]);
                                await _context.SaveChangesAsync();
                            }
                        }
                        _context.Remove(courseSecs[j]);
                        await _context.SaveChangesAsync();

                    }
                }


                Course itmCourse = await _context.Courses.Where(x => x.GuidId == KeyValue).FirstOrDefaultAsync();
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CourseDTO _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {

                var existCourse = await _context.Courses.Where(x => x.CourseNo == _Item.CourseNo).FirstOrDefaultAsync();
                
                if (existCourse != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Course already exists.");
                }

                var schguid = await _context.Schools.Where(x => x.SchoolName == _Item.SchoolName).FirstOrDefaultAsync();

                existCourse = new Course();
                existCourse.GuidId = _Item.GuidId;
                existCourse.CourseName = _Item.CourseName;
                existCourse.PrereqGuidId = _Item.PrereqGuidId;
                existCourse.SchoolGuidId = schguid.GuidId;
                existCourse.CourseNo = _Item.CourseNo;
                _context.Courses.Add(existCourse);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.CourseNo);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CourseDTO _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existCourse = await _context.Courses.Where(x => x.CourseNo == _Item.CourseNo).FirstOrDefaultAsync();

                if (existCourse == null)
                {
                    trans.Commit();
                    await this.Post(_Item);
                    return Ok();
                }
                //existCourse = new Course();
                existCourse.CourseName = _Item.CourseName;
                existCourse.PrereqGuidId = _Item.PrereqGuidId;
                existCourse.SchoolGuidId = _Item.SchoolGuidId;
                existCourse.CourseNo = _Item.CourseNo;
                _context.Courses.Update(existCourse);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.CourseNo);
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("GetCourses")]
        public async Task<DataEnvelope<CourseDTO>> GetCoursesPost([FromBody] DataSourceRequest gridRequest)
        {
            DataEnvelope<CourseDTO> dataToReturn = null;


            IQueryable<CourseDTO> queriableStates = _context.Courses
                .Select(sp => new CourseDTO
                {
                    CourseNo = sp.CourseNo,
                    CourseName = sp.CourseName,
                    GuidId = sp.GuidId,
                    PrereqGuidId = sp.PrereqGuidId,
                    SchoolGuidId = sp.SchoolGuidId,
                    SchoolName = sp.SchoolGuid.SchoolName,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
                }) ;


            try
            {
                DataSourceResult processedData = await queriableStates.ToDataSourceResultAsync(gridRequest);
                if (gridRequest.Groups.Count > 0)
                {
                    
                    dataToReturn = new DataEnvelope<CourseDTO>
                    {
                        GroupedData = processedData.Data.Cast<AggregateFunctionsGroup>().ToList(),
                        TotalItemCount = processedData.Total
                    };
                }
                else
                {
                    dataToReturn = new DataEnvelope<CourseDTO>
                    {
                        CurrentPageData = processedData.Data.Cast<CourseDTO>().ToList(),
                        TotalItemCount = processedData.Total
                    };
                }
            }
           
            catch (Exception e)
            {
            }
            return dataToReturn;
        }
    }
}
