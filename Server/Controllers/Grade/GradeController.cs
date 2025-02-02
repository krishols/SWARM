﻿using AutoMapper;
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
namespace SWARM.Server.Controllers.Grd
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : BaseController, iBaseController<GradeDTO>
    {
        public GradeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(string KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Grade itmCourse = await _context.Grades.Where(x => x.GuidId == KeyValue).FirstOrDefaultAsync();
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
            List<Grade> lstCourses = await _context.Grades.OrderBy(x => x.GuidId).ToListAsync();
            return Ok(lstCourses);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(string KeyValue)
        {
            Grade itmCourse = await _context.Grades.Where(x => x.GuidId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmCourse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeDTO _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {

                var existCourse = await _context.Grades.Where(x => x.GuidId == _Item.GuidId).FirstOrDefaultAsync();

                if (existCourse != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Enrollment already exists.");
                }

                var enr = await _context.Enrollments.Where(x => x.SectionGuid.CourseGuid.CourseNo == _Item.CourseNo
                    &&
                    x.SectionGuid.SectionNo == _Item.SectionNo
                    &&
                    x.StudentGuid.StudentId == _Item.StudentId).FirstOrDefaultAsync();
                if (enr != null)
                {
                    existCourse = new Grade();
                    existCourse.GuidId = _Item.GuidId;
                    existCourse.EnrollmentGuidId = enr.GuidId;
                    existCourse.Grade1 = _Item.Grade;
                    existCourse.CreatedBy = _Item.CreatedBy;
                    existCourse.CreatedDate = _Item.CreatedDate;
                    existCourse.ModifiedBy = _Item.ModifiedBy;
                    existCourse.ModifiedDate = _Item.ModifiedDate;
                    _context.Grades.Add(existCourse);
                    await _context.SaveChangesAsync();
                    trans.Commit();
                    return Ok(_Item.GuidId);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Error in Post");

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeDTO _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existCourse = await _context.Grades.Where(x => x.GuidId == _Item.GuidId).FirstOrDefaultAsync();

                if (existCourse == null)
                {
                    await this.Post(_Item);
                    return Ok();
                }
                var enr = await _context.Enrollments.Where(x => x.SectionGuid.CourseGuid.CourseNo == _Item.CourseNo
                    &&
                    x.SectionGuid.SectionNo == _Item.SectionNo
                    &&
                    x.StudentGuid.StudentId == _Item.StudentId).FirstOrDefaultAsync();
                        if (enr != null)
                        {
                            existCourse.GuidId = _Item.GuidId;
                            existCourse.EnrollmentGuidId = enr.GuidId;
                            existCourse.Grade1 = _Item.Grade;
                            existCourse.CreatedBy = _Item.CreatedBy;
                            existCourse.CreatedDate = _Item.CreatedDate;
                            existCourse.ModifiedBy = _Item.ModifiedBy;
                            existCourse.ModifiedDate = _Item.ModifiedDate;
                            _context.Grades.Update(existCourse);
                            await _context.SaveChangesAsync();
                            trans.Commit();
                            return Ok(_Item.GuidId);
                        }
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in Grade Post");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("GetGrades")]
        public async Task<DataEnvelope<GradeDTO>> GetCoursesPost([FromBody] DataSourceRequest gridRequest)
        {
            DataEnvelope<GradeDTO> dataToReturn = null;

            //Original context call didn't seem to be returning data

            IQueryable<GradeDTO> queriableStates = _context.Grades
                .Select(sp => new GradeDTO
                {
                    GuidId = sp.GuidId,
                    EnrollmentGuidId = sp.EnrollmentGuidId,
                    Grade = sp.Grade1,
                    FirstName = sp.EnrollmentGuid.StudentGuid.FirstName,
                    LastName = sp.EnrollmentGuid.StudentGuid.LastName,
                    CourseNo = sp.EnrollmentGuid.SectionGuid.CourseGuid.CourseNo,
                    SectionNo = sp.EnrollmentGuid.SectionGuid.SectionNo,
                    StudentId = sp.EnrollmentGuid.StudentGuid.StudentId,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
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
                    dataToReturn = new DataEnvelope<GradeDTO>
                    {
                        GroupedData = processedData.Data.Cast<AggregateFunctionsGroup>().ToList(),
                        TotalItemCount = processedData.Total
                    };
                }
                else
                {
                    // When there is no grouping, the simplistic approach of 
                    // just serializing and deserializing the flat data is enough
                    dataToReturn = new DataEnvelope<GradeDTO>
                    {
                        CurrentPageData = processedData.Data.Cast<GradeDTO>().ToList(),
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
