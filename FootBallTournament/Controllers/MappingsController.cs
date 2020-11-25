using System.Linq;
using FootBallTournament.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootBallTournament.Controllers
{
    [Authorize]
    [ApiController]
    [Route("mapping")]
    public class MappingsController : ControllerBase
    {
            public ApplicationDbContext _context;
        public MappingsController(ApplicationDbContext context){
            _context=context;
        }
        [Authorize]
        [HttpGet("view")]
        public IActionResult getAllMAppings(){
            var mappings = _context.Mappings.ToList();
            return Ok(mappings);
        }
        [Authorize]
        [HttpPut("update/{id}")]
        public IActionResult updateTeam([FromBody]Mappings mappings,string id){
                  if(_context.Mappings.Count()!=0){
                     // string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                     // Console.WriteLine("hello"+id);
                        Mappings teams = _context.Mappings.SingleOrDefault(u => u.Id==id);
                        teams.name=mappings.name;
                      
                        _context.Mappings.Update(teams);
                        _context.SaveChanges();
                        return Ok(teams);
                  }
                  else{
                      return BadRequest();
                  }
        }
    }
}