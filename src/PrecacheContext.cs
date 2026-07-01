namespace zResourcePrecacher;

using Microsoft.Extensions.Logging;
using SteamDatabase.ValvePak;
using SwiftlyS2.Shared.Events;

public sealed class PrecacheContext
{
    private readonly zResourcePrecacher plugin;
    private readonly HashSet<string> resources = new();
    private readonly ILogger<PrecacheContext> logger;

    public int resourceCount => this.resources.Count;
    public string assetsDirectory => Path.Combine(this.plugin.Core.PluginPath, "Assets");

    private readonly HashSet<string> resourceTypes = new()
    {
        "vmdl",     "vmdl_c",
        "vpcf",     "vpcf_c",
        "vmat",     "vmat_c",
        "vcompmat", "vcompmat_c",
        "vtex",     "vtex_c",
        "vsnd",     "vsnd_c",
        "vdata",    "vdata_c",
        "vpost",    "vpost_c",
        "vsurf",    "vsurf_c",
        "vanmgrph", "vanmgrph_c",
        "vmix",     "vmix_c",
        "vnmclip",  "vnmclip_c",
        "vrman",    "vrman_c",
        "vrr_c",    "vrr_c",
        "vsc",
        "vsmart",   "vsmart_c",
        "vsnap",    "vsnap_c",
        "vsndevts", "vsndevts_c",
        "vsndgrps",
        "vsndstck", "vsndstck_c",
        "vsvg",     "vsvg_c",
        "vts",      "vts_c",
        "vxml",     "vxml_c"
    };

    public PrecacheContext(ILogger<PrecacheContext> logger, zResourcePrecacher plugin)
    {
        this.logger = logger;
        this.plugin = plugin;
    }

    public void initialize()
    {
        if (!Directory.Exists(this.assetsDirectory))
        {
            try
            {
                Directory.CreateDirectory(this.assetsDirectory);
                this.logger.LogInformation("Created Assets directory at: '{0}'", this.assetsDirectory);
            }
            catch (Exception ex)
            {
                this.logger.LogError("Failed to create Assets directory: {0}", ex.Message);
            }
        }

        this.resources.Clear();

        if (Directory.Exists(this.assetsDirectory))
        {
            foreach (string vpkPath in Directory.EnumerateFiles(this.assetsDirectory, "*.vpk", SearchOption.AllDirectories))
            {
                // we can only read the `_dir` vpks
                if (vpkPath.EndsWith("_000.vpk"))
                {
                    continue;
                }

                string packageName = Path.GetFileNameWithoutExtension(vpkPath);

                using (Package package = new())
                {
                    try
                    {
                        this.logger.LogInformation("Reading Workshop Package: '{0}'", packageName);

                        package.Read(vpkPath);

                        if (package.Entries != null)
                        {
                            foreach (KeyValuePair<string, List<PackageEntry>> fileType in package.Entries)
                            {
                                if (!this.resourceTypes.Contains(fileType.Key))
                                {
                                    continue;
                                }

                                foreach (PackageEntry entry in fileType.Value)
                                {
                                    string fullPath = entry.GetFullPath();

                                    if (fullPath.EndsWith("_c"))
                                    {
                                        fullPath = fullPath[..^2];
                                    }

                                    if (!this.addResource(fullPath))
                                    {
                                        this.logger.LogWarning("Duplicate entry for resource: '{0}'", fullPath);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError("Unable to read package: '{0}' ({1})", packageName, ex.Message);
                    }
                }
            }
        }

        var currentConfig = this.plugin.activeConfig;
        if (currentConfig.resourceList.Count == 0)
        {
            this.logger.LogWarning("'Resources' list is empty in config, did you forget to populate it?");
        }
        else
        {
            foreach (string resourcePath in currentConfig.resourceList)
            {
                if (!this.addResource(resourcePath))
                {
                    this.logger.LogWarning("Duplicate entry for resource in configuration: '{0}'", resourcePath);
                }
            }
        }
    }

    public bool addResource(string resourcePath)
    {
        if (resourcePath.Contains('/'))
        {
            resourcePath = resourcePath.Replace('/', Path.DirectorySeparatorChar);
        }

        string extension = Path.GetExtension(resourcePath);
        if (string.IsNullOrEmpty(extension))
        {
            this.logger.LogError("Resource '{0}' has no extension and cannot be precached.", resourcePath);
            return true;
        }

        string extensionName = extension[1..];

        if (!this.resourceTypes.Contains(extensionName))
        {
            this.logger.LogError("Resource type '{0}' can not be precached. ({1})", extensionName, resourcePath);
            return true;
        }

        return this.resources.Add(resourcePath);
    }

    public bool removeResource(string resourcePath)
    {
        if (resourcePath.Contains('/'))
        {
            resourcePath = resourcePath.Replace('/', Path.DirectorySeparatorChar);
        }

        return this.resources.Remove(resourcePath);
    }

    public void onPrecacheResource(IOnPrecacheResourceEvent @event)
    {
        int precachedResources = 0;
        var currentConfig = this.plugin.activeConfig;

        foreach (string resourcePath in this.resources)
        {
            if (currentConfig.log)
            {
                this.logger.LogInformation("Precaching \"{Resource}\" [{Amount}/{Count}]", resourcePath, ++precachedResources, this.resourceCount);
            }

            @event.AddItem(resourcePath);
        }

        if (currentConfig.log)
        {
            this.logger.LogInformation("Precached {ResourceCount} resources.", this.resourceCount);
        }
    }
}
