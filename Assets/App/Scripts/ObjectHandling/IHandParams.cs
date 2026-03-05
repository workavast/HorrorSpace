using UnityEngine;

namespace App.ObjectHandling
{
    public interface IHandParams
    {
        public LayerMask HandleLayerMask { get; }
        public float PickupRange { get; }

        public float AutoDropDistance { get; }
        public float MinDistance { get; }
        public float MaxDistance { get; }
        public float ZoomSpeed { get; }

        public float AttractionForce { get; }
        public float DragAttractionForce { get; }
        public float MaxAttractionSpeed { get; }
        public float AngularDampingPickup { get; }
        public float LinerDampingPickup { get; }

        public float RotationReturnSpeed { get; }
        public float ManualRotationSpeed { get; }
        public float TorqueMultiplier { get; }
    }
}