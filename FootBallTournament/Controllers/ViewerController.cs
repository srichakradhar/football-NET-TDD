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
    [Route("viewer")]
    public class ViewerController : ControllerBase
    {
   
         public ApplicationDbContext _context;
        public ViewerController(ApplicationDbContext context){
            _context=context;
        }


    }
}