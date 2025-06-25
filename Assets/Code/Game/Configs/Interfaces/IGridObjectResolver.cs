using Configs.Objects;

namespace Code.Game.Configs.Interfaces
{
    public interface IGridObjectResolver
    {
        GridObjectConfigBase? Resolve(string id);
    }
}