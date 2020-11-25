using System.ComponentModel.DataAnnotations;

namespace FootBallTournament.Models
{
    public class Teams
    {
        [Key]
        public string Id{get;set;}
        public string name{get;set;}
        public string password{get;set;}
        public string country{get;set;}
        public string coach{get;set;}

    }
}