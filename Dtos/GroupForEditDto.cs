using System;
using System.Collections.Generic;

namespace OVD.API.Dtos
{
    public class GroupForEditDto : GroupForCreationDto
    {
        public IList<String> RemoveDawgtags { get; set; }
    }
}
