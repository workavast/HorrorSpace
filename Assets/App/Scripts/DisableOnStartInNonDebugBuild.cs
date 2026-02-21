using UnityEngine;

namespace App
{
    public class DisableOnStartInNonDebugBuild : MonoBehaviour
    {
        private void Start()
        {
#if !UNITY_EDITOR && !DEBUG
            gameObject.SetActive(false);
#endif
        }
    }
}