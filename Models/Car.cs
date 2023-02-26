using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DealerShip.Models
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        public string Brand { get; set; } = string.Empty;
        [Required]
        public string Model { get; set; } = string.Empty;
        [Required]
        public int Year { get; set; } = 0;
        [Required]
        public string Color { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; } = 0;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Description { get; set; } = null;
        [Required]
        public bool IsSold { get; set; } = false;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? AcquisitionDate { get; set; } = null;
        [JsonIgnore]
        public int? CustomerId { get; set; } = null;
        [JsonIgnore(Condition =JsonIgnoreCondition.WhenWritingDefault)]
        public List<Link>? Links { get; set; } = new List<Link>();




    }
}
