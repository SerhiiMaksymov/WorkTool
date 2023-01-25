namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IExpressionCache
{
    void CacheExpression(TypeInformation type);
    Expression? GetCacheExpression(TypeInformation type);
}
