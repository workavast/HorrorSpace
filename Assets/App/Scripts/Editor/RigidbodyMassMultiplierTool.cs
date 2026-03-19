using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor tool to multiply Rigidbody.mass values for a target object and all its children.
/// Usage: Tools > Rigidbody Mass Multiplier
/// </summary>
public class RigidbodyMassMultiplierTool : EditorWindow
{
    private GameObject targetObject;
    private float massMultiplier = 1.0f;

    [MenuItem("Tools/Rigidbody Mass Multiplier")]
    public static void ShowWindow()
    {
        GetWindow<RigidbodyMassMultiplierTool>("Rigidbody Mass Multiplier");
    }

    private void OnGUI()
    {
        GUILayout.Label("Multiply Rigidbody Mass", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);
        massMultiplier = EditorGUILayout.FloatField("Mass Multiplier", massMultiplier);

        if (GUILayout.Button("Use Selected GameObject"))
        {
            if (Selection.activeGameObject != null)
            {
                targetObject = Selection.activeGameObject;
            }
            else
            {
                EditorUtility.DisplayDialog("No Selection", "Please select a GameObject in the hierarchy first.", "OK");
            }
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Apply Mass Multiplier"))
        {
            ApplyMassMultiplier();
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("This tool multiplies the mass value of every Rigidbody in the target object and all descendants.\n" +
                                "Set multiplier to 1.0 to keep same mass, < 1 to reduce, > 1 to increase.", MessageType.Info);
    }

    private void ApplyMassMultiplier()
    {
        if (targetObject == null)
        {
            EditorUtility.DisplayDialog("Error", "Target object is not assigned.", "OK");
            return;
        }

        if (Mathf.Approximately(massMultiplier, 1f))
        {
            if (!EditorUtility.DisplayDialog("Confirm Multiplier 1", "Multiplier is 1.0. This will not change masses. Continue?", "Yes", "Cancel"))
                return;
        }

        Rigidbody[] rigidbodies = targetObject.GetComponentsInChildren<Rigidbody>(true);

        if (rigidbodies.Length == 0)
        {
            EditorUtility.DisplayDialog("No Rigidbody", "No Rigidbody components found on target object or its children.", "OK");
            return;
        }

        Undo.RecordObjects(rigidbodies, "Multiply Rigidbody Mass");

        int changedCount = 0;

        foreach (Rigidbody rb in rigidbodies)
        {
            if (rb == null)
                continue;

            float oldMass = rb.mass;
            rb.mass = oldMass * massMultiplier;
            EditorUtility.SetDirty(rb);
            changedCount++;
        }

        // If editing prefab asset object in project, save those changes as well.
        if (AssetDatabase.Contains(targetObject))
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        EditorUtility.DisplayDialog("Done", $"Updated {changedCount} Rigidbody(s).\nMultiplier: {massMultiplier}", "OK");
        Debug.Log($"[RigidbodyMassMultiplierTool] Updated {changedCount} Rigidbody(s) on '{targetObject.name}' with multiplier {massMultiplier}.");
    }
}