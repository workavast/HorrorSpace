using System.Collections.Generic;
using App.HandlingObjects.HoldableObjects;

namespace App.Character.Hand.FSM
{
    public class HandHoldState : HandState
    {
        public HandHoldState(HandContext ctx, IHandParams parameters) 
            : base(ctx, parameters)
        {
        }

        public override void Enter(Dictionary<string, object> args)
        {
            _ctx.CursorViewToggler.SetState(true);
            _ctx.DraggedObjMass = 0;
        }

        public override void Exit()
        {
        }

        public override void ManualUpdate()
        {
            if (!_ctx.HasHandledObject)
            {
                SetState<HandEmptyState>();
                return;
            }
            
            if (!_ctx.InputManager.IsPickupPressed || !ObjInHandleRange(_ctx.HandledObj.Transform))
            {
                DropObject();
                SetState<HandEmptyState>();
                return;
            }

            if (_ctx.InputManager.IsRotatePressed)
            {
                SetState<HandRotateState>();
                return;
            }
        }

        public override void ManualFixedUpdate()
        {
            HoldObject(_ctx.HandledObj as IHoldableObj);
        }
    }
}