using Microsoft.AspNetCore.Mvc;
using QrCode.DB.Models;

namespace QrCode.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DeviceTypeController : ControllerBase
{
    //[HttpGet]
    //public IActionResult GetAll()
    //{
    //    IEnumerable<DeviceType> enumValues = Enum.GetValues(typeof(DeviceType)).Cast<DeviceType>();
    //    return Ok( enumValues.Select(e => Enum.GetName(typeof(DeviceType), e)).ToList());
    //}
}
