﻿using System.Net.Http.Headers;
using BowlingMVC.Servicelayer.Interfaces;
using Newtonsoft.Json;

namespace BowlingMVC.Servicelayer
{
    public class ApiService : IApiService
    {
        //Instantiate
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        //Constructor taking in API url from appsettings
        public ApiService(IConfiguration configuration)
        {
            _apiUrl = configuration["ServiceUrlToUse"];

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_apiUrl)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Get all - list - from database / API
        public async Task<List<T>> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to get data from API, status code: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<List<T>>(responseContent);

            return responseData;
        }

        // Get an object from database / API
        public async Task<T> GetAsynced<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to get data from API, status code: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<T>(responseContent);

            return responseData;
        }

        // Post to database
        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            var requestData = new StringContent(JsonConvert.SerializeObject(data));
            requestData.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.PostAsync(endpoint, requestData);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to post data to API, status code: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var responseData = JsonConvert.DeserializeObject<T>(responseContent);

            return responseData;
        }

        
    }
}
