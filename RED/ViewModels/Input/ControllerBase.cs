using Caliburn.Micro;
using System.Collections.Generic;

namespace RED.ViewModels.Input
{
    public abstract class ControllerBase : PropertyChangedBase
    {
        protected Dictionary<string, bool> DebounceStates;

        protected void InitializeDebounce(IEnumerable<string> keys)
        {
            DebounceStates = new Dictionary<string, bool>();
            foreach (var item in keys)
                DebounceStates.Add(item, false);
        }
        protected float Debounce(string key, bool newState)
        {
            if (!DebounceStates[key] && newState)
            {
                DebounceStates[key] = true;
                return 1f;
            }
            else if (DebounceStates[key] && !newState)
            {
                DebounceStates[key] = false;
                return 0f;
            }
            else
            {
                return 0f;
            }
        }

        static public float TwoButtonToggleDirection(bool directionButtonInput, float analogButtonInput)
        {
            if (directionButtonInput)
            {
                return -1 * analogButtonInput;
            }
            else
            {
                return analogButtonInput;
            }
        }

        static public float DeadzoneTransform(int x, int deadzone)
        {
            return (x < deadzone && x > -deadzone) ? 0f : (x + (x < 0 ? deadzone : -deadzone)) / (float)(32768 - deadzone);
        }
        static public T TwoButtonTransform<T>(bool bool1, bool bool2, T val1, T val2, T val0)
        {
            return bool1 ? val1 : (bool2 ? val2 : val0);
        }
    }
}
