using App.TriggerZones;

namespace App.Mutant
{
	public class MutantTargetTriggerZone : TriggerZoneProviderT<IMutantTarget>
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
