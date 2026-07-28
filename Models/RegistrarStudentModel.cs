using System;
using System.Collections.Generic;

namespace Regis.Models
{
    // ============================================================
    // REGISTRAR STUDENT MODEL
    // Backs the RegistrarStudentList page (Add/Edit form + grid).
    // One row = one student record maintained by the Registrar,
    // with a document checklist (Required vs Submitted) captured
    // via the "Proceed to Documents" popup.
    // ============================================================
    public class RegistrarStudentModel
    {
        public int RegistrarId { get; set; }

        public string StudentName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }

        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public int? BranchId { get; set; }
        public string BranchName { get; set; }

        public int? SemesterId { get; set; }
        public string SemesterName { get; set; }

        // Documents that ARE REQUIRED for the selected Course
        // (pulled from Required Document Master, shown as checklist in popup)
        public string RequiredDocumentIdsCsv { get; set; }
        public string RequiredDocumentNames { get; set; }
        public int RequiredDocumentCount { get; set; }

        // Documents the student HAS ACTUALLY SUBMITTED
        // (the checkboxes the Registrar ticks in the popup)
        public string SubmittedDocumentIdsCsv { get; set; }
        public string SubmittedDocumentNames { get; set; }
        public int SubmittedDocumentCount { get; set; }

        // "Yes" (all required docs received) / "Partial" / "No" — used for the grid badge
        public string DocumentStatus { get; set; }

        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        // Not persisted directly — used only to carry the popup's checked
        // ids in and out of the controller action (bound from the hidden
        // "SelectedDocumentIds" form field).
        public List<int> DocumentEnclosureIds { get; set; }

        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}