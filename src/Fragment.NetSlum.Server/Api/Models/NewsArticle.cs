using System;

namespace Fragment.NetSlum.Server.Api.Models;

public record NewsArticle(ushort Id, ushort? CategoryId, string Title, string Content, DateTime CreatedAt);
