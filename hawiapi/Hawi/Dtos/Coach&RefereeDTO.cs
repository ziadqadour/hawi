using System.ComponentModel.DataAnnotations;

namespace Hawi.Dtos
{
    public class AddPendingAffiliationDto
    {
       
        [Required(ErrorMessage = "requiored")]
        public byte PendingAffiliationListTypeId { get; set; }
      
        [Required(ErrorMessage = "requiored")]
        public IFormFile CertificateImageUrl { get; set; } = null!;
       
    
    }


    public class AffiliationStatusDeterminationDTO
    {
        [Required (ErrorMessage = "Required")]
        public byte? PendingAffiliationStatusId { get; set; }
        //[Required(ErrorMessage = "Required")]
        public string? PendingAffiliationStatusReason { get; set; }

    }


    public class RefereeRequestDTO
    {
        [Required(ErrorMessage = "Required")]
        public DateTime? MatchDate { get; set; }
      
        [Required(ErrorMessage = "Required")]
        public string? Place { get; set; }

        public byte? EstimatedCost { get; set; }

        public byte? NumberOfReferee { get; set; }
    
        [Required(ErrorMessage = "Required")]
        public string? Description { get; set; }

        public long MatchId { get; set; } = 0;
    }

    public class RefereeCandidateDTO
    {
        [Required(ErrorMessage = "Required")]
        public string Comment { get; set; }
    }
}
