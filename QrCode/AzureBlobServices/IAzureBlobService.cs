using QRCoder;

namespace QrCode.API.AzureBlobServices
{
    public interface IAzureBlobService
    {
        Task<string> UploadToAzureStorage(PngByteQRCode qrCode);
    }
}
