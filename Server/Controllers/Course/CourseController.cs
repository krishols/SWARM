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

                existCourse = new Course();
                existCourse.GuidId = _Item.GuidId;
                existCourse.CourseName = _Item.CourseName;
                existCourse.PrereqGuidId = _Item.PrereqGuidId;
                existCourse.SchoolGuidId = _Item.SchoolGuidId;
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

                if (existCourse != null)
                {
                    await this.Post(_Item);
                    return Ok();
                }
                existCourse = new Course();
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

            //Original context call didn't seem to be returning data
            /*
            IQueryable<CourseDTO> queriableStates = _context.Courses
                .Select(sp => new CourseDTO
                {
                    CourseNo = sp.CourseNo,
                    CourseName = sp.CourseName,
                    GuidId = sp.GuidId,
                    PrereqGuidId = sp.PrereqGuidId,
                    SchoolGuidId = sp.SchoolGuidId,
                    SchoolName = sp.SchoolGuid.SchoolName
                }) ;
            */

            //Gets all courses in a list
            List<Course> lstCourses = await _context.Courses.OrderBy(x => x.CourseNo).ToListAsync();

            //Convert Courses to CourseDTO and adds it to new list
            List<CourseDTO> DTOlst = new List<CourseDTO>();
            foreach (Course c in lstCourses)
            {
                CourseDTO c1 = new CourseDTO();
                c1.CourseName = c.CourseName;
                c1.CourseNo = c.CourseNo;
                c1.GuidId = c.GuidId;
                c1.PrereqGuidId = c.PrereqGuidId;
                c1.SchoolGuidId = c.SchoolGuidId;
                DTOlst.Add(c1);

            }


            // use the Telerik DataSource Extensions to perform the query on the data
            // the Telerik extension methods can also work on "regular" collections like List<T> and IQueriable<T>
            try
            {

                //Not sure what this call does, seems to also lose the data though
                DataSourceResult processedData = await lstCourses.ToDataSourceResultAsync(gridRequest);

                if (gridRequest.Groups.Count > 0)
                {
                    // If there is grouping, use the field for grouped data
                    // The app must be able to serialize and deserialize it
                    // Example helper methods for this are available in this project
                    // See the GroupDataHelper.DeserializeGroups and JsonExtensions.Deserialize methods
                    dataToReturn = new DataEnvelope<CourseDTO>
                    {
                        GroupedData = processedData.Data.Cast<AggregateFunctionsGroup>().ToList(),
                        TotalItemCount = processedData.Total
                    };
                }
                else
                {
                    // When there is no grouping, the simplistic approach of 
                    // just serializing and deserializing the flat data is enough
                    dataToReturn = new DataEnvelope<CourseDTO>
                    {
                        //Adding DTOlst to current page data instead of process data like original seen below
                        CurrentPageData = DTOlst,
                        TotalItemCount = processedData.Total
                    };
                    /*
                    dataToReturn = new DataEnvelope<CourseDTO>
                    {
                        CurrentPageData = processedData.Data.Cast<CourseDTO>().ToList(),
                        TotalItemCount = processedData.Total
                    };
                    */
                }
            }
            catch (Exception e)
            {
                //fixme add decent exception handling
            }
            return dataToReturn;
        }
    }
}
