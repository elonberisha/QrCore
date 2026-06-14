using Microsoft.EntityFrameworkCore;
using QrEventApi.Features.QrCodes;

namespace QrEventApi.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<QrCodeRecord> QrCodes => Set<QrCodeRecord>();
}
