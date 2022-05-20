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

namespace SWARM.Client.Pages.School
{
    public partial class School : SwarmUI
    {
        [CascadingParameter]
        private Task<AuthenticationState> authState { get; set; }
        private IEnumerable<SchoolDTO> ieCourses { get; set; }
        private List<School> lstcourse { get; set; }
        [Inject]
        SchoolService _SchoolService { get; set; }
        public TelerikGrid<SchoolDTO> Grid { get; set; }

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
            lstcourse = await Http.GetFromJsonAsync<List<School>>("api/School/GetSchools", options);
        }

        public async Task ReadItems(GridReadEventArgs args)
        {
            IsLoading = true;
            DataEnvelope<SchoolDTO> result = await _SchoolService.GetSchoolService(args.Request);

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

        private async void UpdateSchool(GridCommandEventArgs e)
        {
            Console.WriteLine("HELLO FROM UPDATESCHOOL");
            SchoolDTO _SchoolDTO = e.Item as SchoolDTO;
            var serDTO = JsonSerializer.Serialize(_SchoolDTO);
            var result = await Http.PutAsync("api/School", new StringContent(serDTO, UnicodeEncoding.UTF8, "application/json"));
            Console.WriteLine(result.ToString());

        }

        private async void NewSchool(GridCommandEventArgs e)
        {
            Console.WriteLine("HELLO FROM NEWSCHOOL");
            SchoolDTO _SchoolDTO = e.Item as SchoolDTO;
            var serDTO = JsonSerializer.Serialize(_SchoolDTO);
            var result = await Http.PostAsync("api/School", new StringContent(serDTO, UnicodeEncoding.UTF8, "application/json"));
        }

        private void DeleteSchool(GridCommandEventArgs e)
        {
            SchoolDTO _SchoolDTO= e.Item as SchoolDTO;
            var result = Http.DeleteAsync($"api/School/Delete/{_SchoolDTO.GuidId}");


        }


    }



}
