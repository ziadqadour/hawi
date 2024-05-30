using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hawi.Dtos
{ 
    public class AddEventDto
    {
        [Required(ErrorMessage = "EventTitle is Required")]
        public string? EventTitle { get; set; }
        [Required(ErrorMessage = "EventText is Required")]
        public string? EventText { get; set; }
        [Required(ErrorMessage = "StratDate is Required")]
        public DateTime? StratDate { get; set; }
        [Required(ErrorMessage = "FinishDate is Required")]
        public DateTime? FinishDate { get; set; }
        [Required(ErrorMessage = "EventPlaceLocation is Required")]
        public string? EventPlaceLocation { get; set; }
       
        public bool? IsActive { get; set; }=true;
        public List<IFormFile>? Images { get; set; }
        public string? VideoUrl { get; set; }

    }
   
}
