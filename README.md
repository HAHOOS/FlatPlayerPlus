# FlatPlayer+

**FlatPlayer+** (FlatPlayerPlus) is a mod that adds several quality-of-life features for FlatPlayer, Such as reloading, vehicles, and more!

## Features

* Reloading
* Vehicles
* Grip Toggle
* Customizable Bindings
* HUD
    * Health Bar
    * Ammo
* Some Graphics Settings
    * FOV (Field of view)
    * Full-Screen Mode
    * V-Sync
* Ragdoll
   * Requires [RagdollPlayer](https://thunderstore.io/c/bonelab/p/Lakatrazz/Ragdoll_Player/) to be installed


##  Requirements

* PC
* BONELAB Patch 6
* [BoneLib](https://thunderstore.io/c/bonelab/p/bonelib/BoneLib/)
* [FieldInjector](https://thunderstore.io/c/bonelab/p/WNP78/FieldInjector/)
* [FlatPlayer](https://thunderstore.io/c/bonelab/p/LlamasHere/FlatPlayer/)
* [RagdollPlayer](https://thunderstore.io/c/bonelab/p/Lakatrazz/Ragdoll_Player/) [Optional]


## Controls

> Controllers (DualShock, Xbox, etc.) aren't supported ***yet***

| Key       | Action          | Note                                                                                                                     |
| --------- | --------------- | ------------------------------------------------------------------------------------------------------------------------ |
| R         | Reload          | To reload, first eject the magazine, then grab a magazine from the belt, and click `R`                                   |
| W         | Drive Forward   | While holding `W` the character leans forward, to avoid that, hold `S`                                                   |
| LeftShift | Drive Backwards | Why LeftShift? Because `S` is used to avoid the lean that happens when holding `W`                                       |
| LeftCtrl  | Ragdoll         | You must have [RagdollPlayer](https://thunderstore.io/c/bonelab/p/Lakatrazz/Ragdoll_Player/) installed for this to work. |  

### Binding customization

To change the key bindings for the action mentioned above, follow these steps:

1. Open your BONELAB installation folder.
2. Navigate to the **UserData** directory, then go to **MelonPreferences**.

For the installation paths, refer to the following:

- **Steam**: `C:\Program Files (x86)\Steam\steamapps\common\BONELAB\UserData`
- **Meta Link**: `C:\Program Files\Oculus\Software\Software\Stress Level Zero Inc\BONELAB\UserData`

Once you are in the FlatPlayerPlus section, you can modify each binding to your preference.
For more details on key names, please visit: https://docs.unity3d.com/2021.3/Documentation/ScriptReference/KeyCode.html

## Contributing

FlatPlayer+ is entirely open-source, so you are welcome to fork the mod's repository, add any features you desire, and request a pull request.

## Feedback and Issues

If you encounter any issues or want to submit feedback, feel free to visit the mod's GitHub issues page and submit your issue or feedback.

## Credits
* LlamasHere - [FlatPlayer](https://thunderstore.io/c/bonelab/p/LlamasHere/FlatPlayer/)
* Aiden - [BONELAB Icon Template](https://www.figma.com/community/file/1218386424917309834)
* TrevTV - [MelonLoader Mod Template](https://github.com/TrevTV/MelonLoader.VSWizard)
* HAHOOS - Worked on v2.1.0

# ChangeLog

## [2.1.0] Minor Update

### Fixed

* Errors when FlatPlayer not found

### Improved

* Relying on input rather than calling code (RagdollPlayer)
* Code has been refactored for easier readability

## [2.0.0] Major Update

### Added

* BoneMenu
* HUD
* Preferences
* Grip Toggle
* Customizable Bindings
* Some Graphics Settings
    * FOV (Field of view)
    * Full-Screen Mode
    * V-Sync



## [1.0.0] Regular Update
* Initial Release
