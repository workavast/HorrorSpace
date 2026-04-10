using Avastrad.FSM;

namespace App.Health.Heal.FSM
{
    public abstract class HealStateBase : StateBase
    {
        protected readonly HealParams _params;
        protected readonly HealContext _ctx;

        protected HealStateBase(HealContext ctx, HealParams @params)
        {
            _ctx = ctx;
            _params = @params;
        }
    }
}