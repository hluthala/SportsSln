using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SportsStore.Models{
    public static class SeedData{
        public static void EnsurePopulated(IApplicationBuilder app){
            StoredDbContext context=app.ApplicationServices
                                    .CreateScope()
                                    .ServiceProvider
                                    .GetRequiredService<StoredDbContext>();
            if(context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if(!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product{
                        Name="Kayak",Description="Bateau de peche qui est bon",Category="Bateau",Price=120
                    },
                       new Product{
                        Name="Polo",Description="Bateau de peche qui est bon",Category="Bateau",Price=120
                    },
                       new Product{
                        Name="Pipo",Description="Bateau de peche qui est bon",Category="Bateau",Price=120
                    },
                       new Product{
                        Name="Lolo",Description="Bateau de peche qui est bon",Category="Bateau",Price=120
                    },
                       new Product{
                        Name="Kolo",Description="Bateau de peche qui est bon",Category="Bateau",Price=120
                    }
                );
                context.SaveChanges();
            }
        }
    }
}