using LitMotion;
using UnityEngine;

namespace App.Doors
{
    public class DoorController : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private Transform leftPanel;
        [SerializeField] private Transform rightPanel;

        [Header("Open settings")]
        [Tooltip("Local offset applied to left panel when door is opened")]
        [SerializeField] private Vector3 leftOpenOffset = new Vector3(-1f, 0f, 0f);
        [Tooltip("Local offset applied to right panel when door is opened")]
        [SerializeField] private Vector3 rightOpenOffset = new Vector3(1f, 0f, 0f);
        [SerializeField] private float animationDuration = 0.5f;

        [Header("Easing")]
        [SerializeField] private Ease openEase = Ease.OutQuad;
        [SerializeField] private Ease closeEase = Ease.OutQuad;

        // internal handles to cancel/inspect active motions
        private MotionHandle? _leftHandle;
        private MotionHandle? _rightHandle;

        private Vector3 _leftClosedLocal;
        private Vector3 _rightClosedLocal;
        private bool _isOpen;

        private void Awake()
        {
            if (leftPanel != null) 
                _leftClosedLocal = leftPanel.localPosition;
            if (rightPanel != null) 
                _rightClosedLocal = rightPanel.localPosition;
        }

        public void Open()
        {
            if (_isOpen) return;
            _isOpen = true;
            if (leftPanel != null)
            {
                StartPanelMotion(ref _leftHandle, leftPanel, leftPanel.localPosition, _leftClosedLocal + leftOpenOffset, animationDuration, openEase);
            }

            if (rightPanel != null)
            {
                StartPanelMotion(ref _rightHandle, rightPanel, rightPanel.localPosition, _rightClosedLocal + rightOpenOffset, animationDuration, openEase);
            }
        }

        public void Close()
        {
            if (!_isOpen) 
                return;
            
            _isOpen = false;
            if (leftPanel != null) 
                StartPanelMotion(ref _leftHandle, leftPanel, leftPanel.localPosition, _leftClosedLocal, animationDuration, closeEase);

            if (rightPanel != null) 
                StartPanelMotion(ref _rightHandle, rightPanel, rightPanel.localPosition, _rightClosedLocal, animationDuration, closeEase);
        }

        public void Toggle()
        {
            if (_isOpen) 
                Close(); 
            else 
                Open();
        }

        private void StartPanelMotion(ref MotionHandle? handleRef, Transform panel, Vector3 from, Vector3 to, float baseDuration, Ease ease)
        {
            // Cancel previous
            if (handleRef.HasValue)
            {
                var handle = handleRef.Value;
                if (handle.IsActive()) 
                    handle.Cancel();
            }

            var totalDist = Vector3.Distance(from, to);
            if (totalDist <= 0.0001f)
            {
                panel.localPosition = to;
                handleRef = null;
                return;
            }

            var current = panel.localPosition;
            var remaining = Vector3.Distance(current, to);
            var duration = Mathf.Clamp01(remaining / totalDist) * baseDuration;

            // Ensure minimum tiny duration to avoid zero-duration motions causing issues
            if (duration <= 0f)
            {
                panel.localPosition = to;
                handleRef = null;
                return;
            }

            var motion = LMotion
                .Create(current, to, duration)
                .WithEase(ease)
                .Bind(v => panel.localPosition = v);

            handleRef = motion;
        }
    }
}
