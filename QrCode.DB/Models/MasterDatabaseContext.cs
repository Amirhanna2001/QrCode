using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using QrCode.DB;

#nullable disable

namespace QrCode.DB.Models;

public partial  class MasterDatabaseContext : IdentityDbContext<AppUser>
{
    public MasterDatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
    public DbSet<QRCode> QRCodes { get; set; }
    public DbSet<QRScan> QRScans { get; set; }
    

}
