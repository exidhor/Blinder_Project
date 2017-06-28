using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace AI
{
    public class SteeringSpecsTable : MonoSingleton<SteeringSpecsTable>
    {
        public List<SteeringSpecs> _steeringSpecsList = new List<SteeringSpecs>();

        private Dictionary<string, SteeringSpecs> _steeringSpecsTable = new Dictionary<string, SteeringSpecs>();

        void Awake()
        {
            ConstructTable();
        }

        private void ConstructTable()
        {
            _steeringSpecsTable.Clear();

            for (int i = 0; i < _steeringSpecsList.Count; i++)
            {
                _steeringSpecsTable.Add(_steeringSpecsList[i].name, _steeringSpecsList[i]);
            }
        }

        public SteeringSpecs GetSpecs(string key)
        {
            if (_steeringSpecsTable.ContainsKey(key))
            {
                return _steeringSpecsTable[key];
            }
            
            return null;
        }
    }
}