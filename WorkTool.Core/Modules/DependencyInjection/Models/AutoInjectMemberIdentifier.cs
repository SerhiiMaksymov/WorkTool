namespace WorkTool.Core.Modules.DependencyInjection.Models;

public readonly record struct AutoInjectMemberIdentifier(
    TypeInformation Type,
    AutoInjectMember Member
);
