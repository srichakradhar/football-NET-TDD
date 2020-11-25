using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FootBallTournament.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FootBallTournament.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
          public ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context){
            _context=context;
        }
      
          [AllowAnonymous]
        [HttpPost("register")]
        public  object Register([FromBody] Admin user){
            if(user.name == null){
              return new {status="failed"};
            }
            else{
                _context.Admin.Add(user);
                _context.SaveChanges();
                return new {status="success"};
            }
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public object login([FromBody]Admin userParam){
               // var ApplicationUser = _context.Admin.SingleOrDefault(x => x.name == userParam.name && x.password == userParam.password);
               // Console.WriteLine("hello" +ApplicationUser);
            //  var response1 = new {message="Username or Password is wrong"};
                 if(userParam.name=="Admin" && userParam.password == "Admin@1506"){
                  
                     
                 var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Keepksdjhfjksdhfklsdfhsdkljsdlfnhlsd");
              JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                   
                    new Claim(ClaimTypes.NameIdentifier,userParam.name)
                    
                }),
               
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            // Console.WriteLine("hello"+ClaimTypes.Role+"hello1"+ClaimTypes.Name);
            var token = tokenHandler.CreateToken(tokenDescriptor);
           var token1 = tokenHandler.WriteToken(token);
           // var ApplicationUserId = _httpContextAccessor.HttpContext.ApplicationUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var response = new  { admin = userParam.name,
         
          token = token1,
          };
                  return response;
        }
        else{
            var response1 = new {message="Username or Password is wrong"};
            return response1;
        }
        }
        [Authorize]
        [HttpDelete("teams/delete/{id}")]
        public IActionResult deleteteam(string id){
            if(_context.Teams.Count()!=0){
                var Teams = _context.Teams.SingleOrDefault(u => u.Id==id);
                _context.Teams.Remove(Teams);
                _context.SaveChanges();
                var response = new {message="Team is deleted successfully"};
                return Ok(response);
            }
            else{
                return BadRequest();
            }
        }
         [Authorize]
        [HttpDelete("players/delete/{id}")]
        public IActionResult deleteplayer(string id){
            if(_context.Players.Count()!=0){
                var Players = _context.Players.SingleOrDefault(u => u.Id==id);
                _context.Players.Remove(Players);
                _context.SaveChanges();
                var response = new {message="Player is deleted successfully"};
                return Ok(response);
            }
            else{
                return BadRequest();
            }
        }
        [Authorize]
        [HttpGet("teams/view")]
        public IActionResult ViewAllTeams(){
                 
                var Teams = _context.Teams.ToList();
              
                return Ok(Teams);
           
        }
        [Authorize]
        [HttpPut("teams/update/{id}")]
        public IActionResult updateTeam([FromBody]Teams team,string id){
                  if(_context.Teams.Count()!=0){
                        Teams teams = _context.Teams.SingleOrDefault(u => u.Id==id);
                        teams.name=team.name;
                        teams.country=team.country;
                        teams.coach=team.coach;
                        _context.Teams.Update(teams);
                        _context.SaveChanges();
                        return Ok(teams);
                  }
                  else{
                      return BadRequest();
                  }
        }
        [Authorize]
        [HttpGet("players/view/{id}")]
        public IActionResult playerseleven(string id){
         
             List<Players> players = _context.Players.Where(u => u.belongsTo==id).ToList();
             var response = new {message="",Teams=players};
             return Ok(players);
         
         
        }
        [Authorize]
        [HttpPut("players/update/{id}")]
        public IActionResult updatePlayer([FromBody]Players team,string id){
                  if(_context.Players.Count()!=0){
                        Players teams = _context.Players.SingleOrDefault(u => u.Id==id);
                        teams.name=team.name;
                      //  teams.belongsTo=team.belongsTo;
                        teams.goalsScored=team.goalsScored;
                        teams.age=team.age;
                        teams.type=team.type;
                        teams.noOfMatches=team.noOfMatches;
                        teams.inEleven=team.inEleven;
                        _context.Players.Update(teams);
                        _context.SaveChanges();
                        return Ok(teams);
                  }
                  else{
                      return BadRequest();
                  }
                  }
            [Authorize]
            [HttpPost("players/register/{id}")]
           public IActionResult AddPlayer([FromBody]Players player,string id){
               if(player.name==null){
                   return BadRequest();

               }else{player.belongsTo=id;
               _context.Players.Add(player);
               _context.SaveChanges();
               return StatusCode(201);
               }
           }    
           [Authorize]
           [HttpDelete("players/deleteAll/{id}")]  
           public IActionResult deletePlayers(string id){
               List<Players> players = _context.Players.Where(u => u.belongsTo==id).ToList();
               if(players.Count()>0){
                   _context.Players.RemoveRange(players);
                   _context.SaveChanges();
                   return Ok(new {message="All the players of the team are deleted successfully"});
               }
               else{
                    return BadRequest();
               }
           } 
           [Authorize]
            [HttpGet("teams/eleven/{id}")]  
           public Object inElevenPlayers(string id){
                 var players = _context.Players.Where(u => u.inEleven==true && u.belongsTo==id).ToList();
                 if(players.Where(u => u.type=="Mid-Fielder").ToList().Count>=1 && players.Where(u => u.type=="Defender").ToList().Count>=1 && 
                     players.Where(u => u.type=="Forwarder").ToList().Count>=1  && players.Where(u => u.type=="Goal Keeper").ToList().Count==1 && players.Where(u => u.inEleven==true).ToList().Count==11 ){
                         var Response = new{message=" ",team11s=players};
                         return Response;
                     }
                     else{
                          var Response = new{message="Playing eleven does not meet the conditions ",team11s=players};
                         return Response;  
                     }
                 
           }
           
            [AllowAnonymous]
            [HttpGet("database")]
           public void deleteDatabase(){
                _context.Database.EnsureDeleted();
           }
             
        



    }
}