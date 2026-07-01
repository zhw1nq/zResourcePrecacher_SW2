using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Plugins;

namespace zResourcePrecacher;

[PluginMetadata(Id = "zResourcePrecacher", Version = "1.0.1", Name = "zResourcePrecacher", Author = "vhming", Description = "Precache any Resource in CS2")]
public partial class zResourcePrecacher : BasePlugin
{
    private ServiceProvider? serviceProvider;
    private PrecacheContext? precacheContext;
    private PluginMigrations? pluginMigrations;
    private IOptionsMonitor<PluginConfig>? configMonitor;

    public new ISwiftlyCore Core => base.Core;

    public PluginConfig activeConfig => this.configMonitor?.CurrentValue ?? new PluginConfig();

    public zResourcePrecacher(ISwiftlyCore core) : base(core)
    {
    }

    public override void ConfigureSharedInterface(IInterfaceManager interfaceManager)
    {
    }

    public override void UseSharedInterface(IInterfaceManager interfaceManager)
    {
    }

    public override void Load(bool hotReload)
    {
        if (hotReload)
        {
            this.Core.Logger.LogWarning("Hotreloading zResourcePrecacher has no effect.");
        }

        const string ConfigFileName = "config.jsonc";
        const string ConfigSection = "zResourcePrecacher";

        this.Core.Configuration
            .InitializeJsonWithModel<PluginConfig>(ConfigFileName, ConfigSection)
            .Configure(builder => builder.AddJsonFile(
                this.Core.Configuration.GetConfigPath(ConfigFileName),
                optional: false,
                reloadOnChange: true));

        var services = new ServiceCollection();
        services.AddSwiftly(this.Core);
        services.AddOptionsWithValidateOnStart<PluginConfig>().BindConfiguration(ConfigSection);
        services.AddSingleton<PluginMigrations>();
        services.AddSingleton(this);
        services.AddSingleton<PrecacheContext>();

        this.serviceProvider = services.BuildServiceProvider();
        this.configMonitor = this.serviceProvider.GetRequiredService<IOptionsMonitor<PluginConfig>>();
        this.pluginMigrations = this.serviceProvider.GetRequiredService<PluginMigrations>();
        this.precacheContext = this.serviceProvider.GetRequiredService<PrecacheContext>();

        var loadedConfig = this.activeConfig;
        var defaultConfig = new PluginConfig();
        if (loadedConfig.version < defaultConfig.version)
        {
            this.Core.Logger.LogWarning("Configuration is out of date. Consider updating the plugin.");

            if (this.pluginMigrations.hasInstruction(loadedConfig.version, defaultConfig.version))
            {
                this.Core.Logger.LogWarning("Instruction for migrating your config file: {0}", this.pluginMigrations.getInstruction(loadedConfig.version, defaultConfig.version));
            }
            else
            {
                this.Core.Logger.LogWarning("No migrating instruction available");
            }
        }

        this.configMonitor.OnChange(newConfig =>
        {
            this.Core.Logger.LogInformation("Configuration reloaded.");
            this.precacheContext?.initialize();

            if (newConfig.version < defaultConfig.version)
            {
                this.Core.Logger.LogWarning("Configuration is out of date. Consider updating the plugin.");
                if (this.pluginMigrations.hasInstruction(newConfig.version, defaultConfig.version))
                {
                    this.Core.Logger.LogWarning("Instruction for migrating your config file: {0}", this.pluginMigrations.getInstruction(newConfig.version, defaultConfig.version));
                }
            }
        });

        this.precacheContext.initialize();
        this.Core.Event.OnPrecacheResource += this.precacheContext.onPrecacheResource;
    }

    public override void Unload()
    {
        if (this.precacheContext != null)
        {
            this.Core.Event.OnPrecacheResource -= this.precacheContext.onPrecacheResource;
        }

        if (this.serviceProvider != null)
        {
            this.serviceProvider.Dispose();
        }
    }
}