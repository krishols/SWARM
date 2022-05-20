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

namespace SWARM.Client.Pages.Section
{ 
    public partial class Section : SwarmUI
    {
        [CascadingParameter]
        private Task<AuthenticationState> authState { get; set; }
        private IEnumerable<SectionDTO> ieCourses { get; set; }
        private List<Section> lstcourse { get; set; }
        [Inject]
        SectionService _SectionService { get; set; }
        public TelerikGrid<SectionDTO> Grid { get; set; }

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
            lstcourse = await Http.GetFromJsonAsync<List<Section>>("api/Section/GetSections", options);
        }

        public async Task ReadItems(GridReadEventArgs args)
        {
            IsLoading = true;
            DataEnvelope<SectionDTO> result = await _SectionService.GetStudnetService(args.Request);

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

        private async void UpdateSection(GridCommandEventArgs e)
        {
            Console.WriteLine("HELLO FROM UPDATESECTION");
            SectionDTO _SectionDTO = e.Item as SectionDTO;
            var serDTO = JsonSerializer.Serialize(_SectionDTO);
            var result = await Http.PutAsync("api/Section", new StringContent(serDTO, UnicodeEncoding.UTF8, "application/json"));
            Console.WriteLine(result.ToString());
        }

        private async void NewSection(GridCommandEventArgs e)
        {
            Console.WriteLine("HELLO FROM NEWSECTION");
            SectionDTO _SectionDTO = e.Item as SectionDTO;
            //var httpDTO = new JsonConvert.SerializeObject(_CourseDTO);
            var serDTO = JsonSerializer.Serialize(_SectionDTO);
            //serDTO.Remove("GuidId");
            var result = await Http.PostAsync("api/Section", new StringContent(serDTO, UnicodeEncoding.UTF8, "application/json"));
        }

        private void DeleteSection(GridCommandEventArgs e)
        {
            SectionDTO _SectionDTO = e.Item as SectionDTO;
            var result = Http.DeleteAsync($"api/Section/Delete/{_SectionDTO.GuidId}");


        }


    }



}
