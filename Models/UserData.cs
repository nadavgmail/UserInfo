using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserInfo.Models
{
    public class UserData
    {
        [Required(ErrorMessage ="name is missing")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "full name should be between 5 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "TZ Card is missing")]
        [TZValidation(ErrorMessage = "TZ Not Valid")]
        public string ID { get; set; }


        [Required(ErrorMessage ="ip is missing")]
        [RegularExpression(@"\b(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b", ErrorMessage = "ip is not valid")]
        public string IP { get; set; }
        
        [Required(ErrorMessage = "phone is missing")]
        [RegularExpression(@"^[0][5][0|2|3|4|5|9]{1}[]{0,1}[0-9]{7}$", ErrorMessage = "phone number is invalid")]
        
        public string Phone { get; set; }
    }
}