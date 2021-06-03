using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace PlayList.IdentityServer.Validator
{
    public class CustomerValidator : IExtensionGrantValidator
    {
        private ITokenValidator _validator { get; set; }
        string IExtensionGrantValidator.GrantType => "grant_customer";
        public CustomerValidator(ITokenValidator validator)
        {
            this._validator = validator;
        }

        async Task IExtensionGrantValidator.ValidateAsync(ExtensionGrantValidationContext context)
        {
            string token = context.Request.Raw.Get("token");
            if (!string.IsNullOrEmpty(token))
            {
                var result = await this._validator.ValidateAccessTokenAsync(token);

                if (result.IsError)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Token is valid");
                }
            }
        }
    }
}