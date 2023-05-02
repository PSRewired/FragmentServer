using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fragment.NetSlum.Persistence.Listeners;

public interface IEntityChangeListener
{
    public Task EntityChanged(DbContext context, EntityEntry entry);
}
