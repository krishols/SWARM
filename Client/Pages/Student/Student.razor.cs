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

namespace SWARM.Client.Pages.Student
{
    public partial class Student : SwarmUI
    {
        [CascadingParameter]
        private Task<AuthenticationState> authState { get; set; }
        private IEnumerable<StudentDTO> ieCourses { get; set; }
        private List<Student> lstcourse { get; set; }
        [Inject]
        StudentService _StudentService { get; set; }
        public TelerikGrid<StudentDTO> Grid { get; set; }

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
            lstcourse = await Http.GetFromJsonAsync<List<Student>>("api/Student/GetStudents", options);
        }

        public async Task ReadItems(GridReadEventArgs args)
        {
            IsLoading = true;
            DataEnvelope<StudentDTO> result = await _StudentService.GetStudnetService(args.Request);

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

        private async void UpdateStudent(GridCommandEventArgs e)
        {
            Console.WriteLine("HELLO FROM UPDATESTUDENT");
            StudentDTO _StudentDTO= e.Item as StudentDTO;
            var serDTO = JsonSerializer.Serialize(_StudentDTO);
            Console.WriteLine(serDTO);
            var result = await Http.PutAsync("api/Student", new StringContent(serDTO, UnicodeEncoding.UTF8, "application/json"));
            Console.WriteLine(result.ToString());

        }

        private async void NewStudent(GridCommandEventArgs e)
        {
            Console.WriteLine("HELLO FROM NEWSCHOOL");
            StudentDTO _StudentDTO = e.Item as StudentDTO;
            _StudentDTO.GuidId = "temp";
            var serDTO = JsonSerializer.Serialize(_StudentDTO);
            Console.WriteLine(serDTO);
            var result = await Http.PostAsync("api/Student", new StringContent(serDTO, UnicodeEncoding.UTF8, "application/json"));
        }

        private void DeleteStudent(GridCommandEventArgs e)
        {
            StudentDTO _StudetnDTO = e.Item as StudentDTO;
            var result = Http.DeleteAsync($"api/Student/Delete/{_StudetnDTO.GuidId}");


        }


    }



}
