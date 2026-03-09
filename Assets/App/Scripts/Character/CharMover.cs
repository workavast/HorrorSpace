using Avastrad.Extensions;
using KinematicCharacterController;
using UnityEngine;

namespace App.Character
{
    public class CharMover : MonoBehaviour, ICharacterController
    {
        [SerializeField] private KinematicCharacterMotor motor;
        
        private PlayerContext _ctx;
        private PlayerParams _params;
        private Vector3 _delta;

        public void Initialize(PlayerContext playerContext, PlayerParams playerParams)
        {
            motor.CharacterController = this;
            _ctx = playerContext;
            _params = playerParams;
        }
        
        public void SetLookDeltaRotation(Vector3 eulerDelta)
        {
            _delta = eulerDelta;
        }
        
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            var angle = currentRotation.eulerAngles + _delta;
            currentRotation = Quaternion.Euler(angle);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            {//decelerate old velocity
                const int targetSpeed = 0;
                var decelerationScale = (_params.Deceleration * deltaTime);

                if (currentVelocity.XZ().magnitude - targetSpeed < decelerationScale) 
                    decelerationScale = currentVelocity.XZ().magnitude - targetSpeed;
                
                var decelerationResult = currentVelocity.X0Z().normalized * decelerationScale;
                currentVelocity -= decelerationResult;
            }
            
            if (_ctx.VelocityInput != Vector2.zero)
            {
                var targetSpeed = _ctx.TargetSpeed * _params.VelocityModifierPerGrabObjMass.Evaluate(_ctx.DraggedObjMass);
                var moveDirection = _ctx.Transform.right * _ctx.VelocityInput.x + 
                                    _ctx.Transform.forward * _ctx.VelocityInput.y;    
                
                //add deceleration to the acceleration need cus in prev block we decelerate old velocity
                var velocityDelta = moveDirection * ((_params.Acceleration + _params.Deceleration) * deltaTime);
                currentVelocity += velocityDelta;
                
                if (currentVelocity.XZ().magnitude > targetSpeed)
                {
                    //add acceleration to the deceleration need cus in prev block we use sum of them
                    var deceleration = _params.Deceleration + _params.Acceleration;
                    var decelerationScale = (deceleration * deltaTime);
                    if (currentVelocity.XZ().magnitude - targetSpeed < decelerationScale) 
                        decelerationScale = currentVelocity.XZ().magnitude - targetSpeed;
                
                    var dec = currentVelocity.X0Z().normalized * decelerationScale;
                    currentVelocity -= dec;
                }
            }

            currentVelocity.y = _ctx.GravityVelocity;
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