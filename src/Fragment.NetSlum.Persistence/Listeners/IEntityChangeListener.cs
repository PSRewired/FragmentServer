using System.Threading.Tasks;
using Fragment.NetSlum.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Listeners;

public interface IEntityChangedListener
{
    public Task EntityChanging(DbContext context, EntityChangeSnapshot snapshot);
    public Task EntityChanged(DbContext context, EntityChangeSnapshot snapshot);
}
