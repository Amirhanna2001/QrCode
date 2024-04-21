using LimitlessCareDrPortal.Repository;
using QrCode.DB.Models;
using LimitlessCareDrPortal;
using QrCode.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace QrCode.Repository;
public class QRCodeRepository : GenericRepository<QRCode>, IQRCodeRepository
{
    private readonly DatabaseContext context;

    public QRCodeRepository(DatabaseContext context) : base(context)
    {
        this.context = context;
    }
    public async Task<IEnumerable<QRCode>> GetAll()
    {
        return await context.QRCodes.Where(q => !q.IsDeleted).ToListAsync();
             
    }
    public void Remove(QRCode entity)
    {
        entity.IsDeleted = true;
        Update(entity);
    }
    public Task<QRCode> FindByHash(string hash)
    {
        return context.QRCodes.FirstOrDefaultAsync(q=>q.HashValue == hash);
    }

}
