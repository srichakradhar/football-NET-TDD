using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using FootBallTournament.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FootBallTournament.Controllers
{
    [Authorize]
    [ApiController]
    [Route("teams")]
    public class TeamsController : ControllerBase
    {
         public ApplicationDbContext _context;
        public TeamsController(ApplicationDbContext context){
            _context=context;
        }
        [AllowAnonymous]
        [HttpPost("registration")]
        public  IActionResult Register([FromBody] Teams user){
            if(user.name == null){
              return StatusCode(400);
            }
            else{
                _context.Teams.Add(user);
                _context.SaveChanges();
                return StatusCode(201);
            }
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public object login([FromBody]Teams userParam){
                var ApplicationUser = _context.Teams.SingleOrDefault(x => x.name == userParam.name && x.password == userParam.password);
               // Console.WriteLine("hello" +ApplicationUser);
              var response1 = new {message="Username or Password is wrong"};
                 if(ApplicationUser==null){
                  
                     return response1; }
                 var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Keepksdjhfjksdhfklsdfhsdkljsdlfnhlsd");
              JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
              
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    
                   
                    new Claim(ClaimTypes.NameIdentifier,ApplicationUser.Id)
                    
                }),
               
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            // Console.WriteLine("hello"+ClaimTypes.Role+"hello1"+ClaimTypes.Name);
            var token = tokenHandler.CreateToken(tokenDescriptor);
           var token1 = tokenHandler.WriteToken(token);
           // var ApplicationUserId = _httpContextAccessor.HttpContext.ApplicationUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var response = new  { team = ApplicationUser.name,
         
          token = token1,
          };
                  return response;
        }
        [Authorize]
        [HttpGet("view")]
        public IActionResult getTeamsDetails(){
          //  string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
              var Teams = _context.Teams.ToList();
               if(Teams!=null){
               
              
                return Ok(Teams);
            }
            else{
                return StatusCode(500);
            }
        }
        [Authorize]
        [HttpDelete("delete")]
        public IActionResult Delete(){
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
              var Teams = _context.Teams.SingleOrDefault(u => u.Id==id);
               if(Teams!=null){
               
                   _context.Remove(Teams);
                   _context.SaveChanges();
                return Ok(new {message="Team is deleted successsfully"});
            }
            else{
                return BadRequest();
            }
        }
        [Authorize]
        [HttpPut("update")]
        public IActionResult updateTeam([FromBody]Teams team){
                  if(_context.Teams.Count()!=0){
                      string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                      Console.WriteLine("hello"+id);
                        Teams teams = _context.Teams.SingleOrDefault(u => u.Id==id);
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
            [HttpGet("eleven")]  
           public Object inElevenPlayers(){
                string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                 var players = _context.Players.Where(u => u.inEleven==true && u.belongsTo==id).ToList();
                 if(players.Where(u => u.type=="Mid-Fielder").ToList().Count>=1 && players.Where(u => u.type=="Defender").ToList().Count>=1 && 
                     players.Where(u => u.type=="Forwarder").ToList().Count>=1  && players.Where(u => u.type=="Goal Keeper").ToList().Count==1 ){
                         var Response = new{message=" ",team11s=players};
                         return Response;
                     }
                     else{
                          var Response = new{message="Playing eleven does not meet the conditions ",team11s=players};
                         return Response;  
                     }
                 
           }

    }
}