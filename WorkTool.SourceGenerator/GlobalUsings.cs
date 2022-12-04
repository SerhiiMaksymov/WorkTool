global using System.Collections;
global using System.Data;
global using System.Net;
global using System.Net.Sockets;
global using System.Reflection;
global using System.Runtime.CompilerServices;
global using System.Text;
global using System.Text.RegularExpressions;
global using System.Diagnostics.Contracts;

global using Microsoft.CodeAnalysis;
global using Microsoft.CodeAnalysis.CSharp;
global using Microsoft.CodeAnalysis.CSharp.Syntax;

global using WorkTool.Core.Modules.Common.Exceptions;
global using WorkTool.Core.Modules.Common.Extensions;
global using WorkTool.Core.Modules.Common.Interfaces;
global using WorkTool.Core.Modules.Common.Models;
global using WorkTool.Core.Modules.Common.Services;
global using WorkTool.SourceGenerator.Core.Attributes;
global using WorkTool.SourceGenerator.Extensions;
global using WorkTool.SourceGenerator.Helpers;
global using WorkTool.SourceGenerator.Models;
global using WorkTool.SourceGenerator.Receivers;
global using WorkTool.SourceGenerator.Core.Models;

global using CodeAnalysisTypeInfo = Microsoft.CodeAnalysis.TypeInfo;