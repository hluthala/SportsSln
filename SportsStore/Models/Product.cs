using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models{
    public class Product{
        public long ProductID { get; set; }
        [Required(ErrorMessage ="Please enter product name")]
        public string Name { get; set; }
         [Required(ErrorMessage ="Please enter product description")]
        public string  Description { get; set; }
        [Required]
        [Range(0.01,double.MaxValue,ErrorMessage ="Please enter positive price")]
        [Column(TypeName="decimal(8,2)")]
        public decimal Price{get;set;}
        [Required(ErrorMessage ="Please specify a ccategory")]
        public string Category { get; set; }
    }
}