using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukanHabeshaModels
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(200, ErrorMessage = "Description should be less than 200 wordds!!")]
        public string Description { get; set; }
        [Required]
        [Range(1, 10000)]
        public decimal Price { get; set; }
        [Required]
        [ValidateNever]
        public string ImageUrl { get; set; }
        public bool IsOnSale { get; set; }
        [Required]
        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [Required]
        [Display(Name = "Made In")]
        public int OriginId { get; set; }
        [ForeignKey("OriginId")]
        [ValidateNever]
        public Origin origin { get; set; }

    }
}
