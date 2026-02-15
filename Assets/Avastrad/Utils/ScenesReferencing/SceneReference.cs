using Avastrad.Utils.ScenesReferencing.ReadOnlyAttributeDrawing;
using UnityEngine;

namespace Avastrad.Utils.ScenesReferencing
{
    [CreateAssetMenu(fileName = "SceneReference", menuName = "App/" + nameof(SceneReference))]
    public partial class SceneReference : ScriptableObject
    {
        [SerializeField, ReadOnly] private int bakedSceneIndex = -1;

        public int SceneIndex => bakedSceneIndex;
    }
}