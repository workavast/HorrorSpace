using Avastrad.FSM;

namespace App.Character.FSM.Jump
{
    public abstract class JumpStateBase : StateBase
    {
        protected readonly PlayerParams _params;
        protected readonly IReadOnlyInputProvider _input;
        protected readonly PlayerContext _ctx;

        protected JumpStateBase(PlayerParams p, IReadOnlyInputProvider input, PlayerContext ctx)
        {
            _params = p;
            _input = input;
            _ctx = ctx;
        }
    }
}
