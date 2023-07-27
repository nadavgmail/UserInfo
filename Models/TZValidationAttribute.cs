using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInfo.Models
{
   
    public class TZValidationAttribute : ValidationAttribute, IClientValidatable
    {
        private const int ZERO_ASCII = 48;
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            string tz = value.ToString();
            int sum = 0;
            for (int i = 0; i < tz.Length - 1; i++)
            {
                int currentdigit = tz[i] - ZERO_ASCII;
                if (i % 2 == 1)
                {
                    currentdigit = currentdigit * 2;
                    if (currentdigit > 9)
                    {
                        currentdigit -= 9;
                    }
                }
                sum += currentdigit;
            }
            sum += tz[tz.Length - 1] - ZERO_ASCII;

            return sum % 10 == 0;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = "TZ Not Valid";
            rule.ValidationType = "tzvalidation";
            yield return rule;
        }
    }
}