using App.LifetimeScopes;
using UnityEngine;
using VContainer;

namespace App.PortableRadioSystem
{
	public class PortableRadioScopeConfig : ScopeConfig
	{
		[SerializeField] private PortableRadio portableRadio;
		
		public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
		{
			builder.RegisterInstance(portableRadio);
		}
	}
}