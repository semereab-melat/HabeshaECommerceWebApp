
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
    public class ShoppingCart
    {

        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUSer { get; set; }


        /* [Required]
         public int OrderId { get; set; }
         [ForeignKey("OrderId")]
         [ValidateNever]
         public OrderHeader OrderHeader { get; set; }*/




        //NUTMAPED is a helpful that tells to our entityframework that this attribute shoul not included and mapped to our databse
        //so this attribute won't included in our database even durng migration
        [NotMapped]

        public double Price { get; set; }
    }
}
