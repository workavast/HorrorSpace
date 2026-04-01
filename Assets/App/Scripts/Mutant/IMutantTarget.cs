using UnityEngine;

namespace App.Mutant
{
    public interface IMutantTarget
    {
        public int Priority { get; }
        public MutantTargetType Type { get; }
        Vector3 Position { get; }
        Vector3 MiddlePoint { get; }
    }

    public enum MutantTargetType
    {
        See,
        Smell
    }
}
