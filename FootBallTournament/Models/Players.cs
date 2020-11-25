using System.ComponentModel.DataAnnotations;

namespace FootBallTournament.Models
{
    public class Players
    {
            [Key]
        public string Id{get;set;}
            public string name{get;set;}
            public string type{get;set;}
            public int noOfMatches{get;set;}
             public int goalsScored{get;set;}
             public string belongsTo{get;set;}
             public bool inEleven{get;set;}

             public int age{get;set;}




    }
}