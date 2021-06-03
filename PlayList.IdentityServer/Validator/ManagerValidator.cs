using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace PlayList.IdentityServer.Validator
{
    public class ManagerValidator : IExtensionGrantValidator
    {
        private ITokenValidator _validator { get; set; }
        public ManagerValidator(ITokenValidator validator)
        {
            this._validator = validator;
        }
        string IExtensionGrantValidator.GrantType => "grant_managerment";

        async Task IExtensionGrantValidator.ValidateAsync(ExtensionGrantValidationContext context)
        {

            string token = context.Request.Raw.Get("token");
            if (!string.IsNullOrEmpty(token))
            {
                //首先验证Token的合法性
                var validateResult = await this._validator.ValidateAccessTokenAsync(token);
                if (validateResult.IsError)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Token is valid");
                    return;
                }
            }
        }
    }
}