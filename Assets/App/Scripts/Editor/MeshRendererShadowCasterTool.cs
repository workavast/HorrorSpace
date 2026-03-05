using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Editor tool for setting Static Shadow Caster property on MeshRenderer components in prefabs.
/// Usage: Tools > Mesh Renderer Shadow Caster Tool
/// </summary>
public class MeshRendererShadowCasterTool : EditorWindow
{
    private List<GameObject> prefabs = new List<GameObject>();
    private Vector2 scrollPosition;
    private GUIStyle headerStyle;

    [MenuItem("Tools/Mesh Renderer Shadow Caster Tool")]
    public static void ShowWindow()
    {
        GetWindow<MeshRendererShadowCasterTool>("Shadow Caster Tool");
    }

    private void OnGUI()
    {
        if (headerStyle == null)
        {
            headerStyle = new GUIStyle(EditorStyles.boldLabel) { fontSize = 14 };
        }

        GUILayout.Label("Static Shadow Caster Configuration", headerStyle);
        GUILayout.Label("Select prefabs to configure static shadow caster", EditorStyles.helpBox);
        GUILayout.Space(10);

        // Prefabs list section
        GUILayout.Label("Prefabs:", EditorStyles.boldLabel);
        
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
        
        for (int i = 0; i < prefabs.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            GameObject newPrefab = EditorGUILayout.ObjectField(prefabs[i], typeof(GameObject), false) as GameObject;
            
            // Validate that it's actually a prefab
            if (newPrefab != null)
            {
                PrefabAssetType prefabType = PrefabUtility.GetPrefabAssetType(newPrefab);
                if (prefabType == PrefabAssetType.NotAPrefab)
                {
                    EditorGUILayout.HelpBox("Not a prefab", MessageType.Warning);
                    newPrefab = null;
                }
            }
            prefabs[i] = newPrefab;
            
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                prefabs.RemoveAt(i);
                i--;
            }
            EditorGUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Prefab", GUILayout.Height(30)))
        {
            prefabs.Add(null);
        }

        if (GUILayout.Button("Add Selected Prefabs", GUILayout.Height(30)))
        {
            AddSelectedPrefabs();
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20);

        // Apply button
        GUI.backgroundColor = new Color(0.3f, 0.8f, 0.3f);
        if (GUILayout.Button("Apply Static Shadow Caster", GUILayout.Height(40)))
        {
            ApplyShadowCaster();
        }
        GUI.backgroundColor = Color.white;

        GUILayout.Space(10);
        EditorGUILayout.HelpBox(
            "This tool will set staticShadowCaster = true on all MeshRenderer components " +
            "in the selected prefabs and their children.",
            MessageType.Info);
    }

    private void AddSelectedPrefabs()
    {
        Object[] selected = Selection.objects;
        int addedCount = 0;

        foreach (var obj in selected)
        {
            GameObject gameObject = obj as GameObject;
            if (gameObject == null)
                continue;

            // Check if it's a prefab
            PrefabAssetType prefabType = PrefabUtility.GetPrefabAssetType(gameObject);
            if (prefabType != PrefabAssetType.NotAPrefab)
            {
                // Avoid duplicates
                if (!prefabs.Contains(gameObject))
                {
                    prefabs.Add(gameObject);
                    addedCount++;
                }
            }
        }

        if (addedCount > 0)
        {
            Debug.Log($"[MeshRendererShadowCasterTool] Added {addedCount} prefab(s)");
        }
        else
        {
            EditorUtility.DisplayDialog("Add Selected Prefabs", "No valid prefabs selected.", "OK");
        }
    }

    private void ApplyShadowCaster(){
        int prefabCount = 0;
        int rendererCount = 0;
        List<string> failedPrefabs = new List<string>();

        foreach (var prefab in prefabs)
        {
            if (prefab == null)
                continue;

            try
            {
                // Get all MeshRenderers in the prefab and its children
                MeshRenderer[] renderers = prefab.GetComponentsInChildren<MeshRenderer>();
                
                if (renderers.Length == 0)
                {
                    failedPrefabs.Add($"{prefab.name} (no MeshRenderers found)");
                    continue;
                }

                foreach (var renderer in renderers)
                {
                    renderer.staticShadowCaster = true;
                    EditorUtility.SetDirty(renderer);
                    rendererCount++;
                }

                prefabCount++;
            }
            catch (System.Exception ex)
            {
                failedPrefabs.Add($"{prefab.name} (error: {ex.Message})");
                Debug.LogError($"Error processing prefab {prefab.name}: {ex}", prefab);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Log results
        string message = $"Processed {prefabCount} prefab(s)\nUpdated {rendererCount} MeshRenderer component(s)";
        
        if (failedPrefabs.Count > 0)
        {
            message += $"\n\nFailed to process {failedPrefabs.Count} prefab(s):";
            foreach (var failed in failedPrefabs)
            {
                message += $"\n  - {failed}";
            }
            EditorUtility.DisplayDialog("Shadow Caster Tool - Completed with Issues", message, "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Shadow Caster Tool - Success", message, "OK");
        }

        Debug.Log($"[MeshRendererShadowCasterTool] {message.Replace("\n", " | ")}");
    }
}
