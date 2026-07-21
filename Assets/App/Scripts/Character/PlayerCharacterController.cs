using System.Collections.Generic;
using App.Character.FSM.Jump;
using App.Character.FSM.Movement;
using App.Character.Hand;
using App.Character.Hand.FSM;
using App.Health;
using App.InteractiveObjects;
using App.Usable;
using App.Utils.Debug;
using Avastrad.FSM;
using KinematicCharacterController;
using UnityEngine;

using App.Mutant;

namespace App.Character
{
    public class PlayerCharacterController : MonoBehaviour, IMutantTarget
    {
        [SerializeField] private CharMover charMover;
        [SerializeField] private KinematicCharacterMotor motor;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerInputManager inputManager;
        [SerializeField] private CursorViewToggler cursorViewToggler;
        [SerializeField] private InteractiveObjBoundsDrawer interactiveObjBoundsDrawer;
        [SerializeField] private HealthModel healthComp;
        [Space]
        [SerializeField] private PlayerParams playerParams = new();
        [SerializeField] private HandParams handParams = new();
        
        [Header("Interaction")]
        [SerializeField] private float interactRange = 3f;
        [SerializeField] private LayerMask interactLayerMask = ~0;
        
        [Header("Debug")]
        [SerializeField] private ReadOnlyQueue<string> jumpDebugStates;

        private PlayerContext _ctx;
        public int Priority => 100;
        public MutantTargetType Type => MutantTargetType.See;
        public Vector3 Position => transform.position;
        public Vector3 MiddlePoint => transform.position + Vector3.up * motor.Capsule.height / 2f;
        
        private StateMachine _movementFsm;
        private StateMachine _jumpFsm;
        private StateMachine _mainFsm;
        private StateMachine _handFsm;

        private void Awake()
        {
            Initialize();
            charMover.Initialize(_ctx, playerParams);
        }

        private void Update() 
            => _mainFsm.Update();
        
        private void FixedUpdate() 
            => _mainFsm.FixedUpdate();

        private void Initialize()
        {
            healthComp.Initialize();
            
            var handCtx = new HandContext(inputManager, cameraController, transform, cursorViewToggler);
            _ctx = new PlayerContext(motor,cameraController, transform, handCtx, healthComp);

            //Movement FSM
            var moveStates = new List<StateBase>
            {
                new MovementStandState(playerParams, inputManager, _ctx),
                new MovementCrouchState(playerParams, inputManager, _ctx)
            };
            _movementFsm = new StateMachine(moveStates);
            _movementFsm.Init<MovementStandState>();
            
            // Jump FSM
            var jumpStates = new List<StateBase>
            {
                new JumpGroundedState(playerParams, inputManager, _ctx),
                new JumpAirState(playerParams, inputManager, _ctx)
            };
            _jumpFsm = new StateMachine(jumpStates);
            _jumpFsm.Init<JumpGroundedState>();
            _jumpFsm.OnStateSwitched += (state) => jumpDebugStates.Enqueue(state.Name + " " + Time.time);
            
            // Hand FSM
            var handStates = new List<StateBase>
            {
                new HandEmptyState(handCtx, handParams),
                new HandHoldState(handCtx, handParams),
                new HandRotateState(handCtx, handParams),
                new HandDragState(handCtx, handParams)
            };
            _handFsm = new StateMachine(handStates);
            _handFsm.Init<HandEmptyState>();
            
            // Main FSM with a single active state that routes input and updates inner FSMs
            var mainStates = new List<StateBase>
            {
                new PlayerMainState(this, _movementFsm, _jumpFsm, _handFsm)
            };
            _mainFsm = new StateMachine(mainStates);
            _mainFsm.Init<PlayerMainState>();
        }
        
        private void TryUseObject()
        {
            var cam = Camera.main;
            var origin = cam.transform.position;
            var dir = cam.transform.forward;

            if (Physics.Raycast(origin, dir, out var hit, interactRange, interactLayerMask))
            {
                var interactable = hit.collider.GetComponent<IUsable>();
                interactable?.Interact();
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
        
        // PlayerMainState nested so it can call private helpers cleanly
        private class PlayerMainState : StateBase
        {
            private readonly PlayerCharacterController _owner;
            private readonly StateMachine _movementFsm;
            private readonly StateMachine _jumpFsm;
            private readonly StateMachine _handFsm;

            public PlayerMainState(PlayerCharacterController owner, 
                StateMachine movementFsm, StateMachine jumpFsm, StateMachine handFsm)
            {
                _owner = owner;
                _movementFsm = movementFsm;
                _jumpFsm = jumpFsm;
                _handFsm = handFsm;
            }

            public override void Enter(Dictionary<string, object> args) { }
            public override void Exit() { }

            public override void ManualFixedUpdate()
            {
                if (!_owner.healthComp.IsAlive)
                    return;
                
                _movementFsm.FixedUpdate();
                _jumpFsm.FixedUpdate();
                _handFsm.FixedUpdate();
            }

            public override void ManualUpdate()
            {
                if (!_owner.healthComp.IsAlive)
                    return;
                
                _movementFsm.Update();
                _jumpFsm.Update();
                _handFsm.Update();
                
                var isHoldingObject = _owner._ctx.HasHandledObject;
                _owner.CheckInteractionHighlight(!isHoldingObject);
                if (_owner.inputManager.InteractPressedThisFrame && !isHoldingObject)
                    _owner.TryUseObject();
            }
        }
    }
}
