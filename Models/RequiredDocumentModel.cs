using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace Regis.Models
{
    public class RequiredDocumentModel
    {
   
            public int RequiredDocumentId { get; set; }

            public int AcademicSessionId { get; set; }
            public string SessionName { get; set; }

            public int AdmissionModeId { get; set; }
            public string AdmissionModeName { get; set; }

            public int ProgramId { get; set; }
            public string ProgramName { get; set; }

            public int CourseId { get; set; }
            public string CourseName { get; set; }

            public int CategoryId { get; set; }
            public string CategoryName { get; set; }

            public bool IsActive { get; set; }
            public DateTime? CreatedDate { get; set; }

            // Multi-select mapping (Document Enclosure Master)
            public List<int> DocumentEnclosureIds { get; set; } = new List<int>();
            public string RequiredDocumentNames { get; set; }   // comma-joined, list page ke liye
            public string DocumentEnclosureIdsCsv { get; set; } // comma-joined ids, INSERT/UPDATE ke liye
    }
}