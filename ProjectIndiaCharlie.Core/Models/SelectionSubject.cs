using System.Collections.Generic;

namespace ProjectIndiaCharlie.Core.Models
{
    public class SelectionSubject
    {
        public VSubjectSectionDetail SelectedSubject { get; set; }
        public IEnumerable<VSubjectSectionDetail> SelectedSubjectSections { get; set; }

        public SelectionSubject(VSubjectSectionDetail selectedSubject, IEnumerable<VSubjectSectionDetail> selectedSubjectSections)
        {
            SelectedSubject = selectedSubject;
            SelectedSubjectSections = selectedSubjectSections;
        }
    }
}
