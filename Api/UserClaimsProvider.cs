﻿using System.Security.Claims;
using TourmalineCore.AspNetCore.JwtAuthentication.Core.Contract;

namespace Api;

public class UserClaimsProvider : IUserClaimsProvider
{
    public const string PermissionClaimType = "permissions";

    public const string AUTO_TESTS_ONLY_IsOneOnOnesHardDeleteAllowed = "AUTO_TESTS_ONLY_IsOneOnOnesHardDeleteAllowed";
    public const string CanManageOneOnOnes = "CanManageOneOnOnes";
    public const string CanViewOneOnOnes = "CanViewOneOnOnes";

    public Task<List<Claim>> GetUserClaimsAsync(string login)
    {
        throw new NotImplementedException();
    }
}
