using System.ComponentModel.DataAnnotations;

namespace FootBallTournament.Models
{
    public class viewer
    {
         [Key]
        public string Id{get;set;}
        public string name{get;set;}
        public string password{get;set;}
    }
}