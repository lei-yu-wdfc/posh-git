using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Mappings.Sections
{
    /// <summary>
    /// Your Details Section
    /// </summary>
    public class YourDetailsSection
    {
        public String Fieldset { get; set; }
        public String IdNumber { get; set; }
        public String Dependants { get; set; }
        public String Gender { get; set; }
        public String DateOfBirthDay { get; set; }
        public String DateOfBirthMonth { get; set; }
        public String DateOfBirthYear { get; set; }
        public String HomeStatus { get; set; }
        public String MaritalStatus { get; set; }
        public String HomeLanguage { get; set; }
        public String Warning { get; set; }
        public String IdNumberWarningForm { get; set; }
        public String DependantsErrorForm { get; set; }
        public String MartialStatusErrorForm { get; set; }
        public String PeselNumber { get; set;}
        public String PeselWarningForm { get; set; }
        public String MotherMaidenName { get; set; }
        public String MotherMaidenNameWarningForm { get; set; }
        public String EducationLevel { get; set; }
        public String EducationLevelErrorForm { get; set; }
        public String VehicleOwner { get; set;}
        public String VehicleOwnerErrorForm { get; set; }
        public String AllegroLogin { get; set; }
        public String AllegroLoginErrorForm { get; set; }
    }
}
