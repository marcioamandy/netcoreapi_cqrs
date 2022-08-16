using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;

namespace Globo.PIC.Infra.AWS
{
    public class S3Client : IS3Client
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly AWSOptions _awsOptions;

        /// <summary>
        /// 
        /// </summary>
        private readonly S3Configuration _s3Config;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="awsOptions"></param>
        /// <param name="sqsConfig"></param>
        public S3Client(AWSOptions awsOptions,
                        S3Configuration s3Config)
        {
            _awsOptions = awsOptions;
            _s3Config = s3Config;
        }

        /// <summary>
        /// Generate a Pre-Signed URL to Upload file to Temp Bucket (PUT) or to Download file (GET) from the orignal or compressed folder of Audio Bucket
        /// </summary>
        /// <param name="key">File name to upload</param>
        /// <param name="contentType">File Content-Type to upload</param>
        /// <param name="verb">HttpVerb</param>
        /// <param name="expires">Datetiem to expire</param>
        /// <returns></returns>
        public string GetPreSignedUrl(string key, string fileName, string contentType, string verb, DateTime? expires)
        {
            var httpVerb = Enum.Parse<HttpVerb>(verb.ToUpper());

            expires = (!expires.HasValue || expires.Value == DateTime.MinValue) ? DateTime.Now.AddMinutes(5) : expires.Value;

            using (var client = _awsOptions.CreateServiceClient<IAmazonS3>())
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = Environment.GetEnvironmentVariable("S3_SERVICE_ASSETS"),
                    Key = key,
                    Verb = httpVerb,
                    ContentType = contentType,                   
                    Expires = expires.Value                    
                };                

                if (request.Verb.Equals(HttpVerb.GET))
                {
                    request.BucketName = Environment.GetEnvironmentVariable("S3_SERVICE_ASSETS");

                    request.ResponseHeaderOverrides = new ResponseHeaderOverrides
                    {
                        ContentDisposition = $@"attachment; filename=""{fileName}"""
                    };
                }

				try
				{
                    return client.GetPreSignedURL(request);
				}
				catch (Exception)
				{
                    return null;
                }
            }
        }

        /// <sumary>
        /// Copy the object from temp-bucket to audio-bucket
        /// </summary>
        /// <param name="key">Object from temp-bucket to copy to audio-bucket</param>
        /// <param name="cancellationToken"></param>
        public void CopyObjectToBucket(string key, CancellationToken cancellationToken)
        {
            using (var client = _awsOptions.CreateServiceClient<IAmazonS3>())
            {
                var copyRequest = new CopyObjectRequest
                {
                    SourceBucket = Environment.GetEnvironmentVariable("S3_SERVICE_ASSETS"),
                    SourceKey = key,
                    DestinationBucket = Environment.GetEnvironmentVariable("S3_SERVICE_ASSETS"),
                    DestinationKey = $"{_s3Config.FolderPath}/{key}"
                };

                client.CopyObjectAsync(copyRequest, cancellationToken).Wait();
            }
        }

        /// <sumary>
        /// Copy the object from audio-bucket to temp-bucket renaming to tempFileName
        /// </summary>
        /// <param name="key">Object from audio-bucket to copy to temp-bucket</param>
        /// <param name="tempFileName">Temp file name to copy</param>
        /// <param name="cancellationToken"></param>
        public void CopyObjectToTemp(string key, string tempFileName, CancellationToken cancellationToken)
        {
            using (var client = _awsOptions.CreateServiceClient<IAmazonS3>())
            {                
                var copyRequest = new CopyObjectRequest
                {
                    SourceBucket = Environment.GetEnvironmentVariable("S3_SERVICE_ASSETS"),
                    SourceKey = key,
                    DestinationBucket = Environment.GetEnvironmentVariable("S3_SERVICE_ASSETS"),
                    DestinationKey = tempFileName
                };

                client.CopyObjectAsync(copyRequest, cancellationToken).Wait();
            }
        }

        /// <sumary>
        /// Delete object from temp-bucket
        /// </summary>
        /// <param name="key">File name that was deleted</param>
        /// <param name="cancellationToken"></param>
        public void DeleteObjectFromTemp(string key, CancellationToken cancellationToken)
        {
            using (var client = _awsOptions.CreateServiceClient<IAmazonS3>())
            {
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = Environment.GetEnvironmentVariable("S3_SERVICE_ASSETS"),
                    Key = key
                };

                client.DeleteObjectAsync(deleteRequest, cancellationToken).Wait();
            }
        }

        /// <sumary>
        /// Delete object from audio-bucket
        /// </summary>
        /// <param name="key">File name that was deleted</param>
        /// <param name="cancellationToken"></param>
        public void DeleteObjectFromBucket(string key, CancellationToken cancellationToken)
        {
            using (var client = _awsOptions.CreateServiceClient<IAmazonS3>())
            {
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = Environment.GetEnvironmentVariable("S3_SERVICE_ASSETS"),
                    Key = $"{_s3Config.FolderPath}/{key}"
                };

                client.DeleteObjectAsync(deleteRequest, cancellationToken).Wait();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
