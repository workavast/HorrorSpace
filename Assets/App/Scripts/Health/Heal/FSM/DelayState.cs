using System.Collections.Generic;
using UnityEngine;

namespace App.Health.Heal.FSM
{
    public class DelayState : HealStateBase
    {
        private float _delay;
        
        public DelayState(HealContext ctx, HealParams @params) : base(ctx, @params)
        {
        }

        public override void Enter(Dictionary<string, object> args)
        {
            _delay = _params.DelayBeforeHealing;
        }

        public override void Exit()
        {
        }

        public override void ManualUpdate()
        {
            _delay -= Time.deltaTime;

            if (_delay <= 0f)
            {
                SetState<ApplyHealState>();
                return;
            }
        }

        public override void ManualFixedUpdate()
        {
        }
    }
}