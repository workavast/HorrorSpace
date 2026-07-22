using System;
using System.Linq;
using App.IsolationBlock.Cells;
using App.Usable;
using UnityEngine;

namespace App.IsolationBlock
{
    public class CellsController : MonoBehaviour
    {
        [SerializeField] private UsableObj usableObj;
        [SerializeField] private CellsWrapper cells;
        [SerializeField] private IsolationTable[] tables;

        private bool CellsIsOpen => !cells.CellsIsClosed;

        public void Initialize()
        {
            cells.Initialize();
            TryToggleDoors(false);
            
            usableObj.AddExternalUsableCheck(CanToggleDoors);
            usableObj.OnUse += ToggleDoors;
        }

        private void OnDestroy()
        {
            usableObj.OnUse -= ToggleDoors;
        }

        private void TryToggleDoors(bool openCells)
        {
            if (CanToggleDoors(openCells))
                ToggleDoors(openCells);
        }
        
        private bool CanToggleDoors() 
            => CanToggleDoors(!CellsIsOpen);

        private bool CanToggleDoors(bool isOpen)
        {
            if (!isOpen && !cells.AllCellsCanBeClosed())
                return false;
            
            return true;
        }

        private void ToggleDoors() 
            => ToggleDoors(!CellsIsOpen);

        private void ToggleDoors(bool openCells)
        {
            cells.SetState(openCells);
            foreach (var table in tables)
                table.SetState(openCells);
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