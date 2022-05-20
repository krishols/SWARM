﻿using System;
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
    public class StudentService
    {
        protected HttpClient Http { get; set; }

        public StudentService(HttpClient client)
        {
            Http = client;
        }

        public async Task<DataEnvelope<StudentDTO>> GetStudnetService(DataSourceRequest gridRequest)
        {
            //options very important here !!!!!!!!!!
            var options = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                PropertyNameCaseInsensitive = true
            };
            HttpResponseMessage response = await Http.PostAsJsonAsync("api/Student/GetStudents", gridRequest);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var JsonData = await response.Content.ReadFromJsonAsync<DataEnvelope<StudentDTO>>(options);
                return JsonData;
            }

            throw new Exception($"The service returned with status {response.StatusCode}");
        }

    }
}