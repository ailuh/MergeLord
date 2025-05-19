namespace Code.Application.Interfaces
{
    public interface IGameMessageService
    {
        public void ShowMessage(string message, bool isHasLvl = false);
    }
}