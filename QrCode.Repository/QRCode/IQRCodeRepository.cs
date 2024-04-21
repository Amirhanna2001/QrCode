using LimitlessCareDrPortal.Repository;
using QrCode.DB.Models;

namespace QrCode.Repository;
public interface IQRCodeRepository:IGenericRepository<QRCode>
{
    Task<QRCode> FindByHash(string hash);
    Task<IEnumerable<QRCode>> GetAll();
}
