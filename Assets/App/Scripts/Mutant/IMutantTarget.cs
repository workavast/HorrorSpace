using UnityEngine;

namespace App.Mutant
{
    public interface IMutantTarget
    {
        Vector3 Position { get; }
        Vector3 MiddlePoint { get; }
    }
}
