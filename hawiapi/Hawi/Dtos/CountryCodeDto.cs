using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hawi.Dtos
{
   
    public record   CreateCountryCodeDto
    {
        [Required(ErrorMessage = "CountryCode is required")]
        public string? Code { get; set; }

        [Required(ErrorMessage = "img is required")]
        public IFormFile Image { get; set; }
        [Required(ErrorMessage = "length is required")]
        public byte? Length { get; set; }
        [Required(ErrorMessage = "CountryArabicName is required")]
        public string? CountryArabicName { get; set; }
        [Required(ErrorMessage = "CountryArabicName is required")]
        public string? CountryEnglishName { get; set; }
        [Required(ErrorMessage = "RegionCode is required")]
        public string? RegionCode { get; set; }
    }
    public record GetCountryCodeDto
    {
        public byte CityCountryId { get; set; }

        public string? CountryArabicName { get; set; }

        public string? CountryEnglishName { get; set; }

        public string? ImageUrl { get; set; }

        public byte? Length { get; set; }

        public string? Code { get; set; }

        public string? RegionCode { get; set; }
    };



}

