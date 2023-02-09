namespace WorkTool.Core.Modules.SqlServer.Models;

public record SqlServerConnectionParameters : ConnectionParametersBase
{
    public const string DefaultKeyTrustedConnection = "Trusted_Connection";
    public const string DefaultKeyConnectionTimeout = "Connection Timeout";
    public const string DefaultKeyTrustServerCertificate = "TrustServerCertificate";
    public const string DefaultKeyPersistSecurityInfo = "Persist Security Info";
    public const string DefaultKeyInitialCatalog = "Initial Catalog";
    public const string DefaultKeyEncrypt = "Encrypt";
    public const string DefaultKeyServer = "Server";
    public const string DefaultKeyUserId = "User ID";
    public const string DefaultKeyPassword = "Password";
    public const string DefaultKeyIntegratedSecurity = "Integrated Security";
    public const string DefaultKeyMultipleActiveResultSets = "MultipleActiveResultSets";
    public const string DefaultValueInitialCatalog = "";
    public const string DefaultValueServer = "";
    public const string DefaultValueUserId = "";
    public const string DefaultValuePassword = "";
    public const int DefaultValueConnectionTimeout = 15;
    public const bool DefaultValueEncrypt = true;
    public const bool DefaultValueMultipleActiveResultSets = false;
    public const bool DefaultValueTrustedConnection = false;
    public const bool DefaultValuePersistSecurityInfo = true;
    public const bool DefaultValueTrustServerCertificate = true;

    public const SqlServerIntegratedSecurity DefaultValueIntegratedSecurity =
        SqlServerIntegratedSecurity.False;

    public static readonly StringConnectionParameterInfo InitialCatalogParameterInfo;
    public static readonly StringConnectionParameterInfo ServerParameterInfo;
    public static readonly StringConnectionParameterInfo UserIdParameterInfo;
    public static readonly StringConnectionParameterInfo PasswordParameterInfo;
    public static readonly BooleanConnectionParameterInfo EncryptParameterInfo;
    public static readonly BooleanConnectionParameterInfo PersistSecurityInfoInfo;
    public static readonly BooleanConnectionParameterInfo TrustServerCertificateInfo;
    public static readonly SqlServerIntegratedSecurityConnectionParameterInfo IntegratedSecurityParameterInfo;
    public static readonly BooleanConnectionParameterInfo MultipleActiveResultSetsParameterInfo;
    public static readonly BooleanConnectionParameterInfo TrustedConnectionParameterInfo;
    public static readonly Int32ConnectionParameterInfo TimeoutParameterInfo;

    private readonly Dictionary<ConnectionParameterInfo, ConnectionParameterValue> parameters =
        new();

    public int ConnectionTimeout
    {
        get
        {
            if (parameters.ContainsKey(TimeoutParameterInfo))
            {
                return int.Parse(parameters[TimeoutParameterInfo].Value);
            }

            return int.Parse(TimeoutParameterInfo.DefaultValue);
        }
        init => parameters[TimeoutParameterInfo] = new Int32ConnectionParameterValue(value);
    }

    public bool TrustServerCertificate
    {
        get
        {
            if (parameters.ContainsKey(TrustServerCertificateInfo))
            {
                return bool.Parse(parameters[TrustServerCertificateInfo].Value);
            }

            return bool.Parse(TrustServerCertificateInfo.DefaultValue);
        }
        init => parameters[TrustServerCertificateInfo] = new BooleanConnectionParameterValue(value);
    }

    public bool PersistSecurityInfo
    {
        get
        {
            if (parameters.ContainsKey(PersistSecurityInfoInfo))
            {
                return bool.Parse(parameters[PersistSecurityInfoInfo].Value);
            }

            return bool.Parse(PersistSecurityInfoInfo.DefaultValue);
        }
        init => parameters[PersistSecurityInfoInfo] = new BooleanConnectionParameterValue(value);
    }

    public bool Encrypt
    {
        get
        {
            if (parameters.ContainsKey(EncryptParameterInfo))
            {
                return bool.Parse(parameters[EncryptParameterInfo].Value);
            }

            return bool.Parse(EncryptParameterInfo.DefaultValue);
        }
        init => parameters[EncryptParameterInfo] = new BooleanConnectionParameterValue(value);
    }

    public bool TrustedConnection
    {
        get
        {
            if (parameters.ContainsKey(TrustedConnectionParameterInfo))
            {
                return bool.Parse(parameters[TrustedConnectionParameterInfo].Value);
            }

            return bool.Parse(TrustedConnectionParameterInfo.DefaultValue);
        }
        init =>
            parameters[TrustedConnectionParameterInfo] = new BooleanConnectionParameterValue(value);
    }

    public string InitialCatalog
    {
        get
        {
            if (parameters.ContainsKey(InitialCatalogParameterInfo))
            {
                return parameters[InitialCatalogParameterInfo].Value;
            }

            return InitialCatalogParameterInfo.DefaultValue;
        }
        init => parameters[InitialCatalogParameterInfo] = new StringConnectionParameterValue(value);
    }

    public bool MultipleActiveResultSets
    {
        get
        {
            if (parameters.ContainsKey(MultipleActiveResultSetsParameterInfo))
            {
                return bool.Parse(parameters[MultipleActiveResultSetsParameterInfo].Value);
            }

            return bool.Parse(MultipleActiveResultSetsParameterInfo.DefaultValue);
        }
        init =>
            parameters[MultipleActiveResultSetsParameterInfo] = new BooleanConnectionParameterValue(
                value
            );
    }

    public SqlServerIntegratedSecurity IntegratedSecurity
    {
        get
        {
            if (parameters.ContainsKey(IntegratedSecurityParameterInfo))
            {
                return Enum.Parse<SqlServerIntegratedSecurity>(
                    parameters[IntegratedSecurityParameterInfo].Value
                );
            }

            return Enum.Parse<SqlServerIntegratedSecurity>(
                IntegratedSecurityParameterInfo.DefaultValue
            );
        }
        init =>
            parameters[IntegratedSecurityParameterInfo] =
                new SqlServerIntegratedSecurityConnectionParameterValue(value);
    }

    public string Password
    {
        get
        {
            if (parameters.ContainsKey(PasswordParameterInfo))
            {
                return parameters[PasswordParameterInfo].Value;
            }

            return PasswordParameterInfo.DefaultValue;
        }
        init => parameters[PasswordParameterInfo] = new StringConnectionParameterValue(value);
    }

    public string UserId
    {
        get
        {
            if (parameters.ContainsKey(UserIdParameterInfo))
            {
                return parameters[UserIdParameterInfo].Value;
            }

            return UserIdParameterInfo.DefaultValue;
        }
        init => parameters[UserIdParameterInfo] = new StringConnectionParameterValue(value);
    }

    public string Server
    {
        get
        {
            if (parameters.ContainsKey(ServerParameterInfo))
            {
                return parameters[ServerParameterInfo].Value;
            }

            return ServerParameterInfo.DefaultValue;
        }
        init => parameters[ServerParameterInfo] = new StringConnectionParameterValue(value);
    }

    static SqlServerConnectionParameters()
    {
        ServerParameterInfo = new StringConnectionParameterInfo(
            DefaultKeyServer,
            new[] { DefaultKeyServer },
            DefaultValueServer
        );

        UserIdParameterInfo = new StringConnectionParameterInfo(
            DefaultKeyUserId,
            new[] { DefaultKeyUserId },
            DefaultValueUserId
        );

        PasswordParameterInfo = new StringConnectionParameterInfo(
            DefaultKeyPassword,
            new[] { DefaultKeyPassword },
            DefaultValuePassword
        );

        IntegratedSecurityParameterInfo = new SqlServerIntegratedSecurityConnectionParameterInfo(
            DefaultKeyIntegratedSecurity,
            new[] { DefaultKeyIntegratedSecurity },
            DefaultValueIntegratedSecurity
        );

        MultipleActiveResultSetsParameterInfo = new BooleanConnectionParameterInfo(
            DefaultKeyMultipleActiveResultSets,
            new[] { DefaultKeyMultipleActiveResultSets },
            DefaultValueMultipleActiveResultSets
        );

        InitialCatalogParameterInfo = new StringConnectionParameterInfo(
            DefaultKeyInitialCatalog,
            new[] { DefaultKeyInitialCatalog, "Database" },
            DefaultValueInitialCatalog
        );

        TrustedConnectionParameterInfo = new BooleanConnectionParameterInfo(
            DefaultKeyTrustedConnection,
            new[] { DefaultKeyTrustedConnection },
            DefaultValueTrustedConnection
        );

        EncryptParameterInfo = new BooleanConnectionParameterInfo(
            DefaultKeyEncrypt,
            new[] { DefaultKeyEncrypt },
            DefaultValueEncrypt
        );

        PersistSecurityInfoInfo = new BooleanConnectionParameterInfo(
            DefaultKeyPersistSecurityInfo,
            new[] { DefaultKeyPersistSecurityInfo },
            DefaultValuePersistSecurityInfo
        );

        TrustServerCertificateInfo = new BooleanConnectionParameterInfo(
            DefaultKeyTrustServerCertificate,
            new[] { DefaultKeyTrustServerCertificate },
            DefaultValueTrustServerCertificate
        );

        TimeoutParameterInfo = new Int32ConnectionParameterInfo(
            DefaultKeyConnectionTimeout,
            new[] { DefaultKeyConnectionTimeout },
            DefaultValueConnectionTimeout
        );

        ConnectionParameterValues[typeof(SqlServerConnectionParameters)] =
            new ConnectionParameterInfo[]
            {
                InitialCatalogParameterInfo,
                ServerParameterInfo,
                UserIdParameterInfo,
                PasswordParameterInfo,
                EncryptParameterInfo,
                PersistSecurityInfoInfo,
                TrustServerCertificateInfo,
                IntegratedSecurityParameterInfo,
                MultipleActiveResultSetsParameterInfo,
                TrustedConnectionParameterInfo,
                TimeoutParameterInfo
            };
    }

    public SqlServerConnectionParameters()
    {
        Server = DefaultValueServer;
        UserId = DefaultValueUserId;
        Password = DefaultValuePassword;
        IntegratedSecurity = DefaultValueIntegratedSecurity;
        MultipleActiveResultSets = DefaultValueMultipleActiveResultSets;
        InitialCatalog = DefaultValueInitialCatalog;
        TrustedConnection = DefaultValueTrustedConnection;
        Encrypt = DefaultValueEncrypt;
        PersistSecurityInfo = DefaultValuePersistSecurityInfo;
        TrustServerCertificate = DefaultValueTrustServerCertificate;
        ConnectionTimeout = DefaultValueConnectionTimeout;
    }

    public override IEnumerable<ConnectionParameter> Parameters =>
        parameters.Select(x => new ConnectionParameter(x.Key, x.Value));

    public override IEnumerable<ConnectionParameter> SafeParameters => Parameters;
}
