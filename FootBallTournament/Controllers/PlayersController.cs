using System.Linq;
using System.Security.Claims;
using FootBallTournament.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootBallTournament.Controllers
{
    [Authorize]
    [ApiController]
    [Route("players")]
    public class PlayersController : ControllerBase
    {
          public ApplicationDbContext _context;
        public PlayersController(ApplicationDbContext context){
            _context=context;
        }
          [Authorize]
        [HttpPost("register")]
           public IActionResult AddPlayer([FromBody]Players player){
                string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
               if(player.name==null){
                   return BadRequest();

               }
               else{player.belongsTo=id;
               _context.Players.Add(player);
               _context.SaveChanges();
               return StatusCode(201);
               }
           }
             [Authorize]
        [HttpPut("update/{id}")]
        public IActionResult updatePlayer([FromBody]Players team,string id){
                  if(_context.Players.Count()!=0){
                        Players teams = _context.Players.SingleOrDefault(u => u.Id==id);
                        teams.age=team.age;
                        teams.name=team.name;
                        
                        teams.goalsScored=team.goalsScored;
                        teams.noOfMatches=team.noOfMatches;
                        teams.type=team.type;
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
        [HttpDelete("delete/{id}")]
        public IActionResult deletePlayer(string id){
                  if(_context.Players.Count()!=0){
                        Players teams = _context.Players.SingleOrDefault(u => u.Id==id);
                       
                        _context.Players.Remove(teams);
                        _context.SaveChanges();
                        var response = new {message="Player deleted successfully"};
                        return Ok(response);
                  }
                  else{
                      return BadRequest();
                  }
                  }   
                  [Authorize]
        [HttpGet("view")]
        public IActionResult playerseleven(){
         string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
             var players = _context.Players.Where(u => u.belongsTo==id).ToList();
             var response = new {message="",Teams=players};
             return Ok(players);
         
        }

          [Authorize]
           [HttpDelete("deleteAll")]  
           public IActionResult deletePlayers(){
                 string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
               var players = _context.Players.Where(u => u.belongsTo==id).ToList();
               if(players.Count()>0){
                   _context.Players.RemoveRange(players);
                   _context.SaveChanges();
                   return Ok(new {message="All the players of the team are deleted successfully"});
               }
               else{
                    return BadRequest();
               }
           } 
    }
}