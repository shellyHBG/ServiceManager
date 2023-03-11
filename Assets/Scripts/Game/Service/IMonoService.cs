
namespace Game.Service
{
    public interface IMonoUpdatable
    {
        void OnUpdate();
    }

    public interface IMonoFixedUpdatable
    {
        void OnFixedUpdate();
    }

    public interface IMonoLateUpdatable
    {
        void OnLateUpdate();
    }
}