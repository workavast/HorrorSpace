using UnityEngine;
using Unity.Cinemachine;

namespace App.Character
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform characterRoot;
        [SerializeField] private Transform cameraRig;
        [SerializeField] private CinemachineCamera virtualCamera;
        [SerializeField] private PlayerInputManager inputManager;
        [SerializeField] private PlayerCharacterController characterController;
        [SerializeField] private float lookSensitivity = 1f;
        [SerializeField] private float minPitch = -90f;
        [SerializeField] private float maxPitch = 90f;
        [SerializeField] private float normalCameraHeight = 0.6f;
        [SerializeField] private float crouchCameraHeight = 0.2f;
        [SerializeField] private float cameraHeightSpeed = 5f;

        private float _currentPitch = 0f;
        private float _currentYaw = 0f;
        private float _currentCameraHeight = 0f;

        private void OnEnable()
        {
            if (virtualCamera == null)
            {
                virtualCamera = GetComponentInChildren<CinemachineCamera>();
            }

            if (cameraRig == null && virtualCamera != null)
            {
                cameraRig = virtualCamera.transform;
            }

            if (characterController == null)
            {
                characterController = GetComponent<PlayerCharacterController>();
                if (characterController == null)
                {
                    characterController = transform.parent?.GetComponent<PlayerCharacterController>();
                }
            }

            _currentCameraHeight = normalCameraHeight;
        }

        private void Update()
        {
            if (inputManager == null)
                return;

            HandleCameraRotation();
            HandleCameraHeight();
        }

        private void HandleCameraRotation()
        {
            Vector2 lookDelta = inputManager.LookInput;

            if (lookDelta.magnitude < 0.01f)
                return;

            // Pitch - поворот вверх/вниз (X ось)
            _currentPitch -= lookDelta.y * lookSensitivity;
            _currentPitch = Mathf.Clamp(_currentPitch, minPitch, maxPitch);

            // Yaw - поворот влево/вправо (Y ось)
            _currentYaw += lookDelta.x * lookSensitivity;

            // Применяем pitch к камере (X ось локальная)
            if (cameraRig != null)
            {
                cameraRig.localRotation = Quaternion.Euler(_currentPitch, 0, 0);
            }

            // Применяем yaw к персонажу (Y ось глобальная)
            characterRoot.Rotate(0, lookDelta.x * lookSensitivity, 0, Space.World);
        }

        private void HandleCameraHeight()
        {
            if (characterController == null || cameraRig == null)
                return;

            // Определяем целевую высоту камеры
            float targetHeight = characterController.IsCrouching ? crouchCameraHeight : normalCameraHeight;

            // Плавно изменяем высоту
            _currentCameraHeight = Mathf.Lerp(_currentCameraHeight, targetHeight, Time.deltaTime * cameraHeightSpeed);

            // Применяем смещение к локальной позиции камеры
            Vector3 localPosition = cameraRig.localPosition;
            localPosition.y = _currentCameraHeight;
            cameraRig.localPosition = localPosition;
        }

        public void SetLookSensitivity(float sensitivity)
        {
            lookSensitivity = Mathf.Max(0.1f, sensitivity);
        }

        public void SetPitchLimits(float minValue, float maxValue)
        {
            minPitch = minValue;
            maxPitch = maxValue;
            _currentPitch = Mathf.Clamp(_currentPitch, minPitch, maxPitch);
        }

        public float GetCurrentPitch()
        {
            return _currentPitch;
        }

        public float GetCurrentYaw()
        {
            return _currentYaw;
        }

        public void ResetRotation()
        {
            _currentPitch = 0f;
            _currentYaw = 0f;
            if (cameraRig != null)
            {
                cameraRig.localRotation = Quaternion.identity;
            }
        }

        public void SetCameraHeights(float normalHeight, float crouchHeight)
        {
            normalCameraHeight = normalHeight;
            crouchCameraHeight = crouchHeight;
        }

        public void SetCameraHeightSpeed(float speed)
        {
            cameraHeightSpeed = Mathf.Max(0.1f, speed);
        }
    }
}
