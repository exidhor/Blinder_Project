using MapEditor;
using Tools;

namespace BlinderProject
{
    public class GameManager : MonoSingleton<GameManager>
    {
        void Start()
        {
            Map.instance.CreateNavGrid();   
        }
    }
}
