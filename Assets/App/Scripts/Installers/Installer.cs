using UnityEngine;
using VContainer;

namespace App.LifetimeScopes
{
	public abstract class Installer : MonoBehaviour
	{
		public abstract void Configure(CoreLifetimeScope scope, IContainerBuilder builder);
	}
}