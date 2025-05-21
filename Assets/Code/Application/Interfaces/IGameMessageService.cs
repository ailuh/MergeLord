using Code.Infrastructure.Configs.Monsters;

namespace Code.Application.Interfaces
{
    public interface IGameMessageService
    {
        public void ShowMessage(string message, bool isHasLvl = false, ObjectRef? objectRef = null);
    }
}