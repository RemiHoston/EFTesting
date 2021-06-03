using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using PlayList.Models;

namespace PlayList.Managerment.WebApi
{
    public static class ClaimExtions
    {
        public static Manager GetMangerInfoByClaims(this IEnumerable<Claim> claims)
        {
            PlayList.Models.Manager thisManager = new Manager();
            thisManager.Address = GetClaimValueByKey(claims, "Address"); 
            thisManager.Id = TryGetInt(GetClaimValueByKey(claims, "Id"));
            thisManager.RealName = GetClaimValueByKey(claims, "RealName");
            thisManager.UserName = GetClaimValueByKey(claims, "UserName");
            thisManager.CreateTime = TryGetDateTime(GetClaimValueByKey(claims, "CreateTime"));
            thisManager.CreateUser = TryGetInt(GetClaimValueByKey(claims, "CreateUser"));
            thisManager.UpdateTime = TryGetDateTime(GetClaimValueByKey(claims, "UpdateTime"));
            thisManager.UpdateUser = TryGetInt(GetClaimValueByKey(claims, "UpdateUser"));

            return thisManager;
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