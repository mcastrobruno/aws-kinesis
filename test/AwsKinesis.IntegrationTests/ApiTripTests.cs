using App.Api.Models;
using awskinesis.shared.Kinesis;
using AwsKinesis.IntegrationTests.Fixture;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace AwsKinesis.IntegrationTests
{
    public class ApiTripTests : IClassFixture<WebApplicationFactory<App.Api.Startup>>, IClassFixture<KinesisFixture>
    {
        private readonly WebApplicationFactory<App.Api.Startup> _factory;
        private readonly KinesisFixture _kinesisFixture;

        public ApiTripTests(WebApplicationFactory<App.Api.Startup> factory, KinesisFixture kinesisFixture)
        {
            _factory = factory;
            _kinesisFixture = kinesisFixture;
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

        [Fact]
        public async System.Threading.Tasks.Task Trip_Should_be_published_to_Kinesis()
        {
            //arrange
            var client = _factory.CreateClient();
            var trip = new TripModel
            {
                Engine = true,
                Ignition = true,
                Latitude = 13.2,
                Longitude = 13.2,
                Speed = 120,
                Time = new DateTime(2021, 04, 25, 13, 0, 0)
            };

            var content = new StringContent(JsonConvert.SerializeObject(trip), Encoding.UTF8, "application/json");

            //act
            var response = await client.PostAsync("/trip", content);

            //assert
            response.EnsureSuccessStatusCode();

            var tripPublished = await _kinesisFixture.KinesisConsumer.ReadNext<TripModel>(1);

            tripPublished.SingleOrDefault().Should().BeEquivalentTo(trip);
        }

        [Fact]
        public async System.Threading.Tasks.Task Trip_Should_not_be_published_if_ignition_is_missing()
        {
            //arrange
            var client = _factory.CreateClient();
            var trip = new TripModel
            {
                Engine = true,
                Latitude = 13.2,
                Longitude = 13.2,
                Speed = 120,
                Time = new DateTime(2021, 04, 25, 13, 0, 0)
            };

            var content = new StringContent(JsonConvert.SerializeObject(trip), Encoding.UTF8, "application/json");

            //act
            var response = await client.PostAsync("/trip", content);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var tripPublished = await _kinesisFixture.KinesisConsumer.ReadNext<TripModel>(1);

            tripPublished.SingleOrDefault().Should().BeNull();
        }


    }
}