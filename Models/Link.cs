using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealerShip.Models
{
    
    [NotMapped]
    public class Link
    {
        public string Href { get; set; }
        public string Rel { get; set; } 
        
    }
}
