using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DukanHabeshaModels;
public class Origin
{
    [Key]
    public int Id { get; set; }
    [Required]
    [DisplayName("Made In")]
    [MaxLength(20)]
    public string OriginCountry { get; set; }
    
   
}

