using AutoMapper;
using QrCode.API.DTOs;
using QrCode.DB.Models;

namespace QrCode.API.AutoMapper;

public class QrCodeMapperProfile:Profile
{
    public QrCodeMapperProfile()
    {
        CreateMap<CreateQRCodeDTO, QRCode>().ReverseMap();
    }
}
