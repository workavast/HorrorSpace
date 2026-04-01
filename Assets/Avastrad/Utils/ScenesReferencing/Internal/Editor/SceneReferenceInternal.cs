#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Avastrad.Utils.ScenesReferencing
{
    public partial class SceneReference
    {
        [SerializeField] private SceneAsset sceneAsset;
        
        internal void OnValidate()
        {
            if (sceneAsset != null)
            {
                var scenePath = AssetDatabase.GetAssetPath(sceneAsset);
                bakedSceneIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);

                if (bakedSceneIndex == -1)
                {
                    Debug.LogWarning($"Scene [{sceneAsset.name}] can't be found. Maybe it doesn't add into Scenes List in Build Settings?");
                }
            }
            else
            {
                bakedSceneIndex = -1;
            }
        }
    }
}
#endif