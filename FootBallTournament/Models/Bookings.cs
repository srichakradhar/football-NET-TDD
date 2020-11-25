using System.ComponentModel.DataAnnotations;

namespace FootBallTournament.Models
{
    public class Bookings
    {
        [Key]
        public string Id{get;set;}
        public int quantity{get;set;}
        public string matchId{get;set;}
        public string viewerId{get;set;}

    }
}