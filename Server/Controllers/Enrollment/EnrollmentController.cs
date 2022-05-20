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
    public class EnrollmentController : BaseController, iBaseController<EnrollmentDTO>
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
            List<Enrollment> lstCourses = await _context.Enrollments.OrderBy(x => x.GuidId).ToListAsync();
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
        public async Task<IActionResult> Post([FromBody] EnrollmentDTO _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {

                var existCourse = await _context.Enrollments.Where(x => x.GuidId == _Item.GuidId).FirstOrDefaultAsync();

                if (existCourse != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Enrollment already exists.");
                }

                var stu = await _context.Students.Where(x => x.FirstName == _Item.FirstName
               && x.LastName == _Item.LastName).FirstOrDefaultAsync();

                var sec = await _context.Sections.Where(x => x.SectionNo == _Item.SectionNo
                && x.CourseGuid.CourseNo == _Item.CourseNo).FirstOrDefaultAsync();

                existCourse = new Enrollment();
                existCourse.GuidId = _Item.GuidId;
                existCourse.SectionGuidId = sec.GuidId;
                existCourse.StudentGuidId = stu.GuidId;
                existCourse.CreatedBy = _Item.CreatedBy;
                existCourse.CreatedDate = _Item.CreatedDate;
                existCourse.ModifiedBy = _Item.ModifiedBy;
                existCourse.ModifiedDate = _Item.ModifiedDate;
                _context.Enrollments.Add(existCourse);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.GuidId);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] EnrollmentDTO _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existCourse = await _context.Enrollments.Where(x => x.GuidId == _Item.GuidId).FirstOrDefaultAsync();

                if (existCourse == null)
                {
                    await this.Post(_Item);
                    return Ok();
                }

                var stu = await _context.Students.Where(x => x.FirstName == _Item.FirstName
                && x.LastName == _Item.LastName).FirstOrDefaultAsync();

                var sec = await _context.Sections.Where(x => x.SectionNo == _Item.SectionNo
                && x.CourseGuid.CourseNo == _Item.CourseNo).FirstOrDefaultAsync();

                existCourse.GuidId = _Item.GuidId;
                existCourse.SectionGuidId = sec.GuidId;
                existCourse.StudentGuidId = stu.GuidId;
                _context.Enrollments.Update(existCourse);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.GuidId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        [HttpPost]
        [Route("GetEnrollments")]
        public async Task<DataEnvelope<EnrollmentDTO>> GetCoursesPost([FromBody] DataSourceRequest gridRequest)
        {
            DataEnvelope<EnrollmentDTO> dataToReturn = null;

            //Original context call didn't seem to be returning data

            IQueryable<EnrollmentDTO> queriableStates = _context.Enrollments
                .Select(sp => new EnrollmentDTO
                {
                    GuidId = sp.GuidId,
                    SectionGuidId = sp.SectionGuidId,
                    StudentGuidId = sp.StudentGuidId,
                    FirstName = sp.StudentGuid.FirstName,
                    LastName = sp.StudentGuid.LastName,
                    SectionNo = sp.SectionGuid.SectionNo,
                    CourseNo = sp.SectionGuid.CourseGuid.CourseNo,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
                }) ;


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
                    dataToReturn = new DataEnvelope<EnrollmentDTO>
                    {
                        GroupedData = processedData.Data.Cast<AggregateFunctionsGroup>().ToList(),
                        TotalItemCount = processedData.Total
                    };
                }
                else
                {
                    // When there is no grouping, the simplistic approach of 
                    // just serializing and deserializing the flat data is enough
                    dataToReturn = new DataEnvelope<EnrollmentDTO>
                    {
                        CurrentPageData = processedData.Data.Cast<EnrollmentDTO>().ToList(),
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
