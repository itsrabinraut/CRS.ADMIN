﻿namespace CRS.ADMIN.APPLICATION.Models
{
    public class AmazonS3Configruation
    {
        public string AccessKeyId { get; set; }
        public string SecretAccessKey { get; set; }
        public string BucketName { get; set; }
        public int TimeoutDurationInMin { get; set; }
        public string BaseURL { get; set; }
    }

    public class AmazonConfigruation
    {
        public AmazonS3Configruation AmazonS3Configruation { get; set; }
    }
}