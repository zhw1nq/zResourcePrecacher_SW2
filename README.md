<div align="center">
  <h1>zResourcePrecacher</h1>
  <p><strong>Precache any Resource in CS2.</strong></p>
  <p>
    <a href="https://github.com/swiftly-solution/swiftlys2"><img src="upload://9FgzMGUaXfQ2z6enD1GU5P4Dyfa.svg" alt="Platform"></a>
    <img src="upload://5D7z9a67wMEpmHQY79pSubs65F.svg" alt=".NET Version">
    <img src="upload://yDx742gCfHrMumfsqVdDlNJF06V.svg" alt="License">
  </p>
</div>

## 🔗 Repository

https://github.com/zhw1nq/zResourcePrecacher_SW2

---

## 📌 About

**zResourcePrecacher** is a plugin for **SwiftlyS2** that automatically precaches custom resources when a map starts. It scans custom Workshop `.vpk` packages as well as manually configured resources, ensuring all assets are loaded before gameplay begins. This helps prevent missing models, purple/black error textures, missing sounds, particle loading delays.

* **Original CounterStrikeSharp version**: [KillStr3aK/ResourcePrecacher](https://github.com/KillStr3aK/ResourcePrecacher)
* **SwiftlyS2 Port**: Ported and adapted for SwiftlyS2 by vhming with AI.

---

## ✨ Features

* **Automatic VPK Scanning**: Scans `.vpk` files inside the `Assets/` folder and automatically precaches all supported resources inside them (only reads directory `.vpk` files, ignoring sub-parts).
* **Manual Resource List**: Precache specific individual resource paths configured inside `config.jsonc`.
* **Hot Reload**: Configuration changes are detected and reloaded automatically without restarting the server.
* **Version Checking**: Warns when an outdated configuration schema is detected and provides migration guides.
* **Modern & Clean**: Built with C# using .NET 10, Dependency Injection, and type-safe APIs.

---

## 🚀 Installation

### Requirements
* **SwiftlyS2 Server Environment**
* **.NET 10 SDK** (only needed if compiling from source)

### Steps
1. Place the plugin directory in:
   ```text
   addons/swiftlys2/plugins/zResourcePrecacher/
   ```
2. (Optional) Put Workshop `.vpk` packages inside:
   ```text
   addons/swiftlys2/plugins/zResourcePrecacher/Assets/
   ```
3. Start or restart your server. The plugin will automatically create the default config file at:
   ```text
   addons/swiftlys2/configs/plugins/zResourcePrecacher/config.jsonc
   ```

---

## ⚙️ Configuration

Default `config.jsonc` file layout:

```json
{
  "zResourcePrecacher": {
    "Resources": [],
    "Log": true,
    "Version": 5
  }
}
```

### Options
* **`Resources`**: List of specific custom asset paths to precache.
  ```json
  "Resources": [
    "characters/models/custom_player.vmdl",
    "materials/custom/player.vmat",
    "particles/custom/fire.vpcf",
    "sounds/custom/music.vsnd"
  ]
  ```
* **`Log`**: Set to `true` to enable verbose console logs during precaching; `false` to disable.
* **`Version`**: The configuration structure version (do not edit manually).

---

## 🛠️ Building

To compile the plugin from source:

```bash
# Clone the repository
git clone <repository>
cd zResourcePrecacher_SW2

# Build the project
dotnet build

# Publish for production release
dotnet publish -c Release
```
Compiled release files are placed in:
```text
build/publish/zResourcePrecacher/
```

---

## 📂 Project Structure

```text
zResourcePrecacher/
├── Assets/                 # Workshop VPK folder (created automatically)
├── src/                    # Plugin source files
│   ├── zResourcePrecacher.cs
│   ├── PluginConfig.cs
│   ├── PluginMigrations.cs
│   └── PrecacheContext.cs
├── resources/              # Static plugin files and templates
└── zResourcePrecacher.csproj
```

---

<div align="center">
  <p>Made with ❤️ for the SwiftlyS2 Community</p>
</div>
