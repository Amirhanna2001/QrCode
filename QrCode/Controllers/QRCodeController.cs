using LimitlessCareDrPortal.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QrCode.API.AzureBlobServices;
using QrCode.API.DTOs;
using QrCode.API.Services;
using QrCode.DB.Models;
using QrCode.Repository;
using QRCoder;

namespace QrCode.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class QRCodeController : ControllerBase
{
    private readonly IQRCodeRepository qrCodeRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IQRScanRepository scanRepository;
    private readonly IAzureBlobService azureServices;
    private readonly IConfiguration configuration;
    private readonly IQrCodeServices qrCodeServices;
    private readonly UserManager<AppUser> userManager;

    public QRCodeController(IQRCodeRepository qrCodeRepository,
                            IUnitOfWork unitOfWork,
                            IQRScanRepository scanRepository,
                            IAzureBlobService azureServices,
                            IConfiguration configuration,
                            IQrCodeServices qrCodeServices,
                            UserManager<AppUser> userManager)
    {
        this.qrCodeRepository = qrCodeRepository;
        this.unitOfWork = unitOfWork;
        this.scanRepository = scanRepository;
        this.azureServices = azureServices;
        this.configuration = configuration;
        this.qrCodeServices = qrCodeServices;
        this.userManager = userManager;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<QRCode> qRCodes = await qrCodeRepository.GetAll();
        List<QrCodeBaseDTO> dto = new(); 

        foreach(QRCode qRCode in qRCodes)
        {
            
            QrCodeBaseDTO qrCodeBase = new()
            {
                ID = qRCode.ID,
                QRName = qRCode.QRName,
                URLValue = qRCode.URLValue,
                HashValue = qRCode.HashValue,
                QRPath = qRCode.QRPath,
            };
            dto.Add(qrCodeBase);
        }
        return Ok(dto);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQrCode(int id)
    {

        QRCode qrcode = await qrCodeRepository.FindById(id);

        if (qrcode == null || (qrcode != null && qrcode.IsDeleted) )
            return NotFound("No QrCode with this ID");

        List<QRScan> scansOfThisQrCode = await scanRepository.AllScansInTheSameQrCode(id);
        Dictionary<string, int> countriesOfScans = qrCodeServices.CountriesOfScan(scansOfThisQrCode);
        Dictionary<DeviceType, int> devicesOfScans =qrCodeServices.DevicesOfScans(scansOfThisQrCode);
        Dictionary<string, int> browserOfScans =qrCodeServices.BrowserOfScans(scansOfThisQrCode);

        QrDetailsDTO qrDetails = new()
        {
            QRName = qrcode.QRName,
            URLValue = qrcode.URLValue,
            qrPath = qrcode.QRPath,
            NoOfScans = scansOfThisQrCode.Count,
            DeviceScans = devicesOfScans,
            CountryScans = countriesOfScans,
            BrowserScans = browserOfScans
        };
        return Ok(qrDetails);
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateQRCodeDTO dto)
    {
        AppUser user = await userManager.FindByIdAsync(dto.UserId);
        if (user == null)
            return BadRequest("Invalid User Id");

        if (dto.QRName.IsNullOrEmpty())
            return BadRequest("Invalid Qr Code Name");

        if (dto.URLValue.IsNullOrEmpty())
            return BadRequest("Invalid URL");

        //TODO :Use Auto Mapper Q:Where 
        string hash = Guid.NewGuid().ToString();
        QRCode qRCode = new()
        {
            QRName = dto.QRName,
            URLValue = dto.URLValue,
            HashValue = hash,
            UserId = user.Id,
        };
        qRCode.QRPath = await GenerateQRCode(qRCode);

        await qrCodeRepository.Add(qRCode);
        await unitOfWork.SaveChanges();
        return Ok("Created");
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Edit(int id, [FromBody]EditQrCodeDTO dto)
    {
        
        QRCode qRCode = await qrCodeRepository.FindById(id);
        if (qRCode == null || (qRCode != null && qRCode.IsDeleted))
            return BadRequest($"No QRCode found by this id {id}");

        if (dto.QRName.IsNullOrEmpty())
            return BadRequest("Invalid Qr Code Name");

        if (!IsAuthorized(dto.UserId, qRCode).Result)
            return Unauthorized($"You are not allowed to edit {qRCode.QRName} QrCode");

        qRCode.QRName = dto.QRName;

        qrCodeRepository.Update(qRCode);
        await unitOfWork.SaveChanges();
        return Ok("Updated");
    }
    [HttpDelete("{id}/{userId}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id,string userId)
    {
        QRCode code = await qrCodeRepository.FindById(id);
        if (code == null || (code != null && code.IsDeleted)) 
            return BadRequest("No QrCode Found By This ID");

        if (userId.IsNullOrEmpty())
            return BadRequest("User Id Is Required");

        if (!IsAuthorized(userId, code).Result)
            return Unauthorized($"You are not allowed to delete {code.QRName} QrCode");

        qrCodeRepository.Remove(code);
        await unitOfWork.SaveChanges();

        return Ok($"{code.QRName} Deleted");
    }    
    private  async Task<string> GenerateQRCode(QRCode qRCode)
    {
        string imgPath = configuration.GetValue<string>("JWT:Audience") + qRCode.HashValue;
        QRCodeGenerator qrGenerator = new();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(imgPath, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode pngBytes = new(qrCodeData);
        return await  azureServices.UploadToAzureStorage(pngBytes);
    }
    private async Task<bool> IsAuthorized(string id,QRCode qrCode)
    {
        if(id == qrCode.UserId) 
            return true;

        AppUser user = await userManager.FindByIdAsync(id);
        if (user == null) return false;
        return await userManager.IsInRoleAsync(user, "Admin");
    }
   
}
