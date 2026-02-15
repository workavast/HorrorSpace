using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReplaceWithPrefabEditor : EditorWindow
{
    GameObject prefab;
    List<GameObject> targets = new List<GameObject>();
    Vector2 scroll;

    [MenuItem("Tools/Replace With Prefab")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceWithPrefabEditor>("Replace With Prefab");
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Targets (Array)", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Selected"))
        {
            foreach (var go in Selection.gameObjects)
            {
                if (go != null && !targets.Contains(go)) targets.Add(go);
            }
        }
        if (GUILayout.Button("Clear List")) targets.Clear();
        EditorGUILayout.EndHorizontal();

        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(160));
        for (int i = 0; i < targets.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            targets[i] = (GameObject)EditorGUILayout.ObjectField(targets[i], typeof(GameObject), true);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                targets.RemoveAt(i);
                i--;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        if (GUILayout.Button("Replace Selected"))
        {
            if (prefab == null) EditorUtility.DisplayDialog("Error", "Prefab is not set", "OK");
            else Replace(Selection.gameObjects);
        }

        if (GUILayout.Button("Replace Array"))
        {
            if (prefab == null) EditorUtility.DisplayDialog("Error", "Prefab is not set", "OK");
            else Replace(targets.ToArray());
        }

        if (GUILayout.Button("Replace Selected + Array"))
        {
            if (prefab == null) EditorUtility.DisplayDialog("Error", "Prefab is not set", "OK");
            else
            {
                var set = new HashSet<GameObject>(targets);
                foreach (var g in Selection.gameObjects) if (g != null) set.Add(g);
                Replace(new List<GameObject>(set).ToArray());
            }
        }
    }

    void Replace(GameObject[] sourceTargets)
    {
        if (sourceTargets == null || sourceTargets.Length == 0)
        {
            EditorUtility.DisplayDialog("Info", "No targets to replace.", "OK");
            return;
        }

        int count = 0;
        foreach (var t in sourceTargets) if (t != null) count++;
        if (count == 0)
        {
            EditorUtility.DisplayDialog("Info", "No valid targets found.", "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog("Confirm Replace", $"Replace {count} objects with prefab '{prefab.name}'?", "Replace", "Cancel"))
            return;

        // Build a set of existing names to ensure unique postfixes
        var existingNames = new HashSet<string>();
        foreach (var go in Object.FindObjectsOfType<GameObject>())
            existingNames.Add(go.name);

        int createdIndex = 0;

        // Operate on a copy to avoid modifying collection while iterating
        foreach (var target in sourceTargets)
        {
            if (target == null) continue;

            // Instantiate prefab as a scene instance while keeping connection to prefab
            GameObject newObj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (newObj == null)
            {
                // Fallback to regular Instantiate
                newObj = (GameObject)Object.Instantiate(prefab);
            }

            Transform nt = newObj.transform;

            // Keep hierarchy parent and world transform
            nt.SetParent(target.transform.parent, true);
            nt.position = target.transform.position;
            nt.rotation = target.transform.rotation;
            nt.localScale = target.transform.localScale;

            // Name the instance after prefab with sequential postfix: _0, _1, _2 ...
            string baseName = prefab.name;
            int attempt = createdIndex;
            string newName;
            do
            {
                newName = baseName + "_" + attempt;
                attempt++;
            } while (existingNames.Contains(newName));
            newObj.name = newName;
            existingNames.Add(newName);
            createdIndex = attempt;

            Undo.RegisterCreatedObjectUndo(newObj, "Replace With Prefab");
            Undo.DestroyObjectImmediate(target);
        }
    }
}
