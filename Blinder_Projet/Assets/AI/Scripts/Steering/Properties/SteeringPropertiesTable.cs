using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace AI
{
    public class SteeringPropertiesTable : MonoSingleton<SteeringPropertiesTable>
    {
        public List<SteeringProperties> _steeringPropertiesList = new List<SteeringProperties>();

        private Dictionary<string, SteeringProperties> _steeringPropertiesTable = new Dictionary<string, SteeringProperties>();

        void Awake()
        {
            ConstructTable();
        }

        private void ConstructTable()
        {
            _steeringPropertiesTable.Clear();

            for (int i = 0; i < _steeringPropertiesList.Count; i++)
            {
                _steeringPropertiesTable.Add(_steeringPropertiesList[i].name, _steeringPropertiesList[i]);
            }
        }

        public SteeringProperties GetProperties(string key)
        {
            if (_steeringPropertiesTable.ContainsKey(key))
            {
                return _steeringPropertiesTable[key];
            }
            
            return null;
        }
    }
}