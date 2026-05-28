using System;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;

namespace Fragment.NetSlum.Server.Stores;

public class AreaServerAssociationStore
{
    private readonly IMemoryCache _memoryCache;

    public class AssociationData
    {
        public Guid AuthUserId { get; init;}
        public bool Claimed { get; set; }

        public AssociationData(Guid authUserId)
        {
            AuthUserId = authUserId;
        }
    }

    public AreaServerAssociationStore(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public string GenerateCode(Guid authUserId)
    {
        var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString("D6");

        // Ensure there is not a collision
        while (_memoryCache.TryGetValue(code, out _))
        {
            code = RandomNumberGenerator.GetInt32(100000, 999999).ToString("D6");
        }

        _memoryCache.Set(code, new AssociationData(authUserId), DateTimeOffset.Now.AddMinutes(10));

        return code;
    }

    public bool TryClaimCode(string code, out AssociationData? associationData)
    {
        if (!_memoryCache.TryGetValue(code, out associationData))
        {
            return false;
        }

        // Association data is a reference type, just update the claimed reference and it will update the memory cache object.
        associationData!.Claimed = true;

        // We don't want to remove the code so the api can pull it for a few moments and display claim status

        return true;
    }

    public bool GetClaimStatus(string code)
    {
        return _memoryCache.TryGetValue(code, out AssociationData? associationData) && associationData!.Claimed;
    }
}
