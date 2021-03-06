namespace Andy.X.Cli.Models.Configurations
{
    public class TenantConfiguration
    {
        public string Name { get; set; }
        public List<ProductConfiguration> Products { get; set; }
        public TenantSettings Settings { get; set; }

        public TenantConfiguration()
        {
            Products = new List<ProductConfiguration>();
            Settings = new TenantSettings();
        }
    }
    public class TenantSettings
    {
        public bool AllowProductCreation { get; set; }
        public string DigitalSignature { get; set; }
        public bool EnableEncryption { get; set; }
        public bool EnableGeoReplication { get; set; }
        public TenantLogging Logging { get; set; }

        public bool EnableAuthorization { get; set; }

        // Split tenants by certificates will not be possible with version two
        public string CertificatePath { get; set; }

        public TenantSettings()
        {
            AllowProductCreation = true;
            EnableEncryption = false;

            EnableAuthorization = false;

            EnableGeoReplication = false;
            Logging = TenantLogging.ERROR_ONLY;
        }
    }
    public enum TenantLogging
    {
        ALL,
        INFORMATION_ONLY,
        WARNING_ONLY,
        ERROR_ONLY,
    }

    public class TenantToken
    {
        public string Token { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpireDate { get; set; }
        public string IssuedFor { get; set; }
        public DateTime IssuedDate { get; set; }
    }

    public class ProductConfiguration
    {
        public string Name { get; set; }
        public List<ComponentConfiguration> Components { get; set; }

        public ProductConfiguration()
        {
            Components = new List<ComponentConfiguration>();
        }
    }

    public class ComponentConfiguration
    {
        public string Name { get; set; }
        public List<TopicConfiguration> Topics { get; set; }
        public ComponentSettings Settings { get; set; }

        public ComponentConfiguration()
        {
            Topics = new List<TopicConfiguration>();
            Settings = new ComponentSettings();
        }
    }

    public class ComponentSettings
    {
        public bool AllowSchemaValidation { get; set; }
        public bool AllowTopicCreation { get; set; }

        public bool EnableAuthorization { get; set; }
        public List<ComponentToken> Tokens { get; set; }

        public ComponentRetention RetentionPolicy { get; set; }



        public ComponentSettings()
        {
            AllowSchemaValidation = false;
            AllowTopicCreation = true;
            EnableAuthorization = false;

            Tokens = new List<ComponentToken>();
            RetentionPolicy = new ComponentRetention();
        }
    }

    public class ComponentRetention
    {
        public string Name { get; set; }
        public long RetentionTimeInMinutes { get; set; }

        public ComponentRetention()
        {
            Name = "default";
            RetentionTimeInMinutes = -1;
        }
    }

    public class ComponentToken
    {
        public string Name { get; set; }
        public string Description { get; set; }

        // TOKEN will be generated from andyx-cli and it be added manually via tenants.json
        public string Token { get; set; }

        public bool IsActive { get; set; }

        public bool CanConsume { get; set; }
        public bool CanProduce { get; set; }

        public DateTime ExpireDate { get; set; }
        public string IssuedFor { get; set; }
        public DateTime IssuedDate { get; set; }
    }

    public class TopicConfiguration
    {
        public string Name { get; set; }
    }
}
