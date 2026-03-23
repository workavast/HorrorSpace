using System;
using System.Collections.Generic;
using Avastrad.Libs.CheckOnNullLib;
using UnityEngine;

namespace App.InteractiveObjects
{
    public class InteractiveObjBoundsDrawer : MonoBehaviour
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

        public void SetTarget(IInteractiveObj interactiveObj)
        {
            if (interactiveObj.IsAnyNull() || !interactiveObj.ShouldDraw())
            {
                highlightImage.gameObject.SetActive(false);
                nonInteractableImage.gameObject.SetActive(false);
                return;
            }

            var meshFilters = interactiveObj.MeshFilters;
            if (meshFilters == null || meshFilters.Count == 0)
            {
                highlightImage.gameObject.SetActive(false);
                nonInteractableImage.gameObject.SetActive(false);
                return;
            }
            
            var worldPoints = GetWorldPointsFromMeshes(meshFilters);
            var screenPoints = new Vector3[worldPoints.Length];

            for (var i = 0; i < worldPoints.Length; i++)
                screenPoints[i] = mainCamera.WorldToScreenPoint(worldPoints[i]);

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

            // Clamp rect to screen to ensure UI bounds are always visible
            float screenW = Screen.width;
            float screenH = Screen.height;
            
            minX = Mathf.Clamp(minX, 0f, screenW);
            maxX = Mathf.Clamp(maxX, 0f, screenW);
            minY = Mathf.Clamp(minY, 0f, screenH);
            maxY = Mathf.Clamp(maxY, 0f, screenH);
            
            if (minX >= maxX || minY >= maxY)
            {
                highlightImage.gameObject.SetActive(false);
                nonInteractableImage.gameObject.SetActive(false);
                return;
            }

            // Choose which image to use based on IsInteractable
            RectTransform targetRect;
            if (interactiveObj.IsInteractable)
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

        private static bool TryGetWorldBoundsFromMesh(GameObject target, out Bounds bounds)
        {
            bounds = new Bounds();
            if (target == null)
                return false;

            var meshFilter = target.GetComponent<MeshFilter>() ?? target.GetComponentInChildren<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null || meshFilter.sharedMesh.vertexCount == 0)
                return false;

            var mesh = meshFilter.sharedMesh;
            var vertices = mesh.vertices;
            if (vertices == null || vertices.Length == 0)
                return false;

            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            var mfTransform = meshFilter.transform;

            foreach (var vertex in vertices)
            {
                var worldPoint = mfTransform.TransformPoint(vertex);
                min = Vector3.Min(min, worldPoint);
                max = Vector3.Max(max, worldPoint);
            }

            var center = (min + max) * 0.5f;
            bounds = new Bounds(center, max - min);
            return bounds.size != Vector3.zero;
        }

        private static Vector3[] GetWorldPointsFromMeshes(IReadOnlyList<MeshFilter> meshFilters)
        {
            if (meshFilters == null || meshFilters.Count == 0)
                return Array.Empty<Vector3>();

            var pointList = new List<Vector3>();
            foreach (var mf in meshFilters)
            {
                if (mf == null || mf.sharedMesh == null)
                    continue;

                foreach (var vertex in mf.sharedMesh.vertices)
                    pointList.Add(mf.transform.TransformPoint(vertex));
            }

            if (pointList.Count == 0)
                return Array.Empty<Vector3>();

            return pointList.ToArray();
        }
    }
}