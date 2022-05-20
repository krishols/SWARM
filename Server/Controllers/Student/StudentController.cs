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
namespace SWARM.Server.Controllers.Stu
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : BaseController, iBaseController<StudentDTO>
    {
        public StudentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(string KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Student itmCourse = await _context.Students.Where(x => x.GuidId == KeyValue).FirstOrDefaultAsync();
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
            List<Student> lstCourses = await _context.Students.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lstCourses);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(string KeyValue)
        {
            Student itmCourse = await _context.Students.Where(x => x.GuidId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmCourse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StudentDTO _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {

                var existCourse = await _context.Students.Where(x => x.StudentId == _Item.StudentId).FirstOrDefaultAsync();

                if (existCourse != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Enrollment already exists.");
                }

                existCourse = new Student();
                existCourse.GuidId = _Item.GuidId;
                existCourse.StudentId = _Item.StudentId;
                existCourse.FirstName = _Item.FirstName;
                existCourse.LastName = _Item.LastName;
                existCourse.CreatedBy = _Item.CreatedBy;
                existCourse.CreatedDate = _Item.CreatedDate;
                existCourse.ModifiedBy = _Item.ModifiedBy;
                existCourse.ModifiedDate = _Item.ModifiedDate;
                _context.Students.Add(existCourse);
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
        public async Task<IActionResult> Put([FromBody] StudentDTO _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existCourse = await _context.Students.Where(x => x.GuidId == _Item.GuidId).FirstOrDefaultAsync();

                if (existCourse == null)
                {
                    trans.Commit();
                    await this.Post(_Item);
                    return Ok();
                }
                existCourse.GuidId = _Item.GuidId;
                existCourse.StudentId = _Item.StudentId;
                existCourse.FirstName = _Item.FirstName;
                existCourse.LastName = _Item.LastName;
                existCourse.CreatedBy = _Item.CreatedBy;
                existCourse.CreatedDate = _Item.CreatedDate;
                existCourse.ModifiedBy = _Item.ModifiedBy;
                existCourse.ModifiedDate = _Item.ModifiedDate;
                _context.Students.Update(existCourse);
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
        [Route("GetStudents")]
        public async Task<DataEnvelope<StudentDTO>> GetCoursesPost([FromBody] DataSourceRequest gridRequest)
        {
            DataEnvelope<StudentDTO> dataToReturn = null;

            //Original context call didn't seem to be returning data

            IQueryable<StudentDTO> queriableStates = _context.Students
                .Select(sp => new StudentDTO
                {
                    GuidId = sp.GuidId,
                    StudentId = sp.StudentId,
                    FirstName = sp.FirstName,
                    LastName = sp.LastName,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
                });

            // PROF'S ORIG CODE
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
                    dataToReturn = new DataEnvelope<StudentDTO>
                    {
                        GroupedData = processedData.Data.Cast<AggregateFunctionsGroup>().ToList(),
                        TotalItemCount = processedData.Total
                    };
                }
                else
                {
                    // When there is no grouping, the simplistic approach of 
                    // just serializing and deserializing the flat data is enough
                    dataToReturn = new DataEnvelope<StudentDTO>
                    {
                        CurrentPageData = processedData.Data.Cast<StudentDTO>().ToList(),
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
