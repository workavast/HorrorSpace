using System;
using LitMotion;
using UnityEngine;

namespace App.Doors
{
    public class MoveAnimation : MonoBehaviour
    {
        [SerializeField] private Transform objToMove;
        [SerializeField] private Vector3 moveOffset = new(1f, 0f, 0f);
        [SerializeField] private float animationDuration = 0.5f;
        [SerializeField] private Ease toMoveEasing = Ease.OutQuad;
        [SerializeField] private Ease backMoveEasing = Ease.OutQuad;

        public bool InProcess { get; private set; }

        private MotionHandle? _rightHandle;
        private Vector3 _initialPosition;
        private bool _inInitialPosition;

        public event Action OnAnimationEnd;
        
        private void Awake()
        {
            _inInitialPosition = true;
            _initialPosition = objToMove.localPosition;
        }

        public void Move()
        {
            if (!_inInitialPosition) 
                return;
            
            _inInitialPosition = false;
            if (objToMove != null)
                StartPanelMotion(ref _rightHandle, objToMove, objToMove.localPosition, _initialPosition + moveOffset, animationDuration, toMoveEasing);
        }

        public void Back()
        {
            if (_inInitialPosition) 
                return;
            
            _inInitialPosition = true;
            if (objToMove != null) 
                StartPanelMotion(ref _rightHandle, objToMove, objToMove.localPosition, _initialPosition, animationDuration, backMoveEasing);
        }
        
        public void SetState(bool toPosition)
        {
            if (toPosition) 
                Move(); 
            else 
                Back();
        }

        public void Toggle()
        {
            if (_inInitialPosition) 
                Move();
            else 
                Back(); 
        }

        private void StartPanelMotion(ref MotionHandle? handleRef, Transform obj, Vector3 from, Vector3 to, float baseDuration, Ease ease)
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
                obj.localPosition = to;
                handleRef = null;
                InProcess = false;
                return;
            }

            var current = obj.localPosition;
            var remaining = Vector3.Distance(current, to);
            var duration = Mathf.Clamp01(remaining / totalDist) * baseDuration;

            // Ensure minimum tiny duration to avoid zero-duration motions causing issues
            if (duration <= 0f)
            {
                obj.localPosition = to;
                handleRef = null;
                InProcess = false;
                return;
            }

            InProcess = true;
            var motion = LMotion
                .Create(current, to, duration)
                .WithEase(ease)
                .WithOnComplete(AnimationEnd)
                .Bind(v => obj.localPosition = v);

            handleRef = motion;
        }

        private void AnimationEnd()
        {
            InProcess = false;
            OnAnimationEnd?.Invoke();
        }
    }
}
