using System.Reflection;

using FlatPlayerPlus;

using MelonLoader;

#region MelonLoader

[assembly: MelonInfo(typeof(Mod), "FlatPlayerPlus", Mod.Version, "HL2H0", "https://thunderstore.io/c/bonelab/p/HL2H0/FlatPlayerPlus/")]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]
[assembly: MelonOptionalDependencies("RagdollPlayer")]

#endregion MelonLoader

#region Info

[assembly: AssemblyTitle("")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("HL2H0")]
[assembly: AssemblyProduct("FlatPlayerPlus")]
[assembly: AssemblyCulture("")]

#endregion Info

#region Version

[assembly: AssemblyVersion(Mod.Version)]
[assembly: AssemblyFileVersion(Mod.Version)]
[assembly: AssemblyInformationalVersion(Mod.Version)]

#endregion Version