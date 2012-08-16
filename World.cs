using System.Collections.Generic;

namespace VeeCollision
{
    public class World
    {
        private readonly Cell[,] _cells;
        private readonly int _columns, _rows, _cellSize, _offset;
        private readonly IEnumerable<int> _groups;

        public World(IEnumerable<int> mGroups, int mColumns, int mRows, int mCellSize, int mOffset = 0)
        {
            _groups = mGroups;
            _cells = new Cell[mColumns,mRows];
            _columns = mColumns;
            _rows = mRows;
            _cellSize = mCellSize;
            _offset = mOffset;

            for (var iX = 0; iX < mColumns; iX++)
                for (var iY = 0; iY < mRows; iY++)
                {
                    var left = iX * mCellSize;
                    var right = _cellSize + left;
                    var top = iY * mCellSize;
                    var bottom = _cellSize + top;
                    
                    _cells[iX, iY] = new Cell(_groups, left, right, top, bottom);
                }
        }

        private HashSet<Cell> CalculateCells(CBody mBody)
        {
            var startX = mBody.Left /_cellSize + _offset;
            var startY = mBody.Top / _cellSize + _offset;
            var endX = mBody.Right / _cellSize + _offset;
            var endY = mBody.Bottom / _cellSize + _offset;

            var result = new HashSet<Cell>();

            if (startX < 0 || endX >= _columns || startY < 0 || endY >= _rows)
            {
                mBody.Entity.Destroy();
                return result;
            }

            for (var iY = startY; iY <= endY; iY++)
                for (var iX = startX; iX <= endX; iX++)
                    result.Add(_cells[iX, iY]);

            return result;
        }

        public void AddBody(CBody mCBody)
        {
            mCBody.Cells = CalculateCells(mCBody);
            foreach (var cell in mCBody.Cells) cell.AddBody(mCBody);
        }
        public void RemoveBody(CBody mCBody) { foreach (var cell in mCBody.Cells) cell.RemoveBody(mCBody); }
        public void UpdateBody(CBody mCBody)
        {
            RemoveBody(mCBody);
            AddBody(mCBody);
        }

        public static List<CBody> GetBodies(CBody mCBody)
        {
            var result = new List<CBody>();

            foreach (var cell in mCBody.Cells)
                foreach (var group in mCBody.GroupsToCheck)
                    result.AddRange(cell.GroupedBodies[group]);

            return result;
        }

        public bool[,] GetObstacleMap(int mObstacleGroup)
        {
            var result = new bool[_columns,_rows];

            for (var iX = 0; iX < _columns; iX++)
                for (var iY = 0; iY < _rows; iY++)
                    result[iX, iY] = _cells[iX, iY].HasGroup(mObstacleGroup);

            return result;
        }
    }
}