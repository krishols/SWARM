using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Telerik.DataSource;
using SWARM.Shared.DTO;
using SWARM.EF.Models;
using SWARM.Shared;


namespace SWARM.Client.Services
{
    public class CourseService
    {
        protected HttpClient Http { get; set; }

        public CourseService(HttpClient client)
        {
            Http = client;
        }

        public async Task<DataEnvelope<CourseDTO>> GetCoursesService(DataSourceRequest gridRequest)
        {
            //options very important here !!!!!!!!!!
            var options = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                PropertyNameCaseInsensitive = true
            };
            HttpResponseMessage response = await Http.PostAsJsonAsync("api/Course/GetCourses", gridRequest);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var JsonData = await response.Content.ReadFromJsonAsync<DataEnvelope<CourseDTO>>(options);
                return JsonData;
            }

            throw new Exception($"The service returned with status {response.StatusCode}");
        }

        public async void CreateCoursesService(CourseDTO course)
        {
            /*
            if (!_products.Any())
            {
                product.ProductId = 1;
            }
            else
            {
                product.ProductId = _products.Max(p => p.ProductId) + 1;
            }

            _products.Insert(0, product);
            */
        }

    }
}
