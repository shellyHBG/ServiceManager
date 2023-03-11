namespace Game.Service
{
    public interface IServiceManager
    {
        void OnEnterGame();
        void OnLeaveGame();
        void OnSceneChange();
        void OnSceneChanged();
    }
}