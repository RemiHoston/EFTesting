using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Validation;

namespace PlayList.IdentityServer.Validator
{
    public class CutomerPasswordValidator : IResourceOwnerPasswordValidator
    {

        public const string GrantType = "grant_customer";
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (context.Request.Raw.Get("grant_type") == GrantType)
            {
                if (context.UserName == "customer" && context.Password == "123456")
                {
                    context.Result = new GrantValidationResult(subject: "UserInfo", authenticationMethod: "password", claims: GetUserClaims());
                }
                else
                {
                    context.Result=new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidRequest,"username or password wrong");
                    
                }
            }
        }
        public Claim[] GetUserClaims()
        {
            return new Claim[]{
                new Claim("UserName","customer"),
                new Claim("Company","中国移动"),
                new Claim("Sex","男")
            };
        }
    }
}