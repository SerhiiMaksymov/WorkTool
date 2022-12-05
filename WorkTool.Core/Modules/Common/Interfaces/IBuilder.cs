namespace WorkTool.Core.Modules.Common.Interfaces;

public interface IBuilder<out TBuilding>
{
    TBuilding Build();
}

public interface IBuilder<out TBuilding, in TOptions>
{
    TBuilding Build(TOptions options);
}
