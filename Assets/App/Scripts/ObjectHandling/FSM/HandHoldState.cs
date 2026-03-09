namespace App.ObjectHandling
{
    public class HandHoldState : HandState
    {
        public HandHoldState(HandContext context, IHandParams parameters) 
            : base(context, parameters)
        {
        }

        public override void Enter(string[] args)
        {
            _context.CursorViewToggler.SetState(true);
            _context.DraggedObjMass = 0;
        }

        public override void Exit()
        {
        }

        public override void ManualUpdate()
        {
            if (!_context.HasHandledObject)
            {
                SetState<HandEmptyState>();
                return;
            }
            
            if (!_context.InputManager.IsPickupPressed || !ObjectInHoldRange())
            {
                DropObject();
                SetState<HandEmptyState>();
                return;
            }

            if (_context.InputManager.IsRotatePressed)
            {
                SetState<HandRotateState>();
                return;
            }

            HoldObject();
        }

        public override void ManualFixedUpdate()
        {
        }
    }
}