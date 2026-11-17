using UnityEngine;
using VContainer;

namespace Avastrad.VContainerExt.Installers
{
	public abstract class Installer : MonoBehaviour
	{
		public abstract void Configure(BaseLifetimeScope scope, IContainerBuilder builder);
	}
}