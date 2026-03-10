using App.HandlingObjects.HoldableObjects;
using Avastrad.Extensions;
using Avastrad.FSM;
using Avastrad.Libs.CheckOnNullLib;
using UnityEngine;

namespace App.Character.Hand.FSM
{
    public abstract class HandState : StateBase
    {
        protected readonly HandContext _ctx;
        protected readonly IHandParams _params;

        protected HandState(HandContext ctx, IHandParams parameters)
        {
            _ctx = ctx;
            _params = parameters;
        }

        protected bool ObjInHandleRange(Transform objTransform)
        {
            var playerPosition = _ctx.Transform.position;
            var objPosition = objTransform.position;
            return playerPosition.DistanceXZ(objPosition) <= _params.AutoDropDistance;
        }
        
        /// <summary>
        /// Update in fixed update
        /// </summary>
        protected void HoldObject(IHoldableObj holdableObj)
        {
            if (holdableObj.IsAnyNull())
                return;
            
            var camera = _ctx.CameraController.GetCamera();
            var objTransform = holdableObj.Transform;
            var rb = holdableObj.Rb;

            // Handle zoom with mouse wheel
            var scrollDelta = _ctx.InputManager.ZoomDelta;
            if (scrollDelta != 0)
            {
                var currentDistance = _ctx.CurrentDistance + scrollDelta * _params.ZoomSpeed * Time.deltaTime;
                currentDistance = Mathf.Clamp(currentDistance, _params.MinDistance, _params.MaxDistance);
                _ctx.CurrentDistance = currentDistance;
            }

            // Apply physics forces to attract object to target position
            var targetPosition = camera.transform.position + camera.transform.forward * _ctx.CurrentDistance;
            var direction = targetPosition - objTransform.position;
            var distance = direction.magnitude;
                
            if (distance > 0.01f)
            {
                // Apply attraction force proportional to distance
                var force = direction.normalized * (_params.AttractionForce * distance);
                    
                // Limit maximum speed
                if (rb.linearVelocity.magnitude > _params.MaxAttractionSpeed)
                    rb.linearVelocity = rb.linearVelocity.normalized * _params.MaxAttractionSpeed;
                    
                rb.AddForce(force * Time.deltaTime);
            }

            // Apply rotation return to target rotation relative to player
            // Compute target rotation in world space from stored relative-to-player quaternion
            var targetWorld = _ctx.Transform.rotation * _ctx.TargetRotationRelativeToPlayer;
            var currentWorld = objTransform.rotation;

            // Delta that rotates current -> target: delta = target * inv(current)
            var delta = targetWorld * Quaternion.Inverse(currentWorld);

            delta.ToAngleAxis(out var angleDeg, out var axis);
            if (axis == Vector3.zero || float.IsNaN(axis.x))
            {
                // no meaningful rotation
            }
            else
            {
                // Ensure shortest angle (-180..180)
                if (angleDeg > 180f) 
                    angleDeg -= 360f;

                var angleRad = angleDeg * Mathf.Deg2Rad;

                // Proportional target angular velocity (PD-style)
                var desiredAngVel = axis.normalized * (angleRad * _params.RotationReturnSpeed);

                // Compute corrective torque: proportional to velocity error and some damping
                var angVelError = desiredAngVel - rb.angularVelocity;

                // Scale torque by approximate inertia (use inertiaTensor average) to respect object's shape
                var inertiaAvg = (rb.inertiaTensor.x + rb.inertiaTensor.y + rb.inertiaTensor.z) * 0.3333333f;
                var torque = angVelError * (inertiaAvg * _params.TorqueMultiplier);

                // Apply torque
                rb.AddTorque(torque);
            }
        }
        
        protected void DropObject()
        {
            var currentHandledObject = _ctx.HandledObj;
            if (currentHandledObject == null)
                return;
            
            // Reset angular damping before dropping
            var rb = currentHandledObject.Rb;
            rb.linearDamping = _ctx.DefaultObjectLinearDamping;
            rb.angularDamping = _ctx.DefaultObjectAngularDamping;

            currentHandledObject.Drop();
            _ctx.HandledObj = null;
        }
    }
}