using System;
using UnityEngine;

namespace Avastrad.Storages
{
    [Serializable]
    public class FloatStorage : StorageBase<float>
    {
        public override float FillingPercentage => currentValue / maxValue;
        public override bool IsFull => currentValue >= maxValue;
        public override bool IsEmpty => currentValue <= minValue;

        public override event Action OnChange;
        public override event Action<float> OnMaxValueChange;
        public override event Action<float> OnCurrentValueChange;
        public override event Action<float> OnMinValueChange;

        public FloatStorage(float maxValue = 0, float currentValue = 0, float minValue = 0)
        {
            if (currentValue > maxValue)
                currentValue = maxValue;

            if (minValue > maxValue)
                minValue = maxValue;

            if (minValue > currentValue)
                minValue = currentValue;
            
            this.maxValue = maxValue;
            this.currentValue = currentValue;
            this.minValue = minValue;
        }
    
        public override void SetMaxValue(float newMaxValue)
        {
            maxValue = newMaxValue;
            minValue = Mathf.Clamp(minValue, int.MinValue, maxValue);
            currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
            OnMaxValueChange?.Invoke(maxValue);
            OnChange?.Invoke();
        }

        public override void SetCurrentValue(float newCurrentValue)
        {
            currentValue = Mathf.Clamp(newCurrentValue, minValue, maxValue);
            OnCurrentValueChange?.Invoke(currentValue);
            OnChange?.Invoke();
        }

        public override void SetMinValue(float newMinValue)
        {
            minValue = newMinValue;
            maxValue = Mathf.Clamp(maxValue, minValue, int.MaxValue);
            currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
            OnMinValueChange?.Invoke(minValue);
            OnChange?.Invoke();
        }
    
        public override void ChangeCurrentValue(float value)
        {
            currentValue = Mathf.Clamp(currentValue + value, minValue, maxValue);
            OnCurrentValueChange?.Invoke(currentValue);
            OnChange?.Invoke();
        }
    }
}