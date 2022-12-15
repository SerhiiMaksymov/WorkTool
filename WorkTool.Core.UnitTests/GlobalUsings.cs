global using NUnit.Framework;

global using System.Net;
global using System.Net.Http.Json;
global using System.Text;
global using System.Diagnostics;

global using FluentAssertions;

global using Moq;
global using Moq.Contrib.HttpClient;

global using WorkTool.Core.Modules.Common.Services;
global using WorkTool.Core.Modules.DependencyInjection.Exceptions;
global using WorkTool.Core.Modules.DependencyInjection.Models;
global using WorkTool.Core.Modules.SmsClub.Excpetions;
global using WorkTool.Core.Modules.DependencyInjection.Services;
global using WorkTool.Core.Modules.SmsClub.Models;
global using WorkTool.Core.Modules.Http.Exceptions;
global using WorkTool.Core.Modules.SmsClub.Helpers;
global using WorkTool.Core.Modules.SmsClub.Services;
global using WorkTool.Core.Modules.Common.Extensions;
global using WorkTool.Core.Modules.Common.Interfaces;
global using WorkTool.Core.Modules.Http.Extensions;

global using HttpConsts = WorkTool.Core.Modules.Http.Helpers.Consts;
