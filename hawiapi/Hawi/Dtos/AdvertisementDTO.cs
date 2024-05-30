using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hawi.Dtos
{
    public class AddAdvertisementDTO
    {
        [Required(ErrorMessage = ("*"))]
        public string? TargetUserPhone{ get; set; }
        [Required(ErrorMessage = ("*"))]
        public byte TargetUserRoleId { get; set; }
        public string? AdvertisementTitle { get; set; }
        public string? AdvertisementText { get; set; }
        [Required(ErrorMessage = ("*"))]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = ("*"))]
        public DateTime? EndDate { get; set; }
        public IFormFile? AdvertisementImage { get; set; }
        public IFormFile? TargetUserLogo { get; set; }
        public string? TargetSite { get; set; }
        public List< byte>? AgeRangeId { get; set; }
        public List<long>? CityId { get; set; }
        [Required(ErrorMessage = ("*"))]
        public bool? IsMale { get; set; }
        [Required(ErrorMessage = ("*"))]
        public bool? IsActive { get; set; }

    }
    public class UpdateAdvertisementDTO
    {
        public string? AdvertisementTitle { get; set; }
        public string? AdvertisementText { get; set; }
        [Required(ErrorMessage = ("*"))]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = ("*"))]
        public DateTime? EndDate { get; set; }
        public IFormFile? AdvertisementImage { get; set; }
        public IFormFile? TargetUserLogo { get; set; }
        public string? TargetSite { get; set; }

        public List<byte> AgeRangeId { get; set; }
        public List<long> CityId { get; set; }
        [Required(ErrorMessage = ("*"))]
        public bool? IsMale { get; set; }
        [Required(ErrorMessage = ("*"))]

        public bool? IsActive { get; set; }
    }
    
}
