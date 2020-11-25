using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FootBallTournament.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace FootBallTournament.Tests
{
    [TestCaseOrderer("FootBallTournament.Tests.PriorityOrderer", "FootBallTournament.Tests")]
    public class NewFeatureTests
    {
          string token="";
          
        //   string viewername = new Random();
        //   string viewerPassword = new Random();
          
        private readonly ITestOutputHelper output;
        
    public NewFeatureTests(ITestOutputHelper output)
    {
     
       this.output=output;

     // _Client = _factory.CreateClient();
      
    }
     [Fact,TestPriority(8)]
        public async Task addviewer()
        {
              var _Client = new HttpClient();
              _Client.BaseAddress = new Uri("http://localhost:8001");
           
            var viewer = new viewer{Id="viewer1",name="viewer1",password="Dream@1506"};
            var content = JsonConvert.SerializeObject(viewer);
            var stringcontent = new StringContent(content,Encoding.UTF8,"application/json");
            var response = await _Client.PostAsync("viewer/register",stringcontent);
            response.StatusCode.Should().Be(201);

        }
         [Fact,TestPriority(9)]
        public async Task  login(){
             var _Client = new HttpClient();
              _Client.BaseAddress = new Uri("http://localhost:8001");
           
                var viewer =new viewer{name="viewer1",password="Dream@1506"};
                
                var content = JsonConvert.SerializeObject(viewer);
                var stringcontent = new StringContent(content,Encoding.UTF8,"application/json");

                var response =await _Client.PostAsync("viewer/login",stringcontent);
                 var token1 = await response.Content.ReadAsStringAsync();
        JObject json = JObject.Parse(token1);
         token = Convert.ToString(json.GetValue("token"));
         response.StatusCode.Should().Be(200);
         var response1 = await _Client.GetAsync("viewer/viewmatches");
         response1.StatusCode.Should().Be(401);
         _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
          var response2 = await _Client.GetAsync("viewer/viewmatches");
          response2.StatusCode.Should().Be(200);
          var responsestring = await response2.Content.ReadAsStringAsync();
          var mappings = JsonConvert.DeserializeObject<List<Mappings>>(responsestring);
          Assert.Equal(7,mappings.Count);
        }
          [Fact,TestPriority(10)]
        public async Task  bookmatch(){
             var _Client = new HttpClient();
              _Client.BaseAddress = new Uri("http://localhost:8001");
           
                var viewer =new viewer{name="viewer1",password="Dream@1506"};
                
                var content = JsonConvert.SerializeObject(viewer);
                var stringcontent = new StringContent(content,Encoding.UTF8,"application/json");

                var response =await _Client.PostAsync("viewer/login",stringcontent);
                 var token1 = await response.Content.ReadAsStringAsync();
        JObject json = JObject.Parse(token1);
         token = Convert.ToString(json.GetValue("token"));
         response.StatusCode.Should().Be(200);
         var response1 = await _Client.GetAsync("viewer/viewmatches");
         response1.StatusCode.Should().Be(401);
          _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
          var booking = new Bookings();
            var response2 = await _Client.GetAsync("viewer/viewmatches");
          response2.StatusCode.Should().Be(200);
          var responsestring = await response2.Content.ReadAsStringAsync();
          var mappings = JsonConvert.DeserializeObject<List<Mappings>>(responsestring);
          Assert.Equal(7,mappings.Count);
          var matchid = mappings.FirstOrDefault().Id;
          booking.Id="booking1";
          booking.quantity=2;
           var content1 = JsonConvert.SerializeObject(booking);
        var stringcontent1 = new StringContent(content1,Encoding.UTF8,"application/json");
        var response3= await _Client.PostAsync("viewer/book/"+matchid,stringcontent1);
        response3.StatusCode.Should().Be(201);
        var response4 = await _Client.GetAsync("viewer/bookings");
         var responsestring1 = await response4.Content.ReadAsStringAsync();
          var bookings = JsonConvert.DeserializeObject<List<Bookings>>(responsestring1);
          Assert.Equal(1,bookings.Count());
          Assert.Equal(matchid,bookings.FirstOrDefault().matchId);
          Assert.Equal("viewer1",bookings.FirstOrDefault().viewerId);


        }
        
    }
}