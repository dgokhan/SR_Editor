/*
<copyright file="BGCalcFlowContext.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Graph execution context
    /// </summary>
    public class BGCalcFlowContext
    {
        //thread-safe pool for graph context
        private static readonly BGObjectPool<BGCalcFlowContext> contextPool = new BGObjectPool<BGCalcFlowContext>(() => new BGCalcFlowContext(), OnContextReturn);

        private static readonly BGObjectPool<BGCellPointer> cellsPool = new BGObjectPool<BGCellPointer>(() => new BGCellPointer(), pointer => pointer.Reset());

        private readonly List<BGCellPointer> cells = new List<BGCellPointer>();

        /// <summary>
        /// Graph to execute
        /// </summary>
        public BGCalcGraph Graph;

        /// <summary>
        /// overridden graph variables 
        /// </summary>
        public BGCalcVarsProvider VarsOverrides;

        /// <summary>
        /// Current entity if available
        /// </summary>
        public BGEntity CurrentEntity;

        /// <summary>
        /// Current GameObject if available
        /// </summary>
        public GameObject CurrentGameObject;

        /// <summary>
        /// Graph's type
        /// </summary>
        public BGCalcGraphTypeEnum GraphType;

        /// <summary>
        /// Graph events
        /// </summary>
        public BGCalcFlowEvents Events;

        private BGCalcFlowContext()
        {
        }

        public void AddCell(BGField field, BGEntity entity)
        {
            var pointer = cellsPool.Get();
            pointer.Reset(field, entity);
            cells.Add(pointer);
        }

        public bool ContainsCell(BGField field, BGEntity entity)
        {
            var pointer = cellsPool.Get();
            try
            {
                pointer.Reset(field, entity);
                return cells.Contains(pointer); 
            }
            finally
            {
                cellsPool.Return(pointer);
            }
        }

        public void Reset()
        {
            foreach (var cell in cells) cellsPool.Return(cell);
            Graph = null;
            CurrentEntity = null;
            VarsOverrides = null;
            GraphType = BGCalcGraphTypeEnum.CalculatedField;
            cells.Clear();
        }

        public void CopyCellsFrom(BGCalcFlowContext context)
        {
            foreach (var cell in context.cells)
            {
                var cellClone = cellsPool.Get();
                cellClone.MetaId = cell.MetaId;
                cellClone.FieldId = cell.FieldId;
                cellClone.EntityId = cell.EntityId;
                cells.Add(cellClone);
            }
        }
        
        //clear internal state on context returned to pool
        private static void OnContextReturn(BGCalcFlowContext context) => context.Reset();

        //can we use IDisposable and using statement instead?
        public static BGCalcFlowContext Get() => contextPool.Get();
        public static void Return(BGCalcFlowContext context) => contextPool.Return(context);
    }
}