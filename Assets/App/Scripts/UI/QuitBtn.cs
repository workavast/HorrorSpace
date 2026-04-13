using UnityEngine;
using UnityEngine.UI;

namespace App.UI
{
    [RequireComponent(typeof(Button))]
    public class QuitBtn : MonoBehaviour
    {
        private void Awake() 
            => GetComponent<Button>().onClick.AddListener(Application.Quit);
    }
}