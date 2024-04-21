using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using QRCoder;
using System.Net.NetworkInformation;

namespace QrCode.API.AzureBlobServices
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly IConfiguration configuration;
        private readonly BlobContainerClient containerClient;

        public AzureBlobService(IConfiguration configuration)
        {
            this.configuration = configuration;
            BlobServiceClient blobServiceClient = new BlobServiceClient(configuration.GetValue<string>("BlobService:Connection"));
            containerClient = blobServiceClient.GetBlobContainerClient(configuration.GetValue<string>("BlobService:Container"));
        }
        public async Task<string> UploadToAzureStorage(PngByteQRCode qrCode)
        {
            string path = $"{configuration.GetValue<string>("BlobService:path")}{qrCode.GetHashCode()}.png";

            BlobClient blobClient = containerClient.GetBlobClient($"{configuration.GetValue<string>("BlobService:path")}{qrCode.GetHashCode()}.png");
            await blobClient.UploadAsync(new BinaryData(qrCode.GetGraphic(50)));

            return $"{configuration.GetValue<string>("BlobService:baseUrl")}/{path}";
        }
    }
}
