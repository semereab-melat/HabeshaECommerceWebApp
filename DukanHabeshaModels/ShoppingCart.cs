using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DukanHabeshaModels
{
    public class ShoppingCart
    {
        /* public IEnumerable<ShoppingCart> ListCart { get; set; }
         public double CartTotal { get; set; }*/

        public Product Product { get; set; }
        [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; }
    }
}
