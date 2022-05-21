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

namespace SWARM.Client.Pages.Grade
{
    public partial class Grade : SwarmUI
    {
        [CascadingParameter]
        private Task<AuthenticationState> authState { get; set; }
        private IEnumerable<GradeDTO> ieCourses { get; set; }
        private List<Grade> lstcourse { get; set; }
        [Inject]
        GradeService _GradeService { get; set; }
        public TelerikGrid<GradeDTO> Grid { get; set; }

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
            lstcourse = await Http.GetFromJsonAsync<List<Grade>>("api/Grade/GetGrades", options);
        }

        public async Task ReadItems(GridReadEventArgs args)
        {
            IsLoading = true;
            DataEnvelope<GradeDTO> result = await _GradeService.GetGradeService(args.Request);

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

        private async void UpdateGrade(GridCommandEventArgs e)
        {
            Console.WriteLine("HELLO FROM UPDATEGRADE");
            GradeDTO _GradeDTO = e.Item as GradeDTO;
            //var httpDTO = new JsonConvert.SerializeObject(_CourseDTO);
            var serDTO = JsonSerializer.Serialize(_GradeDTO);
            //serDTO.Remove("GuidId");
            var result = await Http.PutAsync("api/Grade", new StringContent(serDTO, UnicodeEncoding.UTF8, "application/json"));
            Console.WriteLine(result.ToString());
        }

        private async void NewGrade(GridCommandEventArgs e)
        {
            Console.WriteLine("HELLO FROM NEWCOURSE");
            GradeDTO _GradeDTO = e.Item as GradeDTO;
            //var httpDTO = new JsonConvert.SerializeObject(_CourseDTO);
            var serDTO = JsonSerializer.Serialize(_GradeDTO);
            //serDTO.Remove("GuidId");
            var result = await Http.PostAsync("api/Grade", new StringContent(serDTO, UnicodeEncoding.UTF8, "application/json"));
        }

        private void DeleteGrade(GridCommandEventArgs e)
        {
            GradeDTO _GradeDTO = e.Item as GradeDTO;
            var result = Http.DeleteAsync($"api/Grade/Delete/{_GradeDTO.GuidId}");


        }


    }



}