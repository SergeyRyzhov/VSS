﻿namespace Buddy.Common.Structures
{
    public interface IGraph
    {
        uint[] Indexes { get; }
    
        double[] Radius { get; }
        double[] Weight { get; }

        uint[] ColumnIndex { get; }
        uint[] RowIndex { get; }

        uint EdgesAmount { get; }

        uint VerticesAmount { get; }

        /// <summary>
        /// Обновление внутренних структур
        /// </summary>
        void Update();
    }
}