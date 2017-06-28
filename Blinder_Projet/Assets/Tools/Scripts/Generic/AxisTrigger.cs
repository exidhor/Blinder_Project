
namespace Tools
{
    /// <summary>
    /// Store a float, and a trigger flag when this float is modified
    /// </summary>
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
        
        /// <summary>
        /// Reset the trigger mecanism, 
        /// and set the value of the axis to 0 
        /// </summary>
        public void Reset()
        {
            _axis = 0f;
            _isChecked = false;
        }
    }
}