using System;
using System.Collections.Generic;
using UnityEngine;

namespace Avastrad.Bootstraps
{
    [DefaultExecutionOrder(-4000)]
    public class RootBootstrap : MonoBehaviour
    {
        public void Awake()
        {
            var bootstraps = GetChildrenBootstraps(rb => rb._order == Order.Awake);
            foreach (var bootstrap in bootstraps)
                bootstrap.StartInitialization();
        }

        public void Start()
        {
            var bootstraps = GetChildrenBootstraps(rb => rb._order == Order.Start);
            foreach (var bootstrap in bootstraps)
                bootstrap.StartInitialization();
        }

        private List<RootOrderedBootstrap> GetChildrenBootstraps(Func<RootOrderedBootstrap, bool> filter)
        {
            var children = new List<RootOrderedBootstrap>(gameObject.transform.childCount);
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                if (child.gameObject.activeSelf && child.TryGetComponent<RootOrderedBootstrap>(out var childBootstrap) && filter(childBootstrap)) 
                    children.Add(childBootstrap);
            }

            return children;
        }
    }
}