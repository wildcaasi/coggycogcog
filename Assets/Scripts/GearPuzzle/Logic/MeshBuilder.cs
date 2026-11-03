using System.Collections.Generic;

namespace GearPuzzle
{
    public sealed class MeshBuilder
    {
        private readonly GearGeometry geometry;

        public MeshBuilder(GearGeometry geometry)
        {
            this.geometry = geometry;
        }

        public Dictionary<Gear, List<Gear>> Build(GridBoard board)
        {
            var gears = new List<Gear>(board.All);
            var graph = new Dictionary<Gear, List<Gear>>(gears.Count);

            foreach (Gear gear in gears)
            {
                graph[gear] = new List<Gear>();
            }

            for (int i = 0; i < gears.Count; i++)
            {
                for (int j = i + 1; j < gears.Count; j++)
                {
                    if (geometry.Classify(gears[i], gears[j]) != GearContact.Meshed) continue;
                    graph[gears[i]].Add(gears[j]);
                    graph[gears[j]].Add(gears[i]);
                }
            }

            return graph;
        }
    }
}
