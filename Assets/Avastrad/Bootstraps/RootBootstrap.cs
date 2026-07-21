using UnityEngine;

namespace Avastrad.Bootstraps
{
    [DefaultExecutionOrder(-4000)]
    public class RootBootstrap : MonoBehaviour
    {
        public void Awake()
        {
            var bootstraps = gameObject.GetChildrenBootstraps<RootOrderedBootstrap>
                (rb => rb._order == Order.Awake);
            foreach (var bootstrap in bootstraps)
                bootstrap.StartInitialization();
        }

        public void Start()
        {
            var bootstraps = gameObject.GetChildrenBootstraps<RootOrderedBootstrap>
                (rb => rb._order == Order.Start);
            foreach (var bootstrap in bootstraps)
                bootstrap.StartInitialization();
        }
    }
}