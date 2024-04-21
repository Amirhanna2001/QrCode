using LimitlessCareDrPortal.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using QrCode.API.DTOs;
using QrCode.DB.Models;
using QrCode.Repository;

namespace QrCode.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ScanController : ControllerBase
{
    private readonly IQRScanRepository scanRepository;
    private readonly IQRCodeRepository qRCodeRepository;
    private readonly IUnitOfWork unitOfWork;

    public ScanController(IQRScanRepository scanRepository,
                           IUnitOfWork unitOfWork,
                           IQRCodeRepository qRCodeRepository)
    {
        this.scanRepository   = scanRepository;
        this.unitOfWork       = unitOfWork;
        this.qRCodeRepository = qRCodeRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Scan(ScanDTO dto)
    {
        DeviceType device = GetDeviceName(dto);

        QRCode qR = await qRCodeRepository.FindByHash(dto.HashValue);

        if (qR == null || (qR != null && qR.IsDeleted))
            return BadRequest("This QrCode is not exists.");

        var browser = dto.Browser.Split('/');
        QRScan qRScan = new()
        {
            DeviceType = device,
            Country = dto.Country,
            Browser = browser[0],
            QRCodeID = qR.ID,
        };

        await scanRepository.Add(qRScan);
        await unitOfWork.SaveChanges();

        return Ok(qR.URLValue);
    }

    private static DeviceType GetDeviceName(ScanDTO dto)
    {
        dto.DeviceType = dto.DeviceType.ToUpper();
        DeviceType device = dto.DeviceType == DeviceType.ANDRIOD.ToString() ? DeviceType.ANDRIOD
            : dto.DeviceType == DeviceType.IPHONE.ToString() ? DeviceType.IPHONE
            : dto.DeviceType == DeviceType.IPAD.ToString() ? DeviceType.IPAD : DeviceType.PC;
        return device;
    }
}
