using UnityEngine;

namespace App.Mutant
{
    public interface IMutantTarget
    {
        public int Priority { get; }
        public MutantTargetType Type { get; }
        public GameObject gameObject { get; }
        public Vector3 Position { get; }
        public Vector3 MiddlePoint { get; }
    }

    public enum MutantTargetType
    {
        See,
        Smell
    }
}
