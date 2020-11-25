using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FootBallTournament.Models;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit.Abstractions;
using System.Linq;
using FootBallTournament.Tests;

namespace FootBallTournament.Tests
{
    [TestCaseOrderer("FootBallTournament.Tests.PriorityOrderer", "FootBallTournament.Tests")]
    public class FootBallTournamentTests
    {
        string token="";
        private readonly ITestOutputHelper output;
        
    public FootBallTournamentTests(ITestOutputHelper output)
    {
     
       this.output=output;

     // _Client = _factory.CreateClient();
      
    }
    //  private TestServer _server;

    //     public HttpClient _Client { get; private set; }

    //     public FootBallTournamentTests()
    //     {
    //         SetUpClient();
    //     }
        [Fact,TestPriority(1)]
        public async Task addTeam()
        {
              var _Client = new HttpClient();
              _Client.BaseAddress = new Uri("http://localhost:8001");
            //  await _Client.GetAsync("admin/database");
            var team = new Teams{Id="team1",name="dream1",password="Dream@1506",country="India",coach="dreamcoach"};
            var content = JsonConvert.SerializeObject(team);
            var stringcontent = new StringContent(content,Encoding.UTF8,"application/json");
            var response = await _Client.PostAsync("teams/registration",stringcontent);
            response.StatusCode.Should().Be(201);

        }
           [Fact,TestPriority(2)]
        public async Task addTeam2()
        {       var _Client = new HttpClient();
              _Client.BaseAddress = new Uri("http://localhost:8001");
            
            // await _Client.GetAsync("admin/database");
            var team = new Teams{Id="team2",name="dream2",password="Dream@1506",country="India",coach="dreamcoach"};
            var content = JsonConvert.SerializeObject(team);
            var stringcontent = new StringContent(content,Encoding.UTF8,"application/json");
            var response = await _Client.PostAsync("teams/registration",stringcontent);
            response.StatusCode.Should().Be(201);

        }
         [Fact,TestPriority(3)]
        public async Task loginAdmin(){
            var _Client = new HttpClient();
              _Client.BaseAddress = new Uri("http://localhost:8001");
           
                var user =new Admin{name="Admin",password="Admin@1506"};
                
                var content = JsonConvert.SerializeObject(user);
                var stringcontent = new StringContent(content,Encoding.UTF8,"application/json");

                var response =await _Client.PostAsync("admin/login",stringcontent);
                 var token1 = await response.Content.ReadAsStringAsync();
        JObject json = JObject.Parse(token1);
         token = Convert.ToString(json.GetValue("token"));
         response.StatusCode.Should().Be(200);
         //_Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
         var response1 = await _Client.GetAsync("admin/teams/view");
         response1.StatusCode.Should().Be(401);
         _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
          var response2 = await _Client.GetAsync("admin/teams/view");
          response2.StatusCode.Should().Be(200);
          var responsestring = await response2.Content.ReadAsStringAsync();
          var teams = JsonConvert.DeserializeObject<List<Teams>>(responsestring);
          Assert.Equal(2,teams.Count);
        }
        [Fact,TestPriority(4)]
        public async Task updateTeam(){
            var _Client = new HttpClient();
              _Client.BaseAddress = new Uri("http://localhost:8001");
           
              var user =new Admin{name="Admin",password="Admin@1506"};
                
                var content = JsonConvert.SerializeObject(user);
                var stringcontent = new StringContent(content,Encoding.UTF8,"application/json");

                var response =await _Client.PostAsync("admin/login",stringcontent);
                 var token1 = await response.Content.ReadAsStringAsync();
        JObject json = JObject.Parse(token1);
         token = Convert.ToString(json.GetValue("token"));
         _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
        var team = new Teams{name="dream1",password="Dream@1506",country="India",coach="coach"};
        var content1 = JsonConvert.SerializeObject(team);
        var stringcontent1 = new StringContent(content1,Encoding.UTF8,"application/json");
        var response1 = await _Client.PutAsync("admin/teams/update/team1",stringcontent1);
        response1.StatusCode.Should().Be(200);
        var response2 = await _Client.DeleteAsync("admin/teams/delete/team2");
        response2.StatusCode.Should().Be(200);
         var response3 = await _Client.GetAsync("admin/teams/view");
          response3.StatusCode.Should().Be(200);
          var responsestring = await response3.Content.ReadAsStringAsync();
          var teams = JsonConvert.DeserializeObject<List<Teams>>(responsestring);
          Assert.Equal(1,teams.Count);
        }
        [Fact,TestPriority(5)]
        public async Task addPlayersAdmin(){
            var _Client = new HttpClient();
              _Client.BaseAddress = new Uri("http://localhost:8001");
            
              var user =new Admin{name="Admin",password="Admin@1506"};
                
                var content = JsonConvert.SerializeObject(user);
                var stringcontent = new StringContent(content,Encoding.UTF8,"application/json");

                var response =await _Client.PostAsync("admin/login",stringcontent);
                 var token1 = await response.Content.ReadAsStringAsync();
        JObject json = JObject.Parse(token1);
         token = Convert.ToString(json.GetValue("token"));
           var response4 = await _Client.GetAsync("admin/players/view/team1");
        response4.StatusCode.Should().Be(401);
         _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
         var player = new Players{Id="player1",name="dreamplayer1",type="Mid-Fielder",noOfMatches=20,age=22,goalsScored=22,inEleven=false};
         var content1 = JsonConvert.SerializeObject(player);
        var stringcontent1 = new StringContent(content1,Encoding.UTF8,"application/json");
        var response2= await _Client.PostAsync("admin/players/register/team1",stringcontent1);
        response2.StatusCode.Should().Be(201);
        var player2 = new Players{Id="player2",name="dreamplayer2",type="Goal Keeper",noOfMatches=20,age=22,goalsScored=22,inEleven=false};
         var content2 = JsonConvert.SerializeObject(player2);
        var stringcontent2 = new StringContent(content2,Encoding.UTF8,"application/json");
        var response3= await _Client.PostAsync("admin/players/register/team1",stringcontent2);
        response3.StatusCode.Should().Be(201);
          var response5 = await _Client.GetAsync("admin/players/view/team1");
        response5.StatusCode.Should().Be(200);
        var responsestring = await response5.Content.ReadAsStringAsync();
        var players = JsonConvert.DeserializeObject<List<Players>>(responsestring);
        Assert.Equal(2,players.Count);
        var response6 = await _Client.DeleteAsync("admin/players/deleteAll/team1");
        response6.StatusCode.Should().Be(200);
        var response7 = await _Client.GetAsync("admin/players/view/team1");
        response7.StatusCode.Should().Be(200);
        var responsestring1 = await response7.Content.ReadAsStringAsync();
        var players1 = JsonConvert.DeserializeObject<List<Players>>(responsestring1);
        Assert.Equal(0,players1.Count);
      

        }
        [Fact,TestPriority(6)]
        public async Task teamLogin(){
            var _Client = new HttpClient();
              _Client.BaseAddress = new Uri("http://localhost:8001");
             
              var user =new Teams{name="dream1",password="Dream@1506"};
                
                var content = JsonConvert.SerializeObject(user);
                var stringcontent = new StringContent(content,Encoding.UTF8,"application/json");

                var response =await _Client.PostAsync("teams/login",stringcontent);
                response.StatusCode.Should().Be(200);
                 var token1 = await response.Content.ReadAsStringAsync();
                  JObject json = JObject.Parse(token1);
         token = Convert.ToString(json.GetValue("token"));
         _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
         
          var team = new Teams{name="dream",password="Dream@1506",country="India",coach="dreamcoach"};
            var content1 = JsonConvert.SerializeObject(team);
            var stringcontent1 = new StringContent(content1,Encoding.UTF8,"application/json");
            var response3 = await _Client.PutAsync("teams/update",stringcontent1);
            response3.StatusCode.Should().Be(200);
            var response1 = await _Client.GetAsync("teams/view");
         response1.StatusCode.Should().Be(200);
         var responsestring = await response1.Content.ReadAsStringAsync();
         var teams = JsonConvert.DeserializeObject<List<Teams>>(responsestring);
         Assert.Equal(1,teams.Count);
         Assert.Equal("dreamcoach",teams[0].coach);
        }
         [Fact,TestPriority(7)]
        public async Task addPlayersTeam(){
            var _Client = new HttpClient();
              _Client.BaseAddress = new Uri("http://localhost:8001");
              
              var user =new Teams{name="dream1",password="Dream@1506"};
                
                var content = JsonConvert.SerializeObject(user);
                var stringcontent = new StringContent(content,Encoding.UTF8,"application/json");

                var response =await _Client.PostAsync("teams/login",stringcontent);
                response.StatusCode.Should().Be(200);
                 var token1 = await response.Content.ReadAsStringAsync();
                  JObject json = JObject.Parse(token1);
         token = Convert.ToString(json.GetValue("token"));
         _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
           var player = new Players{Id="player1",name="dreamplayer1",type="Mid-Fielder",noOfMatches=20,age=22,goalsScored=22,inEleven=false};
         var content1 = JsonConvert.SerializeObject(player);
        var stringcontent1 = new StringContent(content1,Encoding.UTF8,"application/json");
        var response2= await _Client.PostAsync("players/register",stringcontent1);
        response2.StatusCode.Should().Be(201);
        var player2 = new Players{Id="player2",name="dreamplayer2",type="Goal Keeper",noOfMatches=20,age=22,goalsScored=22,inEleven=false};
         var content2 = JsonConvert.SerializeObject(player2);
        var stringcontent2 = new StringContent(content2,Encoding.UTF8,"application/json");
        var response3= await _Client.PostAsync("players/register",stringcontent2);
        response3.StatusCode.Should().Be(201);
         var response1 = await _Client.GetAsync("players/view");
         response1.StatusCode.Should().Be(200);
         var responsestring = await response1.Content.ReadAsStringAsync();
         var players = JsonConvert.DeserializeObject<List<Players>>(responsestring);
         Assert.Equal(2,players.Count);
      

         
        }
        //   private void SetUpClient()
        // {
        //     var builder = new WebHostBuilder()
        //         .UseStartup<Startup>()
        //         .ConfigureServices(services =>
        //         {
        //            var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
        //                 .UseSqlite("DataSource=:memory:")
        //                 .EnableSensitiveDataLogging()
        //                 .Options);

        //             services.RemoveAll(typeof(ApplicationDbContext));
        //             services.AddSingleton(context);

        //             context.Database.OpenConnection();
        //             context.Database.EnsureCreated();

        //             context.SaveChanges();

        //             // Clear local context cache
        //             foreach (var entity in context.ChangeTracker.Entries().ToList())
        //             {
        //                 entity.State = EntityState.Detached;
        //             }
        //         });

        //     _server = new TestServer(builder);

        //     _Client = _server.CreateClient();
        // }
        

    }
}
