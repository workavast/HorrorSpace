using System.Collections.Generic;
using App.RobotVacuumCleaner.FSM;
using App.Utils.Debug;
using Avastrad.FSM;
using UnityEngine;

namespace App.RobotVacuumCleaner
{
    public class RobotVacuumCleaner : MonoBehaviour
    {
        [SerializeField] private RvcParams rvcParams = new();
        [SerializeField] private BoxCollider occupationCheckCollider;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private List<Transform> forwardObstacleChecks;

        private StateMachine _fsm;
        private RvcContext _ctx;

        public ReadOnlyQueue<string> DebugQueue = new(5);

        private void Awake()
        {
            _ctx = new RvcContext(transform, rb, occupationCheckCollider, groundCheckPoint, forwardObstacleChecks);
            var states = new List<StateBase>
            {
                new RvcIdleState(_ctx, rvcParams),
                new RvcRotateState(_ctx, rvcParams),
                new RvcMoveState(_ctx, rvcParams),
                new RvcOccupiedState(_ctx, rvcParams),
                new RvcInAirState(_ctx, rvcParams)
            };
            
            _fsm = new StateMachine(states);
            _fsm.Init<RvcIdleState>();

            _fsm.OnStateSwitched += (st) => DebugQueue.Enqueue(st.Name);
        }

        private void Update() 
            => _fsm.Update();

        private void FixedUpdate()
            => _fsm.FixedUpdate();

        private void OnValidate()
        {
            if (occupationCheckCollider != null)
                occupationCheckCollider.isTrigger = true;
        }

        private void OnDrawGizmosSelected()
        {
            if (occupationCheckCollider == null)
                return;

            Gizmos.color = Color.cyan;
            var worldCenter = occupationCheckCollider.transform.TransformPoint(occupationCheckCollider.center);
            Gizmos.matrix = Matrix4x4.TRS(worldCenter, occupationCheckCollider.transform.rotation, occupationCheckCollider.transform.lossyScale);
            Gizmos.DrawWireCube(Vector3.zero, occupationCheckCollider.size);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}
