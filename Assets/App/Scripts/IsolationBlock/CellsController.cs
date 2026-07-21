using System;
using System.Linq;
using App.IsolationBlock.Cells;
using App.Usable;
using UnityEngine;

namespace App.IsolationBlock
{
    public class CellsController : MonoBehaviour, IUsable
    {
        [SerializeField] private CellsWrapper cells;
        [SerializeField] private IsolationTable[] tables;

        private bool CellsIsOpen => !cells.CellsIsClosed;
        public bool IsUsable { get; private set; } = true;

        public event Action OnUse;
        public event Action<bool> OnTryUse;
        public event Action OnUsableStateChanged;

        public void Initialize()
        {
            cells.Initialize();
            SetState(false);
        }

        public void Interact()
        {
            if (!IsUsable)
            {
                OnTryUse?.Invoke(false);
                return;
            }

            if (!SetState(!CellsIsOpen))
            {
                OnTryUse?.Invoke(false);
                return;
            }

            OnUse?.Invoke();
            OnTryUse?.Invoke(true);
            OnUsableStateChanged?.Invoke();
        }

        private bool SetState(bool isOpen)
        {
            if (!isOpen && !cells.AllCellsCanBeClosed())
                return false;

            cells.SetState(isOpen);
            foreach (var table in tables)
                table.SetState(isOpen);

            return true;
        }

        [Serializable]
        private class CellsWrapper
        {
            [SerializeField] private IsolationCell[] cells;

            public bool CellsIsClosed => cells.All(cell => cell.IsClosed);

            public void Initialize()
            {
                foreach (var cell in cells)
                    cell.Initialize();
            }

            public bool AllCellsCanBeClosed()
            {
                foreach (var cell in cells)
                    if (cell.HasObstacles())
                        return false;

                return true;
            }

            public void SetState(bool isOpen)
            {
                foreach (var cell in cells)
                    cell.SetState(isOpen);
            }
        }
    }
}