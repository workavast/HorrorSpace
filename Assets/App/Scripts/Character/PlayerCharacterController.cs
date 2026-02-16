using UnityEngine;

namespace App.Character
{
    public class PlayerCharacterController : MonoBehaviour
    {
        [SerializeField] private PlayerInputManager inputManager;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float sprintSpeed = 8f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 10f;
        [SerializeField] private float groundDrag = 5f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float gravity = 9.81f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckDistance = 0.1f;
        [SerializeField] private float crouchHeight = 0.6f;
        [SerializeField] private float crouchSpeed = 2f;
        [SerializeField] private float crouchHeightSpeed = 5f;
        [SerializeField] private float headCollisionDistance = 0.3f;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private float standCheckDistance = 0.2f;
        [SerializeField] private float standCheckOffset = -0.1f;

        private float _normalHeight;
        private Vector3 _velocity = Vector3.zero;
        private Vector3 _jumpHorizontalVelocity = Vector3.zero;
        private bool _isInAirJump = false;
        private bool _isGrounded = false;
        private bool _isCrouching = false;
        private Animator _animator;

        private readonly int _animatorSpeedHash = Animator.StringToHash("Speed");
        private readonly int _animatorIsGroundedHash = Animator.StringToHash("IsGrounded");
        private readonly int _animatorIsCrouchingHash = Animator.StringToHash("IsCrouching");

        private void OnEnable()
        {
            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }

            _animator = GetComponentInChildren<Animator>();
            
            if (characterController != null)
            {
                _normalHeight = characterController.height;
            }
        }

        private void Update()
        {
            if (characterController == null || inputManager == null)
                return;

            CheckGround();
            HandleCrouch();
            HandleMovement();
            CheckHeadCollision();
            HandleGravity();
            ApplyMovement();
            UpdateAnimator();
        }

        private void CheckGround()
        {
            _isGrounded = Physics.Raycast(
                transform.position + Vector3.up * characterController.height * 0.5f,
                Vector3.down,
                characterController.height * 0.5f + groundCheckDistance,
                groundLayer
            );

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -0.1f; // Малое значение для сохранения контакта с землёй
            }
            // Если касаемся земли — заканчиваем фазу прыжка с инерцией
            if (_isGrounded)
            {
                _isInAirJump = false;
                _jumpHorizontalVelocity = Vector3.zero;
            }
        }

        private void CheckHeadCollision()
        {
            // Проверяем удар головой только во время прыжка (когда скорость вверх положительна)
            if (_velocity.y <= 0)
                return;

            // Позиция верхушки головы
            Vector3 headPosition = transform.position + Vector3.up * characterController.height;
            
            // Проверяем коллизию выше персонажа
            if (Physics.Raycast(headPosition, Vector3.up, headCollisionDistance, obstacleLayer))
            {
                // При ударе головой останавливаем восхождение
                _velocity.y = -0.1f;
            }
        }

        private void HandleMovement()
        {
            // Если мы в фазе прыжка с сохранением инерции — фиксируем горизонтальную скорость
            if (!_isGrounded && _isInAirJump)
            {
                _velocity.x = _jumpHorizontalVelocity.x;
                _velocity.z = _jumpHorizontalVelocity.z;
                // В воздухе во время прыжка игрок не меняет направление/скорость движения
                return;
            }

            Vector2 moveInput = inputManager.MoveInput;
            bool isSprinting = inputManager.IsSprintPressed && _isGrounded && !_isCrouching;

            float targetSpeed = isSprinting ? sprintSpeed : moveSpeed;
            
            // Скорость меньше при приседании
            if (_isCrouching)
            {
                targetSpeed = crouchSpeed;
            }
            
            targetSpeed *= moveInput.magnitude;

            // Плавное изменение скорости
            float currentHorizontalSpeed = new Vector3(_velocity.x, 0, _velocity.z).magnitude;
            
            if (moveInput.magnitude > 0)
            {
                currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * acceleration);
            }
            else
            {
                currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, 0, Time.deltaTime * deceleration);
            }

            // Направление движения относительно персонажа
            Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
            
            _velocity.x = moveDirection.x * currentHorizontalSpeed;
            _velocity.z = moveDirection.z * currentHorizontalSpeed;

            // Прыжок
            if (_isGrounded && inputManager.JumpPressedThisFrame)
            {
                Jump();
            }
        }

        private void Jump()
        {
            // Фиксируем горизонтальную скорость в момент старта прыжка
            _jumpHorizontalVelocity = new Vector3(_velocity.x, 0f, _velocity.z);
            _isInAirJump = true;

            _velocity.y = jumpForce;
            _isGrounded = false;
        }

        private void HandleGravity()
        {
            if (!_isGrounded)
            {
                _velocity.y -= gravity * Time.deltaTime;
            }
        }

        private void ApplyMovement()
        {
            if (characterController.enabled)
            {
                characterController.Move(_velocity * Time.deltaTime);
            }
        }

        private void UpdateAnimator()
        {
            if (_animator == null)
                return;

            float currentSpeed = new Vector3(_velocity.x, 0, _velocity.z).magnitude;
            _animator.SetFloat(_animatorSpeedHash, currentSpeed);
            _animator.SetBool(_animatorIsGroundedHash, _isGrounded);
            _animator.SetBool(_animatorIsCrouchingHash, _isCrouching);
        }

        private void HandleCrouch()
        {
            bool wantsToCrouch = inputManager.IsCrouchPressed;
            
            if (wantsToCrouch && !_isCrouching)
            {
                // Начинаем приседание
                _isCrouching = true;
            }
            else if (!wantsToCrouch && _isCrouching)
            {
                // Проверяем, можно ли встать
                if (CanStand())
                {
                    // Встаём только если достаточно места
                    _isCrouching = false;
                }
                // Иначе остаемся в приседе
            }

            // Плавно изменяем высоту персонажа
            float targetHeight = _isCrouching ? crouchHeight : _normalHeight;
            characterController.height = Mathf.Lerp(characterController.height, targetHeight, Time.deltaTime * crouchHeightSpeed);

            // Корректируем центр при приседании
            Vector3 center = characterController.center;
            float targetCenterY = _isCrouching ? crouchHeight * 0.5f : _normalHeight * 0.5f;
            center.y = Mathf.Lerp(center.y, targetCenterY, Time.deltaTime * crouchHeightSpeed);
            characterController.center = center;
        }

        private bool CanStand()
        {
            // Проверяем только если персонаж в приседе
            if (!_isCrouching)
                return true;

            // Позиция верхушки головы при нормальной высоте
            Vector3 headPosition = transform.position + Vector3.up * crouchHeight + Vector3.up * standCheckOffset;
            
            // Проверяем, будет ли коллизия если встанем
            if (Physics.Raycast(headPosition, Vector3.up, standCheckDistance, obstacleLayer))
            {
                // Если есть препятствие - не можем встать
                return false;
            }

            return true;
        }

        public bool IsGrounded => _isGrounded;
        public bool IsCrouching => _isCrouching;
        public float CurrentSpeed => new Vector3(_velocity.x, 0, _velocity.z).magnitude;
        
        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }

        public void SetSprintSpeed(float speed)
        {
            sprintSpeed = speed;
        }

        public void SetCrouchSpeed(float speed)
        {
            crouchSpeed = speed;
        }

        public void SetCrouchHeight(float height)
        {
            crouchHeight = height;
        }
    }
}
