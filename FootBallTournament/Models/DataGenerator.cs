using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FootBallTournament.Models
{
   public class DataGenerator
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            // Look for any board games.
            if (context.Mappings.Any())
            {
                return;   // Data was already seeded
            }

            Mappings m = new Mappings("Semi-Final 1","");
            context.Add(m);
             Mappings m1 = new Mappings("Semi-Final 2","");
            context.Add(m1);
             Mappings m2 = new Mappings("Final","");
            context.Add(m2);
             Mappings m3 = new Mappings("Team 1","");
            context.Add(m3);
             Mappings m4 = new Mappings("Team 2","");
            context.Add(m4);
             Mappings m5 = new Mappings("Team 3","");
            context.Add(m5);
             Mappings m6 = new Mappings("Team 4","");
            context.Add(m6);
            context.SaveChanges();  
        }
    }
}

}
// _id: mappingsOneID,
//   category: "Semi-Final 1",
//   name: "",
// };

// const mappingsTwo = {
//   _id: new mongoose.Types.ObjectId(),
//   category: "Semi-Final 2",
//   name: "",
// };

// const mappingsThree = {
//   _id: new mongoose.Types.ObjectId(),
//   category: "Final",
//   name: "",
// };

// const mappingsFour = {
//   _id: new mongoose.Types.ObjectId(),
//   category: "Team 1",
//   name: "",
// };

// const mappingsFive = {
//   _id: new mongoose.Types.ObjectId(),
//   category: "Team 2",
//   name: "",
// };

// const mappingsSix = {
//   _id: new mongoose.Types.ObjectId(),
//   category: "Team 3",
//   name: "",