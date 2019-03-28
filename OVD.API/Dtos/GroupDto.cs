using System.ComponentModel.DataAnnotations;

namespace OVD.API.Dtos
{
    public class GroupDto
    {
    	[Required]
		public string Name { get; set; }
		[Required]
		public string VmChoice { get; set; }
		[Required]
		public int MaxVms { get; set; }
		[Required]
		public int MinVms { get; set; }
		[Required]
		public int NumHotspares { get; set; }
		[Required]
		public string[] Dawgtags { get; set; }
    }
}