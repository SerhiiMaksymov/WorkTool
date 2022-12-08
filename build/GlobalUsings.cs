extern alias NC;

global using System.ComponentModel;
global using System;
global using System.Text;
global using System.Runtime.CompilerServices;
global using System.IO;
global using System.Threading.Tasks;
global using System.Collections.Generic;
global using System.Collections;
global using System.Data;
global using System.Linq;
global using System.Reflection;
global using System.Linq.Expressions;

global using LibGit2Sharp;

global using CliWrap;

global using Serilog;

global using NC.Nuke.Common;
global using NC.Nuke.Common.ProjectModel;
global using NC.Nuke.Common.Tooling;
global using NC.Nuke.Common.Tools.DotNet;
global using NC.Nuke.Common.Tools.GitVersion;

global using JetBrains.Annotations;

global using WorkTool.Core.Modules.Common.Exceptions;
global using WorkTool.Core.Modules.Common.Extensions;
global using WorkTool.Core.Modules.Git.Exceptions;
global using WorkTool.Core.Modules.LibGit2Sharp.Helpers;
global using WorkTool.Core.Modules.LibGit2Sharp.Extensions;

global using static NC.Nuke.Common.Tools.DotNet.DotNetTasks;
