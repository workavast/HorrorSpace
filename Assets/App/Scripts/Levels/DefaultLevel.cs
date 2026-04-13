using UnityEngine;
using VContainer;

namespace App.Levels
{
    public class DefaultLevel : MonoBehaviour, ILevel
    {
        [SerializeField] private InterfaceReference<IContainerInitializable>[] containerInitializable;
        
        public void Initialize(IObjectResolver container)
        {
            foreach (var initializable in containerInitializable) 
                initializable.Value.Initialize(container);
        }
    }
}