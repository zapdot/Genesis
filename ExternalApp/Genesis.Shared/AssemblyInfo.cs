﻿using System.Reflection;
using System.Runtime.CompilerServices;

// External Assemblies
[assembly: InternalsVisibleTo("Genesis.CLI")]
[assembly: InternalsVisibleTo("Genesis.Plugin")]
[assembly: InternalsVisibleTo("Genesis.Plugin.Tests")]

// Unity Assemblies
[assembly: InternalsVisibleTo("Genesis.Editor")]
[assembly: InternalsVisibleTo("Genesis.Editor.Tests")]

[assembly: AssemblyVersion("2.0.4.0")]
[assembly: AssemblyFileVersion("2.0.4.0")]
[assembly: AssemblyInformationalVersion("2.0.4+Branch.feat-v2.Sha.eb4b39a06495c3d84ad76f7e07ab01cbf8d051f9")]