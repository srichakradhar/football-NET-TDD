using System.ComponentModel.DataAnnotations;

namespace FootBallTournament.Models
{
    public class Mappings
    {
       
        [Key]
        public string Id{get;set;}
        public string category{get;set;}
        public string name{get;set;}

        public Mappings(){
            
        }
        public Mappings(string v1, string v2)
        {
            category = v1;
            name = v2;
        }

        

        
    }
}