using System.ComponentModel.DataAnnotations;

namespace OVD.API.Dtos
{
    public class GroupForCreationDto
    {
    [Required]
		public string Name { get; set; }
		[Required]
		public string Image { get; set; }
		[Required]
		public int Total { get; set; }
		[Required]
		public int Online { get; set; }
		[Required]
		public int Hotspares { get; set; }
		[Required]
		public int Active { get; set; }
    }
}