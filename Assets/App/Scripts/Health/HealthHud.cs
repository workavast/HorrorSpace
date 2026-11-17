using System;
using UnityEngine;
using VContainer;

namespace App.Health
{
    public class HealthHud : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float showSpeed;
        [SerializeField] private float hideSpeed;

        private float _targetValue;
        
        [Inject] private readonly HealthContext _healthContext; 
        
        private void OnEnable()
        {
            _healthContext.OnMaxHealthChanged += UpdateTargetValue;
            _healthContext.OnHealthChanged += UpdateTargetValue;
            canvasGroup.alpha = 0;
            
            SetTargetValue();
        }

        private void OnDisable()
        {
            _healthContext.OnMaxHealthChanged -= UpdateTargetValue;
            _healthContext.OnHealthChanged -= UpdateTargetValue;
        }

        private void Update()
        {
            if (Mathf.Approximately(canvasGroup.alpha, _targetValue))
                return;

            if (_targetValue >= canvasGroup.alpha)
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, _targetValue, showSpeed * Time.deltaTime);
            else
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, _targetValue, hideSpeed * Time.deltaTime);
        }

        private void UpdateTargetValue(float _) 
            => SetTargetValue();

        private void SetTargetValue()
        {
            var rounded = (float)Math.Round(1 - _healthContext.FillPercentage, 1); // → 3.1f
            SetTargetValue(rounded);
        }

        private void SetTargetValue(float healthFillPercentage) 
            => _targetValue = Mathf.Clamp01(healthFillPercentage);
    }
}