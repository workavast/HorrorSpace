using System;
using UnityEngine;

namespace Avastrad.Utils.PolymorphicAttributeDrawing
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class PolymorphicAttribute : PropertyAttribute { }
}