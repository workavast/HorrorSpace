using App.Character;
using UnityEngine;
using Avastrad.FSM;
using System.Collections.Generic;
using App.ObjectHandling.FSM;

namespace App.ObjectHandling
{
    public class HandsController : MonoBehaviour
    {
        [SerializeField] private PlayerInputManager inputManager;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private CursorViewToggler cursorViewToggler;
        [SerializeField] private HandParams handParams = new();

        public float DraggedObjMass => _handContext.DraggedObjMass;
        
        private HandContext _handContext;
        private StateMachine _fsm;

        private void Awake() 
            => Initialize();

        private void Update() 
            => _fsm.ManualUpdate();

        private void Initialize()
        {
            _handContext = new HandContext(inputManager, cameraController, playerTransform, cursorViewToggler);
            
            var states = new List<StateBase>
            {
                new HandEmptyState(_handContext, handParams),
                new HandHoldState(_handContext, handParams),
                new HandRotateState(_handContext, handParams),
                new HandDragState(_handContext, handParams)
            };
            _fsm = new StateMachine(states);
            _fsm.Init<HandEmptyState>();
        }
        
        public bool IsHoldingObject() 
            => _handContext.HasHandledObject;
    }
}