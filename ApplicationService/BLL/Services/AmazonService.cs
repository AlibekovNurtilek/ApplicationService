using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using ApplicationService.BLL.Models;
using Microsoft.Extensions.Options;

namespace ApplicationService.BLL.Services
{
    public interface IAmazonService
    {
        Task<string> UploadImage(string fullBase64);
    }
    public class AmazonService : IAmazonService
    {

        private readonly AmazonSettings _amazonSettings;

        public AmazonService(IOptions<AmazonSettings> options)
        {
            this._amazonSettings = options.Value;
        }

        public async Task<string> UploadImage(string fullBase64)
        {
            var s3Client = new AmazonS3Client(_amazonSettings.ConfigAccess,
                                              _amazonSettings.ConfigSecret,
                                              RegionEndpoint.EUNorth1);
            try
            {
                var position = fullBase64.Split(new string[] { ";base64," }, StringSplitOptions.None);
                if (position.Length <= 1)
                {
                    return await Task.FromResult(((Func<string>)(() =>
                    {
                        return "This is not a valid base 64 format";
                    }))());
                }
                var base64 = position[1];
                var fileName = await GetFileName(fullBase64);
                return await UploadImageIntoS3Bucket(s3Client, base64, fileName);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(((Func<string>)(() =>
                {
                    return ex.Message;
                }))());
            }

        }


        private async Task<string> UploadImageIntoS3Bucket(AmazonS3Client s3Client, string base64, string fileName)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            using (s3Client)
            {
                var request = new PutObjectRequest
                {
                    BucketName = _amazonSettings.BucketName,
                    CannedACL = S3CannedACL.PublicRead,
                    Key = string.Format($"Medium/{fileName}")
                };
                using (var ms = new MemoryStream(bytes))
                {
                    request.InputStream = ms;
                    await s3Client.PutObjectAsync(request);
                }
                return await Task.FromResult(((Func<string>)(() =>
                {
                    return $"https://asmangroupimages.s3.eu-north-1.amazonaws.com/Medium/{fileName}";
                }))());
            }
        }

        private async Task<string> GetFileName(string base64)
        {
            var extension = !string.IsNullOrEmpty(base64) ? base64.Split(new string[] { ";base64," }, StringSplitOptions.None)[0].Replace("data:image/", "") : "";
            string fileName = $"{DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss")}.{extension}";
            return await Task.FromResult(((Func<string>)(() =>
            {
                return fileName;
            }))());
        }
    }
}

