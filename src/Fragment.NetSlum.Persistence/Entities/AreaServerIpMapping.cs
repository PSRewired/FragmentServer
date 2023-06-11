using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Fragment.NetSlum.Persistence.Entities
{
    public class AreaServerIpMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required ulong DiscordUserId { get; set; }
        public required string PublicIp  { get;set; }
        public required string LocalIp { get; set; }
    }
}
