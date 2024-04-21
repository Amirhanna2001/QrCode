using QrCode.API.DTOs;

namespace QrCode.API.Services;

public interface IEmailServices
{
    void SendEmail(EmailDTO dto);
}
