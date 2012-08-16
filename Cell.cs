using System.Collections.Generic;
using System.Linq;

namespace VeeCollision
{
    public class Cell
    {
        public Cell(IEnumerable<int> mGroups, int mLeft, int mRight, int mTop, int mBottom)
        {
            Left = mLeft;
            Right = mRight;
            Top = mTop;
            Bottom = mBottom;

            GroupedBodies = new Dictionary<int, List<CBody>>();
            foreach (var group in mGroups) GroupedBodies.Add(group, new List<CBody>());
        }

        public int Left { get; private set; }
        public int Right { get; private set; }
        public int Top { get; private set; }
        public int Bottom { get; private set; }

        public Dictionary<int, List<CBody>> GroupedBodies { get; private set; }

        public void AddBody(CBody mBody) { foreach (var group in mBody.Groups) GroupedBodies[group].Add(mBody); }
        public void RemoveBody(CBody mBody) { foreach (var group in mBody.Groups) GroupedBodies[group].Remove(mBody); }
        public bool HasGroup(int mGroup) { return GroupedBodies[mGroup].Any(); }
    }
}