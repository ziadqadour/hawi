using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hawi.Dtos
{
    public class ShowSportInstitutionsTypeDto
    {
        public byte SportInstitutionTypeId { get; set; }
        public string? SportInstitutionType { get; set; }
        public string? Imagepath { get; set; }
    }
 
    public record GetCitydistrictDto
    {
        public long CityDistrictsId { get; set; }
        public long CityId { get; set; }
        public string? DistractArabicName { get; set; }
        public string? DistractEnglishName { get; set; }

    }
    public record GetCityDto 
    {
       public long CityId { get; set; }
       public byte? CityCountryId { get; set; }
        public string? CityArabicName { get; set; }
        public string? CityEnglishName { get; set; }
        public string? CityAdressLatitude { get; set; }
        public string? CityAdressLongitude{ get; set; }
       
    }

    public class SportInstitutionEmployeeJobTypeDto
    {
        public byte JobTypeId { get; set; }

        public string? JobType { get; set; }
    }
    public class SportInstitutionBranchTypeDto
    {
        public byte SportInstitutionBranchTypeId { get; set; }

        public string? SportInstitutionBranchType { get; set; }
    }

    public class SportInstitutionAgePriceSubscriptionPeriodDTO
    {
        public byte SubscriptionPeriodId { get; set; }

        public string? SubscriptionPeriod { get; set; }
    }

    public class GETSportInstitutionAgePriceAgeCategoryDto
    {
        public byte AgeCategoryId { get; set; }
        public string? AgeCategory { get; set; }
    }

    //////
    
    public class CreatesportInstitutionsDto
    {

        [Required(ErrorMessage = "!يجب ادخال الاسم ")]
        public string? SportInstitutionName { get; init; }

        [Required(ErrorMessage = "!يجب اضافة اسم المالك للمنظمة الرياضية")]
        public string? FounderName { get; init; }

        [Required(ErrorMessage = "!يجب اضافة تاريخ الانشاء ")]
        public DateTime? DateCreated { get; init; }

        public string? Description { get; init; }

        [Required(ErrorMessage = "!يجب اضافة شعار")]
        public IFormFile? Logo { get; set; }

        [Required(ErrorMessage = "!يجب اضافة بريد الكترونى")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "!بريد الكترونى غير صالح")]
        public string? Gmail { get; set; }

    };

    public class UpdatesportInstitutionsDto
    {

        [Required(ErrorMessage = "!يجب ادخال هذا الحقل")]
        public string? SportInstitutionName { get; init; }

        //[Required(ErrorMessage = "LicenseNumber is required")]
        public string? LicenseNumber { get; set; }

        [Required(ErrorMessage = "!يجب ادخال هذا الحقل")]
        public string? FounderName { get; init; }

        //[Required(ErrorMessage = "DateCreated is required")]
        public DateTime? DateCreated { get; init; }

        public string? Description { get; init; }

        [Required(ErrorMessage = "!يجب ادخال هذا الحقل")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "!بريد الكترونى غير صالح")]
        public string? Gmail { get; set; }

        //branchedata
        public DateTime? MainBranchDateCreated { get; set; }

        public string MainBranchCityDistricts { get; set; }
        
        public long? MainBranchCCityId { get; set; }

        public string? MainBranchLocation { get; set; }

       
        public string? MainBranchPhone { get; set; }

        //public IFormFile? Logo { get; set; }

        // public string? oldSportinstitutionLogo { get; set; }

    };

    public class GetsportInstitutionByIdDto
    {

        public long UserProfileId { get; set; }
        public string? SportInstitutionName { get; init; }
        public string? LicenseNumber { get; set; }
        public string? FounderName { get; init; }
        public DateTime? DateCreated { get; init; }
        public string? LogoUrlfullPath { get; set; }
        public string? Description { get; init; }

    };

    public class SportInstitutionBranchDTO
    {
        [Required(ErrorMessage = "!يجب ادخال هذا الحقل")]
        public long SportInstitutionId { get; set; }

        [Required(ErrorMessage = "!يجب ادخال هذا الحقل")]
        public byte SportInstitutionBranchTypeId { get; set; }

        //[Required(ErrorMessage = "DateCreated is required")]
        public DateTime? DateCreated { get; set; }

        [Required(ErrorMessage = "!يجب ادخال هذا الحقل")]
        public long? CityId { get; set; }
      
        public string? CityDistricts { get; set; }
        public string? SportInstitutionBranchName { get; set; }

        [Required(ErrorMessage = "!يجب ادخال هذا الحقل")]
        public string? Location { get; set; }
        public string? BranchPhone { get; set; }
    }

    public class UpdateSportInstitutionBranchDTO
    {  
        public DateTime? DateCreated { get; set; }

        public string CityDistricts { get; set; }
        public string SportInstitutionBranchName { get; set; }

        [Required(ErrorMessage = "!يجب ادخال هذا الحقل")]
        public long? CityId { get; set; }

        [Required(ErrorMessage = "!يجب ادخال هذا الحقل")]
        public string? Location { get; set; }

        public string? BranchPhone { get; set; }
    }

    public class SportInstitutionAgecategoryDto
    {
        [Required(ErrorMessage = "!هذا الحقل مطلوب")]
        public long UserProfileId { get; set; }

        [Required(ErrorMessage = "!هذا الحقل مطلوب")]
        public long SportInstitutionBranchId { get; set; }

        [Required(ErrorMessage = "!هذا الحقل مطلوب")]
        public List<byte?> AgeCategoryId { get; set; }

        [Required(ErrorMessage = "!هذا الحقل مطلوب")]
        public byte? SportInstitutionTypeID { get; set; }
    }
    
    public class SportInstitutionPriceDto
    {
        [Required(ErrorMessage = "!هذا الحقل مطلوب")]
        public long UserProfileId { get; set; }
       
        [Required(ErrorMessage = "!هذا الحقل مطلوب")]
        public long SportInstitutionBranchId { get; set; }

        [Required(ErrorMessage = "!هذا الحقل مطلوب")]
        public double  price { get; set; }
    }
 
    public class UpdateSportInstitutionAgePriceDto
    {

        [Required(ErrorMessage = "!هذا الحقل مطلوب")]
        public List<byte>? SubscriptionPeriodId { get; set; }

        [Required(ErrorMessage = "!هذا الحقل مطلوب")]
        public byte? SportInstitutionTypeID { get; set; }

        [Required(ErrorMessage = "!هذا الحقل مطلوب")]
        public List<double>? Price { get; set; }
    }
   
    //employee
    public class UpdateSportInstitutionBelongDto
    {
        [Required (ErrorMessage = "يجب ادخال الفرع المنتمى الية هذا الشخص!")]
        public long SportInstitutionBranchId { get; set; }
        
        [Required(ErrorMessage = "يجب ادخال وصف لهذا الشخص!")]
        public string? ShortDescription { get; set; }

        [Required(ErrorMessage = "يجب ادخال نوع عمل هذا الشخص!")]
        public byte? BelongTypeId { get; set; }

    }

    //sponser
    public class CreateSportInstitutionSponserDto
    {
        [Required(ErrorMessage ="!هذاالحقل مطلوب")]
        public long UserProfileId { get; set; }

        public string? SponsorName { get; set; }

        [Required(ErrorMessage = "!هذاالحقل مطلوب")]
        public IFormFile? SponsorLogo { get; set; }

        public byte? SponsorTypeId { get; set; }

        public long? SponsorUserProfileId { get; set; }
    }

}

