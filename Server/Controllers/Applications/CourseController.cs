using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
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

namespace SWARM.Server.Controllers.Crse
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : BaseController, iBaseController<Course>
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
        public async Task<IActionResult> Get(int KeyValue)
        {
            Course itmCourse = await _context.Courses.Where(x => x.CourseNo == KeyValue).FirstOrDefaultAsync();
            return Ok(itmCourse);
        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();

            try
            {

                var _sections = await _context.Sections.Where(x => x.CourseNo == KeyValue).ToListAsync();
                foreach (var _sec in _sections)
                {
                    var _enrollments = await _context.Enrollments.Where(x => x.SectionId == _sec.SectionId).ToListAsync();


                    foreach (var _enr in _enrollments)
                    {
                        _context.Enrollments.Remove(_enr);
                    }
                    _context.Sections.Remove(_sec);
                }

                Course itmCourse = await _context.Courses.Where(x => x.CourseNo == KeyValue).FirstOrDefaultAsync();
                _context.Courses.Remove(itmCourse);
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






        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Course _Item)
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
                existCourse.Cost = _Item.Cost;
                existCourse.Description = _Item.Description;
                existCourse.Prerequisite = _Item.Prerequisite;
                existCourse.PrerequisiteSchoolId = _Item.PrerequisiteSchoolId;
                existCourse.SchoolId = _Item.SchoolId;
                _context.Courses.Update(existCourse);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.CourseNo);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Course _Item)
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
                existCourse.Cost = _Item.Cost;
                existCourse.Description = _Item.Description;
                existCourse.Prerequisite = _Item.Prerequisite;
                existCourse.PrerequisiteSchoolId = _Item.PrerequisiteSchoolId;
                existCourse.SchoolId = _Item.SchoolId;
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

        
        [HttpPost]
        [Route("GetCourses")]
        public async Task<DataEnvelope<CourseDTO>> GetCoursesPost([FromBody] DataSourceRequest gridRequest)
        {
            DataEnvelope<CourseDTO> dataToReturn = null;
            IQueryable<CourseDTO> queriableStates = _context.Courses
                    .Select(sp => new CourseDTO
                    {
                        Cost = sp.Cost,
                        CourseNo = sp.CourseNo,
                        CreatedBy = sp.CreatedBy,
                        CreatedDate = sp.CreatedDate,
                        Description = sp.Description,
                        ModifiedBy = sp.ModifiedBy,
                        ModifiedDate = sp.ModifiedDate,
                        Prerequisite = sp.Prerequisite,
                        PrerequisiteSchoolId = sp.PrerequisiteSchoolId,
                        SchoolId = sp.SchoolId
                    });

            // use the Telerik DataSource Extensions to perform the query on the data
            // the Telerik extension methods can also work on "regular" collections like List<T> and IQueriable<T>
            try
            {

                DataSourceResult processedData = await queriableStates.ToDataSourceResultAsync(gridRequest);

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
                        CurrentPageData = processedData.Data.Cast<CourseDTO>().ToList(),
                        TotalItemCount = processedData.Total
                    };
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
