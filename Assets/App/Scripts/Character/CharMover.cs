using KinematicCharacterController;
using UnityEngine;

namespace App.Character
{
    public class CharMover : MonoBehaviour, ICharacterController
    {
        public KinematicCharacterMotor Motor;

        private PlayerContext _ctx;
        private PlayerParams _params;
        private Vector3 _delta;

        private void Awake()
        {
            Motor.CharacterController = this;
        }

        public void SetLookDeltaRotation(Vector3 eulerDelta)
        {
            _delta = eulerDelta;
        }
            
        public void Initialize(PlayerContext playerContext, PlayerParams playerParams)
        {
            _ctx = playerContext;
            _params = playerParams;
        }
        
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            var angle = currentRotation.eulerAngles + _delta;
            currentRotation = Quaternion.Euler(angle);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            currentVelocity = _ctx.Velocity;
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
            
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            
        }
    }
}