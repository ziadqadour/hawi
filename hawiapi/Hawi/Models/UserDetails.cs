using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hawi.Models
{
    public partial class UserDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? ParentMobil { get; set; }
        public string? ParentChild { get; set; }
    }
}
