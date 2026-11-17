using System.Collections.Generic;

namespace Avastrad.UI.UiSystem
{
    internal sealed class ScreenHistory
    {
        private readonly Stack<List<Item>> _snapshots = new();

        public bool CanUndo => _snapshots.Count > 0;
        public void Push(List<Item> snapshot) => _snapshots.Push(snapshot);
        public List<Item> Peek() => _snapshots.Peek();
        public void Pop() => _snapshots.Pop();
        
        internal readonly struct Item
        {
            public readonly ScreenRecord Record;
            public readonly int Order;

            public Item(ScreenRecord record, int order)
            {
                Record = record;
                Order = order;
            }
        }
    }
}
