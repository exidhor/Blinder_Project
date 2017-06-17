using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public class AxisTrigger
    {
        public static implicit operator float(AxisTrigger axisTrigger)
        {
            return axisTrigger.axis;
        }

        public float axis
        {
            get
            {
                _isChecked = true;
                return _axis;
            }

            set
            {
                if (_axis == 0f || _isChecked)
                {
                    _axis = value;
                    _isChecked = false;
                }
            }
        }

        private float _axis = 0f;
        private bool _isChecked = false;

        public void Reset()
        {
            _axis = 0f;
            _isChecked = false;
        }
    }
}