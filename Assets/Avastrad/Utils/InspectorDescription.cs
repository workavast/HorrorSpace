using UnityEngine;

namespace Avastrad.Utils
{
    public class InspectorDescription : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, TextArea(5, 20)] private string description;
#endif
    }
}
