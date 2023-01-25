namespace WorkTool.Core.Modules.DependencyInjection.Models;

public readonly record struct AutoInjectIdentifier(TypeInformation Type, AutoInjectMember Member);
