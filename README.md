<div align="center">
  <h2><strong>zResourcePrecacher</strong></h2>
  <h3>Precache any Resource in CS2 (SwiftlyS2 Plugin)</h3>
  <p>This plugin is ported from the original CounterStrikeSharp version by AI.</p>
</div>

<p align="center">
  <img src="https://img.shields.io/badge/platform-swiftlys2-orange" alt="Platform">
  <img src="https://img.shields.io/badge/dotnet-10.0-blue" alt=".NET Version">
  <img src="https://img.shields.io/badge/license-MIT-green" alt="License">
</p>

---

## 📌 About The Project

**zResourcePrecacher** is a SwiftlyS2 plugin designed for Counter-Strike 2 that automatically precaches customized game resources (models, materials, sounds, particle effects, etc.) during map loads. Precaching resources ensures that players don't experience stutter, lag, or missing assets when custom models or effects are spawned in-game.

This project is a port of the original CSSharp plugin:
🔗 **Original Repository**: [KillStr3aK/ResourcePrecacher](https://github.com/KillStr3aK/ResourcePrecacher)  
🤖 **Ported by**: Antigravity (Advanced Agentic Coding AI at Google DeepMind)

---

## ✨ Features

- **Automated VPK Scan**: Place your custom workshop `.vpk` packages in the `Assets/` directory, and the plugin will automatically parse and precache all valid assets inside them.
- **Custom Resource List**: Explicitly specify paths of assets you want to precache directly in the JSON configuration file.
- **Config Hot-Reload**: Configuration changes are monitored and reloaded automatically without restarting the server.
- **Version Migration Helper**: Automatically warns and guides server owners if their configuration file is out of date.
- **Clean Architecture**: Written in .NET 10.0 using Dependency Injection and modern, type-safe API patterns.

---

## 🚀 Getting Started

### Prerequisites

- [SwiftlyS2 CS2 Server Environment](https://github.com/swiftly-solution/swiftlys2)
- .NET 10.0 SDK (only needed if building from source)

### Installation

1. Download or publish the compiled build.
2. Place the `zResourcePrecacher` folder inside your server's `addons/swiftlys2/plugins/` directory.
3. (Optional) Place custom workshop `.vpk` files into `addons/swiftlys2/plugins/zResourcePrecacher/Assets/`.
4. Start/Restart the server. A default configuration file will be automatically generated at `addons/swiftlys2/configs/plugins/zResourcePrecacher/config.json`.

---

## ⚙️ Configuration

The default generated configuration (`config.json`) looks like this:

```json
{
  "zResourcePrecacher": {
    "Resources": [],
    "Log": true,
    "Version": 5
  }
}
```

### Options:
- **`Resources`**: A list of paths to individual assets you wish to precache. (e.g. `characters/models/custom_player.vmdl`).
- **`Log`**: Set to `true` to print detailed precaching logs in the server console; set to `false` to disable.
- **`Version`**: The configuration schema version. Do not modify this manually.

---

## 🛠️ Building from Source

To compile the plugin yourself:

```bash
# Clone the repository and navigate to directory
cd zResourcePrecacher_SW2

# Build the project
dotnet build

# Publish the project for distribution
dotnet publish -c Release
```

The output and ready-to-use plugin package will be located in `build/publish/zResourcePrecacher/`.