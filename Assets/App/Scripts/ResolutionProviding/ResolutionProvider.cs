using System;
using UnityEngine;

namespace App.ResolutionProviding
{
    public class ResolutionProvider : MonoBehaviour, IResolutionProviderRO
    {
        public Vector2Int Resolution { get; private set; }

        public event Action OnResolutionChanged;

        private void Start()
        {
            Resolution = GetResolution();
        }

        private void Update()
        {
            var prevResolution = Resolution;
            Resolution = GetResolution();
           
            if (prevResolution != Resolution)
                OnResolutionChanged?.Invoke();
        }

        private static Vector2Int GetResolution() 
            => new(Screen.width, Screen.height);
    }
}