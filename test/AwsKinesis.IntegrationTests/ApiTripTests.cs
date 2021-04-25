using App.Api.Models;
using AwsKinesis.IntegrationTests.Fixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using Xunit;

namespace AwsKinesis.IntegrationTests
{
    public class ApiTripTests : IClassFixture<WebApplicationFactory<App.Api.Startup>>, IClassFixture<KinesisFixture>
    {
        private readonly WebApplicationFactory<App.Api.Startup> _factory;

        public ApiTripTests(WebApplicationFactory<App.Api.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async System.Threading.Tasks.Task Get_Trip()
        {
            //arrange
            var client = _factory.CreateClient();

            //act
            var response = await client.GetAsync("/trip");

            //assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async System.Threading.Tasks.Task Post_return_OkAsync()
        {
            //arrange
            var client = _factory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(new TripModel
            {
                Engine = true,
                Ignition = true,
                Latitude = 13.2,
                Longitude = 13.2,
                Speed = 120,
                Time = new DateTime(2021, 04, 25, 13, 0, 0)
            }), Encoding.UTF8, "application/json");

            //act
            var response = await client.PostAsync("/trip", content);

            //assert
            response.EnsureSuccessStatusCode();
        }


    }
}