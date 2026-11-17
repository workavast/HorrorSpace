using System;

namespace App.Health
{
    public class HealthContext
    {
        public float MaxHealth { get; private set; }
        public float Health { get; set; }
        public float FillPercentage => Health / MaxHealth;
        public bool IsAlive => Health > 0;
        
        public event Action<float> OnMaxHealthChanged;
        public event Action<float> OnHealthChanged;

        public void SetAllHealth(float maxValue, float currentValue)
        {
            MaxHealth = maxValue;
            Health = currentValue;
            OnMaxHealthChanged?.Invoke(MaxHealth);
            OnHealthChanged?.Invoke(Health);
        }
        
        public void SetMaxHealth(float value)
        {
            MaxHealth = value;
            OnMaxHealthChanged?.Invoke(MaxHealth);
        }

        public void SetHealth(float value)
        {
            Health = value;
            OnHealthChanged?.Invoke(Health);
        }
    }
}