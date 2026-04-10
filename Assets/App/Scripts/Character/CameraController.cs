using App.Health;
using UnityEngine;
using Unity.Cinemachine;

namespace App.Character
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private HealthModel healthComp;
        [SerializeField] private CharMover charMover;
        [SerializeField] private Transform characterRoot;
        [SerializeField] private Transform cameraRig;
        [SerializeField] private CinemachineCamera virtualCamera;
        [SerializeField] private PlayerInputManager inputManager;
        [SerializeField] private float lookSensitivity = 1f;
        [SerializeField] private float minPitch = -90f;
        [SerializeField] private float maxPitch = 90f;
        [SerializeField] private float cameraHeightSpeed = 5f;

        private float _currentPitch;
        private float _currentCameraHeight;
        private float _targetCameraHeight;

        private void OnEnable()
        {
            if (virtualCamera == null)
                virtualCamera = GetComponentInChildren<CinemachineCamera>();

            if (cameraRig == null)
                cameraRig = virtualCamera.transform;

            _currentCameraHeight = cameraRig.localPosition.y;
        }

        private void Update()
        {
            if (!healthComp.IsAlive)
                return;
            
            HandleCameraRotation();
            HandleCameraHeight();
        }

        private void HandleCameraRotation()
        {
            if (!_cameraRotationEnabled)
            {
                charMover.SetLookDeltaRotation(new Vector3(0, 0, 0));
                return;
            }

            var lookDelta = inputManager.LookInput;

            if (lookDelta.magnitude < 0.01f)
            {
                charMover.SetLookDeltaRotation(new Vector3(0, 0, 0));
                return;
            }

            // Pitch - поворот вверх/вниз (X ось)
            _currentPitch -= lookDelta.y * lookSensitivity;
            _currentPitch = Mathf.Clamp(_currentPitch, minPitch, maxPitch);

            // Применяем pitch к камере (X ось локальная)
            cameraRig.localRotation = Quaternion.Euler(_currentPitch, 0, 0);
            charMover.SetLookDeltaRotation(new Vector3(0, lookDelta.x * lookSensitivity, 0));
        }

        private void HandleCameraHeight()
        {
            // Плавно изменяем высоту
            _currentCameraHeight =
                Mathf.Lerp(_currentCameraHeight, _targetCameraHeight, Time.deltaTime * cameraHeightSpeed);

            // Применяем смещение к локальной позиции камеры
            var localPosition = cameraRig.localPosition;
            localPosition.y = _currentCameraHeight;
            cameraRig.localPosition = localPosition;
        }

        public void SetTargetHeight(float height)
            => _targetCameraHeight = height;

        public void SetLookSensitivity(float sensitivity)
            => lookSensitivity = Mathf.Max(0.1f, sensitivity);

        public void SetPitchLimits(float minValue, float maxValue)
        {
            minPitch = minValue;
            maxPitch = maxValue;
            _currentPitch = Mathf.Clamp(_currentPitch, minPitch, maxPitch);
        }

        public float GetCurrentPitch()
            => _currentPitch;

        public void ResetRotation()
        {
            _currentPitch = 0f;
            cameraRig.localRotation = Quaternion.identity;
        }

        public void SetCameraHeightSpeed(float speed)
            => cameraHeightSpeed = Mathf.Max(0.1f, speed);

        public Camera GetCamera()
        {
            return Camera.main;
        }

        public void SetCameraRotationEnabled(bool enabled)
        {
            // We'll modify HandleCameraRotation to check this flag
            _cameraRotationEnabled = enabled;
        }

        private bool _cameraRotationEnabled = true;
    }
}
