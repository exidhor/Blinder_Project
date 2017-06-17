using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
