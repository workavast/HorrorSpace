using System;
using System.Collections.Generic;
using UnityEngine;

namespace Avastrad.Bootstraps
{
    public static class BootstrapUtils
    {
        public static List<T> GetChildrenBootstraps<T>(this GameObject gameObject, Func<T, bool> filter)
            where T : Bootstrap
        {
            var children = new List<T>(gameObject.transform.childCount);
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                if (child.gameObject.activeSelf && child.TryGetComponent<T>(out var childBootstrap) && filter(childBootstrap)) 
                    children.Add(childBootstrap);
            }

            return children;
        }
    }
}