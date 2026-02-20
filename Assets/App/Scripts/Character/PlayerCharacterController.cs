using System.Collections.Generic;
using App.Character.FSM.Jump;
using App.Character.FSM.Movement;
using App.InteractiveObjects;
using App.ObjectHandling;
using App.Usable;
using Avastrad.FSM;
using UnityEngine;

namespace App.Character
{
    public class PlayerCharacterController : MonoBehaviour
    {
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerInputManager inputManager;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerParams playerParams = new();
        [SerializeField] private InteractiveObjBoundsDrawer interactiveObjBoundsDrawer;

        private PlayerContext _ctx;
        private InputProvider _inputProvider;
        private StateMachine _movementFsm;
        private StateMachine _jumpFsm;
        private StateMachine _mainFsm;

        [Header("Interaction")]
        [SerializeField] private float interactRange = 3f;
        [SerializeField] private LayerMask interactLayerMask = ~0;
        [SerializeField] private HandleObjectController handleObjectController;

        private readonly int _animatorSpeedHash = Animator.StringToHash("Speed");
        private readonly int _animatorIsGroundedHash = Animator.StringToHash("IsGrounded");
        private readonly int _animatorIsCrouchingHash = Animator.StringToHash("IsCrouching");

        private void Awake() 
            => Initialize();

        private void Update() 
            => _mainFsm.ManualUpdate();

        private void Initialize()
        {
            if (characterController == null)
                characterController = GetComponent<CharacterController>();

            if (animator == null)
                animator = GetComponentInChildren<Animator>();

            _ctx = new PlayerContext(characterController, cameraController, transform, animator);
            _inputProvider = new InputProvider();

            // Movement FSM
            var moveStates = new List<StateBase>
            {
                new MovementStandState(playerParams, _inputProvider, _ctx),
                new MovementCrouchState(playerParams, _inputProvider, _ctx)
            };
            _movementFsm = new StateMachine(moveStates);
            _movementFsm.Init<MovementStandState>();

            // Jump FSM
            var jumpStates = new List<StateBase>
            {
                new JumpGroundedState(playerParams, _inputProvider, _ctx),
                new JumpAirState(playerParams, _inputProvider, _ctx)
            };
            _jumpFsm = new StateMachine(jumpStates);
            _jumpFsm.Init<JumpGroundedState>();

            // Main FSM with a single active state that routes input and updates inner FSMs
            var mainStates = new List<StateBase>
            {
                new PlayerMainState(this, _inputProvider, _movementFsm, _jumpFsm)
            };
            _mainFsm = new StateMachine(mainStates);
            _mainFsm.Init<PlayerMainState>();
        }

        // Exposed helpers used by PlayerMainState
        private void CheckGroundContact()
        {
            var legs = _ctx.Transform.position;
            var checkOffset = Vector3.down * playerParams.GroundCheckDistance;
            var origin = legs + checkOffset + Vector3.up * playerParams.CollisionCheckRadius;

            _ctx.IsGroundContact = Physics.CheckSphere(origin, playerParams.CollisionCheckRadius, playerParams.ObstacleLayer);
        }

        private void ApplyMovement()
        {
            if (characterController.enabled)
                characterController.Move(_ctx.Velocity * Time.deltaTime);
        }

        private void UpdateAnimator()
        {
            if (animator == null)
                return;

            var currentSpeed = new Vector3(_ctx.Velocity.x, 0, _ctx.Velocity.z).magnitude;
            animator.SetFloat(_animatorSpeedHash, currentSpeed);
            animator.SetBool(_animatorIsGroundedHash, _ctx.IsGrounded);
            animator.SetBool(_animatorIsCrouchingHash, _ctx.IsCrouching);
        }

        // PlayerMainState nested so it can call private helpers cleanly
        private class PlayerMainState : StateBase
        {
            private readonly PlayerCharacterController _owner;
            private readonly InputProvider _inputProvider;
            private readonly StateMachine _movementFsm;
            private readonly StateMachine _jumpFsm;

            public PlayerMainState(PlayerCharacterController owner, InputProvider input, StateMachine movementFsm, StateMachine jumpFsm)
            {
                _owner = owner;
                _inputProvider = input;
                _movementFsm = movementFsm;
                _jumpFsm = jumpFsm;
            }

            public override void Enter(string[] args) { }
            public override void Exit() { }
            public override void ManualFixedUpdate() { }

            public override void ManualUpdate()
            {
                // Read inputs into provider
                _inputProvider.SetMove(_owner.inputManager.MoveInput);
                _inputProvider.SetSprint(_owner.inputManager.IsSprintPressed);
                _inputProvider.SetCrouch(_owner.inputManager.IsCrouchPressed);
                _inputProvider.SetJumpThisFrame(_owner.inputManager.JumpPressedThisFrame);

                // Environment and collision checks
                _owner.CheckGroundContact();

                // Update nested FSMs
                _movementFsm.ManualUpdate();
                _jumpFsm.ManualUpdate();

                _owner.ApplyMovement();
                _owner.UpdateAnimator();

                var isHoldObject = _owner.handleObjectController.IsHoldingObject();
                // Check for interaction highlight
                _owner.CheckInteractionHighlight(!isHoldObject);

                // Using handling
                if (_owner.inputManager.InteractPressedThisFrame && !isHoldObject)
                    _owner.TryUse();
            }
        }

        private void TryUse()
        {
            var cam = Camera.main;
            var origin = cam.transform.position;
            var dir = cam.transform.forward;

            if (Physics.Raycast(origin, dir, out var hit, interactRange, interactLayerMask))
            {
                var interactable = hit.collider.GetComponent<IUsable>() ?? hit.collider.GetComponentInParent<IUsable>();
                interactable?.Interact(_ctx);
            }
        }

        private void CheckInteractionHighlight(bool show)
        {
            if (!show)
            {
                interactiveObjBoundsDrawer.SetTarget(null);
                return;
            }
            
            var cam = Camera.main;
            var origin = cam.transform.position;
            var dir = cam.transform.forward;

            if (Physics.Raycast(origin, dir, out var hit, interactRange, interactLayerMask))
            {
                var interactive = hit.collider.GetComponent<IInteractiveObj>() ?? hit.collider.GetComponentInParent<IInteractiveObj>();
                interactiveObjBoundsDrawer.SetTarget(interactive);
            }
            else
            {
                interactiveObjBoundsDrawer.SetTarget(null);
            }
        }
    }
}
