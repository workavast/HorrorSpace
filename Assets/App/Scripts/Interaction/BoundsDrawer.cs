using UnityEngine;

namespace App.Interaction
{
    public class BoundsDrawer : MonoBehaviour
    {
        [SerializeField] private RectTransform highlightImage;
        [SerializeField] private RectTransform nonInteractableImage;
        [SerializeField] private Camera mainCamera;

        private void Awake()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            highlightImage.gameObject.SetActive(false);
            nonInteractableImage.gameObject.SetActive(false);
        }

        public void SetTarget(IInteractable interactable)
        {
            if (interactable == null)
            {
                highlightImage.gameObject.SetActive(false);
                nonInteractableImage.gameObject.SetActive(false);
                return;
            }

            var target = interactable.GetGameObject();
            if (target == null)
            {
                highlightImage.gameObject.SetActive(false);
                nonInteractableImage.gameObject.SetActive(false);
                return;
            }
            
            // Get bounds from renderer or collider
            var bounds = GetBounds(target);
            if (bounds.size == Vector3.zero)
            {
                highlightImage.gameObject.SetActive(false);
                nonInteractableImage.gameObject.SetActive(false);
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
                highlightImage.gameObject.SetActive(false);
                nonInteractableImage.gameObject.SetActive(false);
                return;
            }

            // Choose which image to use based on IsInteractable
            RectTransform targetRect;
            if (interactable.IsInteractable)
            {
                targetRect = highlightImage;
                nonInteractableImage.gameObject.SetActive(false);
            }
            else
            {
                targetRect = nonInteractableImage;
                highlightImage.gameObject.SetActive(false);
            }

            // Set image position and size
            targetRect.position = new Vector2(minX, minY);
            targetRect.sizeDelta = new Vector2(maxX - minX, maxY - minY);

            targetRect.gameObject.SetActive(true);
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