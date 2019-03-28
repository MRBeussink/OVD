using System.ComponentModel.DataAnnotations;

namespace OVD.API.Dtos
{
    public class GroupDto
    {
    	[Required]
		public string Name { get; set; }
		[Required]
		public string[] Dawgtags { get; set; }
    }
}