using System;
using System.Collections.Generic;
using System.Text;

namespace Sphix.ViewModels.User
{
   public class SignUpStep3ViewModel
    {
        public long editId { get; set; }
        public List<SelectListItems> GroupsList { get; set; }
        public string SelectedGroupId { get; set; }
        public List<SelectListItems> Association { get; set; }
        public string SelectedAssociationId { get; set; }
        public List<SelectListItems> Type1List { get; set; }
        public string SelectedType1Id { get; set; }
        public List<SelectListItems> Type2List { get; set; }
        public string SelectedType2Id { get; set; }
        public List<SelectListItems> Interests { get; set; }

    }
}
