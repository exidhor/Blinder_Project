using MapEditor;
using Tools;
using UnityEngine;

namespace BlinderProject
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public GameObject player
        {
            get { return _player; }
        }

        [SerializeField] private GameObject _player;

        void Awake()
        {
            Map.instance.CreateNavGrid();   
        }
    }
}
