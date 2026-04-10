using System.Collections.Generic;
using App.Health.Heal.FSM;
using Avastrad.FSM;
using UnityEngine;

namespace App.Health.Heal
{
    public class HealthHeal : MonoBehaviour
    {
        [SerializeField] private HealParams healthParams;
        [SerializeField] private HealthModel healthModel;

        private HealContext _ctx;
        private StateMachine _fsm;

        private void Awake() 
            => Initialize();

        public void Initialize()
        {
            healthModel.OnTakeDamage += SetDelayState;

            _ctx = new HealContext(healthModel);
            var states = new List<StateBase>()
            {
                new ApplyHealState(_ctx, healthParams),
                new DelayState(_ctx, healthParams),
            };

            _fsm = new StateMachine(states);
            _fsm.Init<DelayState>();
        }
        
        private void OnDestroy()
        {
            healthModel.OnTakeDamage += SetDelayState;
        }

        private void Update() 
            => _fsm.Update();

        private void FixedUpdate() 
            => _fsm.FixedUpdate();

        private void SetDelayState() 
            => _fsm.SetState<DelayState>();
    }
}