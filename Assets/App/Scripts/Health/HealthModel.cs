using System;
using App.Utils.ReadOnlyAttributeDrawing;
using UnityEngine;
using VContainer;

namespace App.Health
{
    public class HealthModel : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField, ReadOnly]  private float health;

        [Inject] private readonly HealthContext _healthContext;
        
        public float MaxHealth => maxHealth;
        public float Health { get => health; private set => health = value; }
        public float FillPercentage => Health / MaxHealth;
        public bool IsAlive => Health > 0;
        
        public event Action OnTakeDamage;
        public event Action OnHealthChanged;

        public void Initialize()
        {
            Heal(MaxHealth, true);
            _healthContext.SetAllHealth(MaxHealth, Health);
        }

        public void TakeDamage(float damage)
        {
            if (Health <= 0)
                return;
            
            Health = Mathf.Clamp(Health - damage, 0f, MaxHealth);
            _healthContext.SetHealth(Health);
            OnHealthChanged?.Invoke();
            OnTakeDamage?.Invoke();
        }

        public void Heal(float healAmount, bool force = false)
        {
            if (Health <= 0 && !force)
                return;
            
            Health = Mathf.Clamp(Health + healAmount, 0f, MaxHealth);
            _healthContext.SetHealth(Health);
            OnHealthChanged?.Invoke();
        }
        
        public bool CanBeDamaged() 
            => IsAlive;
    }
}