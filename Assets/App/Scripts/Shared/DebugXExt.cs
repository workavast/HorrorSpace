using DCFApixels;
using UnityEngine;

namespace App
{
    public static class DebugXExt
    {
        public static void DrawCube(this Transform transform, BoxCollider boxCollider) 
            => transform.DrawCube(boxCollider, Color.white);

        public static void DrawCube(this Transform transform, BoxCollider boxCollider, Color color)
        {
            if (boxCollider == null)
                return;
              
            var position = transform.position;
            var rotation = transform.rotation;
            var size = transform.lossyScale;
            
            position += boxCollider.center;
            
            size.x *= boxCollider.size.x;
            size.y *= boxCollider.size.y;
            size.z *= boxCollider.size.z;
            
            DebugX.Draw(color).Cube(position, rotation, size);
        }
    }
}