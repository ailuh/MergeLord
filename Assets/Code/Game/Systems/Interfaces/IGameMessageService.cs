using Code.Game.Configs.Monsters;

namespace Code.Game.Systems.Interfaces
{
    public interface IGameMessageService
    {
        public void ShowMessage(string message, bool isHasLvl = false, ObjectRef? objectRef = null);
    }
}