using UnityEngine;
using VContainer;

namespace App.Installers
{
	public abstract class Installer : MonoBehaviour
	{
		public abstract void Configure(CoreLifetimeScope scope, IContainerBuilder builder);
	}
}