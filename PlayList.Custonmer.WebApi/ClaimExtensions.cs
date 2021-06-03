using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using PlayList.Models;

namespace PlayList.Custonmer.WebApi
{
     public static class ClaimExtions
    {
        public static Customer GetMangerInfoByClaims(this IEnumerable<Claim> claims)
        {
            PlayList.Models.Customer thisCustomer = new Customer();
            thisCustomer.NickName = GetClaimValueByKey(claims, "NickName"); 
            thisCustomer.Id = TryGetInt(GetClaimValueByKey(claims, "Id"));
            thisCustomer.RealName = GetClaimValueByKey(claims, "RealName");
            thisCustomer.PhoneNumber = GetClaimValueByKey(claims, "PhoneNumber");
            
            return thisCustomer;
        }
        private static string GetClaimValueByKey(IEnumerable<Claim> claims, string type)
        {
            var claim = claims.Where(c => c.Type == type).FirstOrDefault();
            if (claim.IsNotNullOrEmpty())
            {
                return claim.Value;
            }
            else
            {
                return string.Empty;
            }
        }
        private static int TryGetInt(string value)
        {
            int returnValue;
            int.TryParse(value,out returnValue);
            return returnValue;
        }
        private static DateTime TryGetDateTime(string value)
        {
            DateTime returnValue;
            if(DateTime.TryParse(value,out returnValue))
            {
                return returnValue;
            }
            else
            {
                return DateTime.MinValue;
            }
        }
    }
}