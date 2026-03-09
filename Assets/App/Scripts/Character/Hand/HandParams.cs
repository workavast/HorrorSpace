using UnityEngine;

namespace App.Character.Hand
{
    [System.Serializable]
    public class HandParams : IHandParams
    {
        [Header("Pickup")]
        [SerializeField] private LayerMask handleLayerMask = ~0;
        [SerializeField] private float pickupRange = 3f;

        [Header("Distance")] 
        [SerializeField] private float autoDropDistance = 6f;
        [SerializeField] private float minDistance = 1f;
        [SerializeField] private float maxDistance = 5f;
        [SerializeField] private float zoomSpeed = 1f;

        [Header("Physics")] 
        [SerializeField] private float attractionForce = 100f;
        [SerializeField] private float dragAttractionForce = 100f;
        [SerializeField] private float maxAttractionSpeed = 10f;
        [SerializeField] private float angularDampingPickup = 5f;
        [SerializeField] private float linerDampingPickup = 5f;

        [Header("Rotation")] 
        [SerializeField] private float rotationReturnSpeed = 2f;
        [SerializeField] private float manualRotationSpeed = 5f;
        [SerializeField] private float torqueMultiplier = 1f;

        public LayerMask HandleLayerMask => handleLayerMask;
        public float PickupRange => pickupRange;
        public float AutoDropDistance => autoDropDistance;
        public float MinDistance => minDistance;
        public float MaxDistance => maxDistance;
        public float ZoomSpeed => zoomSpeed;
        public float AttractionForce => attractionForce;
        public float DragAttractionForce => dragAttractionForce;
        public float MaxAttractionSpeed => maxAttractionSpeed;
        public float AngularDampingPickup => angularDampingPickup;
        public float LinerDampingPickup => linerDampingPickup;
        public float RotationReturnSpeed => rotationReturnSpeed;
        public float ManualRotationSpeed => manualRotationSpeed;
        public float TorqueMultiplier => torqueMultiplier;
    }
}