
namespace GameFlow.GameState
{
    public interface IGameState 
    {
        void Construct();
        void Destruct();
        void UpdateState();
    }
}
