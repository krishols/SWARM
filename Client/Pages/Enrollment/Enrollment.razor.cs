using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using SWARM.Client.Helper;
using SWARM.Client.Services;
using SWARM.Shared;
using SWARM.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SWARM.Client.Pages.Enrollment
{
    public partial class Enrollment : SwarmUI
    {
        [CascadingParameter]
        private Task<AuthenticationState> authState { get; set; }
        private IEnumerable<EnrollmentDTO> ieCourses { get; set; }
        private List<Enrollment> lstcourse { get; set; }
        [Inject]
        EnrollmentService _CourseService { get; set; }
        public TelerikGrid<EnrollmentDTO> Grid { get; set; }

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
            lstcourse = await Http.GetFromJsonAsync<List<Enrollment>>("api/Enrollment/GetEnrollments", options);
        }

        public async Task ReadItems(GridReadEventArgs args)
        {
            IsLoading = true;
            DataEnvelope<EnrollmentDTO> result = await _CourseService.GetCoursesService(args.Request);

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

        private async void UpdateEnrollment(GridCommandEventArgs e)
        {
            Console.WriteLine("HELLO FROM UPDATEENROLLMENT");
            EnrollmentDTO _EnrollmentDTO = e.Item as EnrollmentDTO;
            //var httpDTO = new JsonConvert.SerializeObject(_CourseDTO);
            var serDTO = JsonSerializer.Serialize(_EnrollmentDTO);
            //serDTO.Remove("GuidId");
            var result = await Http.PutAsync("api/Enrollment", new StringContent(serDTO, UnicodeEncoding.UTF8, "application/json"));
            Console.WriteLine(result.ToString());
        }

        private async void NewEnrollment(GridCommandEventArgs e)
        {
            Console.WriteLine("HELLO FROM NEWENROLLMENT");
            EnrollmentDTO _EnrollmentDTO = e.Item as EnrollmentDTO;
            var serDTO = JsonSerializer.Serialize(_EnrollmentDTO);
            var result = await Http.PostAsync("api/Enrollment", new StringContent(serDTO, UnicodeEncoding.UTF8, "application/json"));
        }

        private void DeleteEnrollment(GridCommandEventArgs e)
        {
            EnrollmentDTO _EnrollmentDTO = e.Item as EnrollmentDTO;
            var result = Http.DeleteAsync($"api/Enrollment/Delete/{_EnrollmentDTO.GuidId}");


        }


    }



}

