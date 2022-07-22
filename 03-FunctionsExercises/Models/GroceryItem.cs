using System;

namespace _03_FunctionsExercises.Models
{
    public class GroceryItem
    {
        public string Id { get; set; }  = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
