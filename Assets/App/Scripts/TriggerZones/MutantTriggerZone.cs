using App.Mutant;

namespace App.TriggerZones
{
	public class MutantTriggerZone : TriggerZoneProviderT<IMutantTarget>
	{
		public IMutantTarget CurrentTarget { get; private set; }

		private void Awake()
		{
			OnEnter += target => CurrentTarget = target;
			OnExit += target =>
			{
				if (ReferenceEquals(CurrentTarget, target))
					CurrentTarget = null;
			};
		}
	}
}
