using LimitlessCareDrPortal.Repository;
using QrCode.DB.Models;

namespace QrCode.Repository;
public interface IQRScanRepository: IGenericRepository<QRScan>
{
    Task<List<QRScan>> AllScansInTheSameQrCode(int id);
    Task<List<QRScan>> GetAll();

}
