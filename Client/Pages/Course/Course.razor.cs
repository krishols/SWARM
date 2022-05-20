using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SWARM.Client.Helper;
using SWARM.Client.Services;
using SWARM.Shared;
using SWARM.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace SWARM.Client.Pages.Course
{
    public partial class Course : SwarmUI
    {
        [CascadingParameter]
        private Task<AuthenticationState> authState { get; set; }
        private IEnumerable<CourseDTO> ieCourses { get; set; }
        private List<Course> lstcourse { get; set; }
        [Inject]
        CourseService _CourseService { get; set; }
        public TelerikGrid<CourseDTO> Grid { get; set; }

        public List<int?> PageSizes => true ? new List<int?> { 10, 25, 50, null } : null;
        private int PageSize = 10;
        private int PageIndex { get; set; } = 0;
        private async Task PageChangedHandler(int currPage)
        {
            PageIndex = currPage;
        }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            await LoadLookupData();
            IsLoading = false;
            await base.OnInitializedAsync();

        }
        private async Task LoadLookupData()
        {
            //School?
          lstcourse = await Http.GetFromJsonAsync<List<Course>>("api/Course/GetCourses", options);
        }

        public async Task ReadItems(GridReadEventArgs args)
        {
            IsLoading = true;
            DataEnvelope<CourseDTO> result = await _CourseService.GetCoursesService(args.Request);

            if (args.Request.Groups.Count > 0)
            {
                /***
                NO GROUPING FOR THE TIME BEING
                var data = GroupDataHelpers.DeserializeGroups<WeatherForecast>(result.GroupedData);
                GridData = data.Cast<object>().ToList();
                ***/
            }
            else
            {
                ieCourses = result.CurrentPageData.ToList();
            }

            args.Total = result.TotalItemCount;
            args.Data = result.CurrentPageData.ToList();

            IsLoading = false;

            StateHasChanged();
        }

        private async void UpdateCourse(GridCommandEventArgs e)
        {
            CourseDTO _CourseDTO = e.Item as CourseDTO;
        }

        private async void NewCourse(GridCommandEventArgs e)
        {
            String EmptyGuid = Guid.Empty.ToString();
            NavManager.NavigateTo($"/Course/Detail/{EmptyGuid}");
        }

        private void DeleteCourse(GridCommandEventArgs e)
        {
            CourseDTO _CourseDTO = e.Item as CourseDTO;
            NavManager.NavigateTo($"/Course/DeleteCourse/{_CourseDTO.CourseNo}");
        }


    }



}
