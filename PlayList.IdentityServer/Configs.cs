using System.Collections.Generic;
using IdentityServer4.Models;

namespace PlayList.IdentityServer
{
    public class Configs
    {
        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>{
            new ApiResource("Managerment","Managerment"){Scopes={"Managerment"}},
            new ApiResource("Customer","Customer"){Scopes={"Customer"}}
        };
        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>{
            new ApiScope("Managerment","Managerment"),
            new ApiScope("Customer","Customer")
        };
        public static IEnumerable<Client> Clients => new List<Client>{
            new Client{
                ClientId="ManagermentClient",
                ClientSecrets={new Secret("Managerment".Sha256())},
                AllowedScopes={
                  "Managerment"
                },
                AllowedGrantTypes=GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                AllowAccessTokensViaBrowser=true,
                AllowOfflineAccess=true
            },
            new Client{
                 ClientId="CustomerClient",
                ClientSecrets={new Secret("Customer".Sha256())},
                ClientName="Customer Api",
                AllowedScopes={
                  "Customer"
               },
               AllowedGrantTypes=GrantTypes.ResourceOwnerPasswordAndClientCredentials,
               AllowAccessTokensViaBrowser=true
            }
        };
    }
}