using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukanHabeshaModels
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Range(1, 100, ErrorMessage = "Display Order should be between 1 and 100!")]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
        [DisplayName("Date Created")]
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
    }
}
