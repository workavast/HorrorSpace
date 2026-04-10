using System.Collections.Generic;
using UnityEngine;

namespace App.Health.Heal.FSM
{
    public class ApplyHealState : HealStateBase
    {
        private float _timer;
        
        public ApplyHealState(HealContext ctx, HealParams @params) : base(ctx, @params)
        {
        }

        public override void Enter(Dictionary<string, object> args)
        {
            _timer = 0;
        }

        public override void Exit()
        {
            
        }

        public override void ManualUpdate()
        {
            _timer += Time.deltaTime;
            
            if (_timer >= _params.FrequencyInSeconds)
            {
                _ctx.HealthModel.Heal(_params.HealPerSecond * _timer);
                _timer = 0;
            }
        }

        public override void ManualFixedUpdate()
        {
        }
    }
}