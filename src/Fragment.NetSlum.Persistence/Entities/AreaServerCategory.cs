using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fragment.NetSlum.Persistence.Entities;

public class AreaServerCategory : IConfigurableEntity<AreaServerCategory>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ushort Id { get; set; }

    public required string CategoryName { get; set; }

    public void Configure(EntityTypeBuilder<AreaServerCategory> entityBuilder)
    {
        entityBuilder.HasData(new AreaServerCategory
        {
            Id = 1,
            CategoryName = "Main",
        });
        entityBuilder.HasData(new AreaServerCategory
        {
            Id = 2,
            CategoryName = "Test",
        });
    }
}
