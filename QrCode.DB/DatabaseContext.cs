using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QrCode.DB.Models;

namespace QrCode.DB;

public class DatabaseContext : MasterDatabaseContext
{
	public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
	}
}
