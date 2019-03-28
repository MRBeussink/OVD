using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OVD.API.Dtos
{
    public class GroupForCreationDto
    {
        [Required]
        public String Name { get; set; }
        [Required]
        public String VMChoice { get; set; }
        [Required]
        public int MaxVms { get; set; }
        [Required]
        public int MinVms { get; set; }
        [Required]
        public int NumHotspares { get; set; }
        [Required]
        public IList<String> Dawgtags { get; set; }
    }
}