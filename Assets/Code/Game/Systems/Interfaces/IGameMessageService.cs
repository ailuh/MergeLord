using Code.Game.Configs.Monsters;
using Configs.Objects;

namespace Code.Game.Systems.Interfaces
{
    public interface IGameMessageService
    {
        public void ShowMessage(string message, bool isHasLvl = false, GridObjectConfigBase? objectConfig = null);
    }
}