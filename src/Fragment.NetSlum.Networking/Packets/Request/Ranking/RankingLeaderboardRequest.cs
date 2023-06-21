using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Ranking;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.Ranking;

[FragmentPacket(MessageType.Data, OpCodes.RankingLeaderboard)]
public class RankingLeaderboardRequest : BaseRequest
{
    private readonly FragmentContext _database;
    private readonly ILogger<RankingLeaderboardRequest> _logger;

    public RankingLeaderboardRequest(FragmentContext database, ILogger<RankingLeaderboardRequest> logger)
    {
        _database = database;
        _logger = logger;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var categoryId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        // This is intentionally the high-order bytes since class types start at a value of 1. This lets us know if the menu is on the
        // first or second page
        var classType = (byte) categoryId & 0x00FF;
        var categoryType = (byte) ((categoryId & 0xFF00) >> 8);

        IQueryable<Persistence.Entities.CharacterStats> rankingQuery = _database.CharacterStats
                .Include(cs => cs.Character)
                .Where(cs => cs.Character != null)
            ;

        _logger.LogDebug("Leaderboard request classType: {ClassType:X2} -- categoryType: {CategoryType:X2} ({FullId:X4})", classType, categoryType, categoryId);

        if (categoryType == 0)
        {
            return HandleRankCategories();
        }


        if (classType == 0xFF)
        {
            return HandleClassCategories(categoryType);
        }

        switch ((CharacterRanks.RankCategory) categoryType)
        {
            case CharacterRanks.RankCategory.Level:
                rankingQuery = rankingQuery
                    .Include(cs => cs.Character)
                    .OrderByDescending(c => c.Character!.CurrentLevel);
                break;
            case CharacterRanks.RankCategory.HP:
                rankingQuery = rankingQuery.OrderByDescending(c => c.CurrentHp);
                break;
            case CharacterRanks.RankCategory.SP:
                rankingQuery = rankingQuery.OrderByDescending(c => c.CurrentSp);
                break;
            case CharacterRanks.RankCategory.GP:
                rankingQuery = rankingQuery.OrderByDescending(c => c.CurrentGp);
                break;
            case CharacterRanks.RankCategory.OnlineTreasures:
                rankingQuery = rankingQuery.OrderByDescending(c => c.OnlineTreasures);
                break;
            case CharacterRanks.RankCategory.AverageFieldLevel:
                rankingQuery = rankingQuery.OrderByDescending(c => c.AverageFieldLevel);
                break;
            case CharacterRanks.RankCategory.GoldCoin:
                rankingQuery = rankingQuery.OrderByDescending(c => c.AverageFieldLevel);
                break;
            case CharacterRanks.RankCategory.SilverCoin:
                rankingQuery = rankingQuery.OrderByDescending(c => c.AverageFieldLevel);
                break;
            case CharacterRanks.RankCategory.BronzeCoin:
                rankingQuery = rankingQuery.OrderByDescending(c => c.AverageFieldLevel);
                break;
        }

        rankingQuery = rankingQuery
            .Where(cs => cs.Character!.Class == (CharacterClass) classType)
            .Take(100);

        var responses = new List<FragmentMessage>
        {
            new RankingLeaderboardPlayerCountResponse((ushort)rankingQuery.Count()).Build()
        };

        foreach (var rank in rankingQuery)
        {
            responses.Add(new RankingLeaderboardPlayerEntryResponse()
                .SetPlayerId((uint)rank.CharacterId)
                .SetPlayerName($"{rank.Character!.CharacterName} {rank.CharacterId}")
                .Build());
        }

        return Task.FromResult<ICollection<FragmentMessage>>(responses);
    }

    private static Task<ICollection<FragmentMessage>> HandleRankCategories()
    {
        var categoryTypes = Enum.GetValues(typeof(CharacterRanks.RankCategory));
        var responses = new List<FragmentMessage>
        {
            new RankingLeaderboardCategoryCountResponse((ushort)categoryTypes.Length).Build(),
        };

        foreach (CharacterRanks.RankCategory category in categoryTypes)
        {
            responses.Add(new RankingLeaderboardCategoryEntryResponse()
                // Shift the category ID to the upper bytes and set the "class category" to the max value to signal that no class has been selected
                .SetCategoryId((ushort)((ushort)((ushort)category << 8) | 0x00FF))
                .SetCategoryName(category.GetCategoryName())
                .Build());
        }

        return Task.FromResult<ICollection<FragmentMessage>>(responses);
    }

    private static Task<ICollection<FragmentMessage>> HandleClassCategories(byte rankId)
    {
        var categoryTypes = Enum.GetValues(typeof(CharacterClass));
        var responses = new List<FragmentMessage>
        {
            new RankingLeaderboardCategoryCountResponse((ushort)categoryTypes.Length).Build(),
        };

        foreach (CharacterClass category in categoryTypes)
        {
            responses.Add(new RankingLeaderboardCategoryEntryResponse()
                .SetCategoryId((ushort)((ushort)category | (rankId << 8)))
                .SetCategoryName(category.GetClassName())
                .Build());
        }

        return Task.FromResult<ICollection<FragmentMessage>>(responses);
    }
}
