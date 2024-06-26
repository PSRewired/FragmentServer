﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities.Deprecated;

[Table("bbs_category")]
[Index("CategoryId", Name = "BBS_Category_categoryID_uindex", IsUnique = true)]
[MySqlCharSet("cp932")]
[MySqlCollation("cp932_japanese_ci")]
public class BbsCategory
{
    [Key]
    [Column("categoryID")]
    public int CategoryId { get; set; }

    [Column("categoryName")]
    [StringLength(33)]
    [MySqlCharSet("utf8mb4")]
    [MySqlCollation("utf8mb4_0900_ai_ci")]
    public string CategoryName { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<BbsThread> BbsThreads { get; set; } = new List<BbsThread>();
}
