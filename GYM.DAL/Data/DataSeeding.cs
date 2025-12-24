using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GYM.DAL.Data.Contexts;
using GYM.DAL.Entities;

namespace GYM.DAL.Data
{
    public static class DataSeeding
    {
        public static bool SeedData(GYMDbContext context)
        {
            try
            {
                var CategoryExists = context.Categorys.Any();
                var planExists = context.Plans.Any();

                if (CategoryExists && planExists) return false;

                if (!planExists)
                {
                    var plans = LoadDataFromJson<Plan>("plans.json");
                    if (plans.Any())
                        context.Plans.AddRange(plans);
                }

                if (!CategoryExists)
                {
                    var categories = LoadDataFromJson<Category>("categories.json");
                    if (categories.Any())
                        context.Categorys.AddRange(categories);
                }

                return context.SaveChanges() > 0;
            }catch(Exception ex)
            {
                Console.WriteLine($"seeding failed: {ex}");
                return false;
            }

        }

        private static List<T> LoadDataFromJson<T>(string filename)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", filename);
            if(!File.Exists(filepath)) throw new FileNotFoundException();
            var Data = File.ReadAllText(filepath);
            var options = new JsonSerializerOptions(){
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<T>>(Data, options) ?? new List<T>();
        }
    }
}
