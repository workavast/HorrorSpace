using System.Collections.Generic;
using App.Character.FSM.Jump;
using App.Character.FSM.Movement;
using App.InteractiveObjects;
using App.ObjectHandling;
using App.Usable;
using App.Utils.Debug;
using Avastrad.FSM;
using KinematicCharacterController;
using UnityEngine;

namespace App.Character
{
    public class PlayerCharacterController : MonoBehaviour
    {
        [SerializeField] private CharMover charMover;
        [SerializeField] private KinematicCharacterMotor motor;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerInputManager inputManager;
        [SerializeField] private HandsController handsController;
        [SerializeField] private InteractiveObjBoundsDrawer interactiveObjBoundsDrawer;
        [Space]
        [SerializeField] private PlayerParams playerParams = new();

        [Header("Interaction")]
        [SerializeField] private float interactRange = 3f;
        [SerializeField] private LayerMask interactLayerMask = ~0;
        
        [Header("Debug")]
        [SerializeField] private ReadOnlyQueue<string> jumpDebugStates;

        private PlayerContext _ctx;
        private InputProvider _inputProvider;
        private StateMachine _movementFsm;
        private StateMachine _jumpFsm;
        private StateMachine _mainFsm;

        private void Awake()
        {
            Initialize();
            charMover.Initialize(_ctx, playerParams);
        }

        private void Update() 
            => _mainFsm.ManualUpdate();

        private void Initialize()
        {
            _ctx = new PlayerContext(charMover, motor,cameraController, transform);
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
            _jumpFsm.OnStateSwitched += (state) => jumpDebugStates.Enqueue(state.Name + " " + Time.time);

            // Main FSM with a single active state that routes input and updates inner FSMs
            var mainStates = new List<StateBase>
            {
                new PlayerMainState(this, _inputProvider, _movementFsm, _jumpFsm)
            };
            _mainFsm = new StateMachine(mainStates);
            _mainFsm.Init<PlayerMainState>();
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

                // Update nested FSMs
                _movementFsm.ManualUpdate();
                _jumpFsm.ManualUpdate();

                var isHoldingObject = _owner.handsController.IsHoldingObject();
                _owner.CheckInteractionHighlight(!isHoldingObject);
                if (_owner.inputManager.InteractPressedThisFrame && !isHoldingObject)
                    _owner.TryUseObject();
            }
        }

        private void TryUseObject()
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
