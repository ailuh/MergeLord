using System.Collections.Generic;
using Configs.Objects;

namespace Code.Game.Configs.Interfaces
{
    public interface IObjectInfoChainProvider
    {
        List<GridObjectConfigBase> GetUpgradeChain();
    }
}