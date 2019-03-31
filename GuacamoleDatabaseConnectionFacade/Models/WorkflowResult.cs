using System.Collections.Generic;

namespace GuacamoleDatabaseConnectionFacade.Models
{
    public class WorkflowResult
    {
        public int exitCode { get; set; }
        public string supplierId { get; set; }
        public ICollection<string> messages { get; set; }
    }
}
