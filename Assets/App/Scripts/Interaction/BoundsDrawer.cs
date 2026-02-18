using UnityEngine;
using UnityEngine.UI;

namespace App.Interaction
{
    public class BoundsDrawer : MonoBehaviour
    {
        [SerializeField] private Image highlightImage;
        [SerializeField] private Image nonInteractableImage;
        [SerializeField] private Camera mainCamera;

        private void Awake()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            highlightImage.enabled = false;
            nonInteractableImage.enabled = false;
        }

        public void SetTarget(IInteractable interactable)
        {
            if (interactable == null)
            {
                highlightImage.enabled = false;
                nonInteractableImage.enabled = false;
                return;
            }

            var target = interactable.GetGameObject();
            if (target == null)
            {
                highlightImage.enabled = false;
                nonInteractableImage.enabled = false;
                return;
            }
            
            // Get bounds from renderer or collider
            var bounds = GetBounds(target);
            if (bounds.size == Vector3.zero)
            {
                highlightImage.enabled = false;
                nonInteractableImage.enabled = false;
                return;
            }

            // Project bounds corners to screen space
            var worldCorners = GetWorldCorners(bounds);
            var screenPoints = new Vector3[worldCorners.Length];

            for (var i = 0; i < worldCorners.Length; i++) 
                screenPoints[i] = mainCamera.WorldToScreenPoint(worldCorners[i]);

            // Calculate screen rect
            float minX = float.MaxValue, maxX = float.MinValue;
            float minY = float.MaxValue, maxY = float.MinValue;

            foreach (var point in screenPoints)
            {
                if (point.z < 0) 
                    continue; // Behind camera

                minX = Mathf.Min(minX, point.x);
                maxX = Mathf.Max(maxX, point.x);
                minY = Mathf.Min(minY, point.y);
                maxY = Mathf.Max(maxY, point.y);
            }

            if (minX >= maxX || minY >= maxY)
            {
                highlightImage.enabled = false;
                nonInteractableImage.enabled = false;
                return;
            }

            // Choose which image to use based on IsInteractable
            Image targetImage;
            if (interactable.IsInteractable)
            {
                targetImage = highlightImage;
                nonInteractableImage.enabled = false;
            }
            else
            {
                targetImage = nonInteractableImage;
                highlightImage.enabled = false;
            }

            // Set image position and size
            var rectTransform = targetImage.rectTransform;
            rectTransform.position = new Vector2(minX, minY);
            rectTransform.sizeDelta = new Vector2(maxX - minX, maxY - minY);

            targetImage.enabled = true;
        }

        private static Bounds GetBounds(GameObject target)
        {
            // Try to get bounds from renderer first
            var renderer = target.GetComponent<Renderer>();
            if (renderer == null)
                renderer = target.GetComponentInChildren<Renderer>();

            if (renderer != null)
                return renderer.bounds;

            // Fallback to collider bounds
            var collider = target.GetComponent<Collider>();
            if (collider == null)
                collider = target.GetComponentInChildren<Collider>();

            if (collider != null)
                return collider.bounds;

            return new Bounds();
        }

        private static Vector3[] GetWorldCorners(Bounds bounds)
        {
            return new Vector3[]
            {
                bounds.min,
                new Vector3(bounds.min.x, bounds.min.y, bounds.max.z),
                new Vector3(bounds.min.x, bounds.max.y, bounds.min.z),
                new Vector3(bounds.min.x, bounds.max.y, bounds.max.z),
                new Vector3(bounds.max.x, bounds.min.y, bounds.min.z),
                new Vector3(bounds.max.x, bounds.min.y, bounds.max.z),
                new Vector3(bounds.max.x, bounds.max.y, bounds.min.z),
                bounds.max
            };
        }
    }
}