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
    public class SectionController : BaseController, iBaseController<SectionDTO>
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
        public async Task<IActionResult> Post([FromBody] SectionDTO _Item)
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
                existCourse.CreatedBy = _Item.CreatedBy;
                existCourse.CreatedDate = _Item.CreatedDate;
                existCourse.ModifiedBy = _Item.ModifiedBy;
                existCourse.ModifiedDate = _Item.ModifiedDate;
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
        public async Task<IActionResult> Put([FromBody] SectionDTO _Item)
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
                existCourse.CreatedBy = _Item.CreatedBy;
                existCourse.CreatedDate = _Item.CreatedDate;
                existCourse.ModifiedBy = _Item.ModifiedBy;
                existCourse.ModifiedDate = _Item.ModifiedDate;
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

        [HttpPost]
        [Route("GetSections")]
        public async Task<DataEnvelope<SectionDTO>> GetCoursesPost([FromBody] DataSourceRequest gridRequest)
        {
            DataEnvelope<SectionDTO> dataToReturn = null;

            //Original context call didn't seem to be returning data

            IQueryable<SectionDTO> queriableStates = _context.Sections
                .Select(sp => new SectionDTO
                {
                    GuidId = sp.GuidId,
                    SectionNo = sp.SectionNo,
                    CourseGuidId = sp.CourseGuidId,
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
                    dataToReturn = new DataEnvelope<SectionDTO>
                    {
                        GroupedData = processedData.Data.Cast<AggregateFunctionsGroup>().ToList(),
                        TotalItemCount = processedData.Total
                    };
                }
                else
                {
                    // When there is no grouping, the simplistic approach of 
                    // just serializing and deserializing the flat data is enough
                    dataToReturn = new DataEnvelope<SectionDTO>
                    {
                        CurrentPageData = processedData.Data.Cast<SectionDTO>().ToList(),
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
