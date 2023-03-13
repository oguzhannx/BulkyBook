using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        [Range(0, 1000)]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 1000)]
        [DisplayName("Price for 1-50")]

        public double Price { get; set; }

        [Required]
        [Range(1, 1000)]
        [DisplayName("Price for 50-100")]

        public double Price50 { get; set; }

        [Required]
        [Range(1, 1000)]
        [DisplayName("Price for 100+")]
        public double Price100 { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }


        //Foregign Keys
        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [Required]
        [DisplayName("Cover Type")]
        public int CoverTypeId { get; set; }
        [ForeignKey("CoverTypeId")]
        [ValidateNever]
        public CoverType CoverType { get; set; }


    }
}
