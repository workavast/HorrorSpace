using UnityEngine;

namespace Avastrad.Extensions
{
    public static class ColliderExt
    {
        public static bool Contains(this Collider checkCollider, Vector3 point)
            => checkCollider.bounds.Contains(point);

        public static bool ContainsByY(this Collider checkCollider, Vector3 point)
        {
            var maxY = checkCollider.bounds.max.y;
            var minY = checkCollider.bounds.min.y;
            return minY <= point.y && point.y <= maxY;
        }

        public static bool CheckBox(this BoxCollider checkCollider, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            var center = checkCollider.transform.TransformPoint(checkCollider.center);
            var extents = Vector3.Scale(checkCollider.size * 0.5f, checkCollider.transform.lossyScale);
            var rotation = checkCollider.transform.rotation;
            return Physics.CheckBox(center, extents, rotation, layerMask, queryTriggerInteraction);
        }
    }
}