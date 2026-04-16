using System;
using UnityEngine;

namespace App.Doors
{
    public class DoorAnimator : MonoBehaviour
    {
        [SerializeField] private MoveAnimation[] animations;

        public event Action OnAnimationEnd;
        public event Action OnOpenStarted;

        private void Awake()
        {
            foreach (var anim in animations)
                anim.OnAnimationEnd += CheckAnimations;
        }

        private void OnDestroy()
        {
            foreach (var anim in animations)
                anim.OnAnimationEnd -= CheckAnimations;
        }

        public void SetOpen(bool open)
        {
            if (open)
            {
                foreach (var anim in animations)
                    anim.Move();
                OnOpenStarted?.Invoke();
            }
            else
                foreach (var anim in animations)
                    anim.Back();
        }

        private void CheckAnimations()
        {
            foreach (var anim in animations)
                if (anim.InProcess)
                    return;

            OnAnimationEnd?.Invoke();
        }
    }
}
