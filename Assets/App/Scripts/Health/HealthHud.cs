using System;
using UnityEngine;

namespace App.Health
{
    public class HealthHud : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private HealthModel healthModel;
        [SerializeField] private float showSpeed;
        [SerializeField] private float hideSpeed;

        private float _targetValue;
        
        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            healthModel.OnHealthChanged += SetTargetValue;
            canvasGroup.alpha = 0;
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

        private void SetTargetValue()
        {
            var rounded = (float)Math.Round(1 - healthModel.FillPercentage, 1); // → 3.1f
            SetTargetValue(rounded);
        }

        private void SetTargetValue(float healthFillPercentage) 
            => _targetValue = Mathf.Clamp01(healthFillPercentage);
    }
}