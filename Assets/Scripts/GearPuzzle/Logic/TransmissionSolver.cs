using System;
using System.Collections.Generic;

namespace GearPuzzle
{
    public sealed class TransmissionSolver
    {
        public Dictionary<Gear, GearState> Solve(
            Dictionary<Gear, List<Gear>> graph,
            Gear driver,
            GearState driverDirection)
        {
            var states = new Dictionary<Gear, GearState>(graph.Count);
            foreach (Gear gear in graph.Keys)
            {
                states[gear] = GearState.NotMoving;
            }

            if (!graph.ContainsKey(driver)) return states;

            var directions = new Dictionary<Gear, GearState> { [driver] = driverDirection };
            var queue = new Queue<Gear>();
            queue.Enqueue(driver);
            bool jammed = false;

            while (queue.Count > 0)
            {
                Gear current = queue.Dequeue();
                GearState expected = Opposite(directions[current]);

                foreach (Gear neighbour in graph[current])
                {
                    if (directions.TryGetValue(neighbour, out GearState existing))
                    {
                        if (existing != expected) jammed = true;
                        continue;
                    }

                    directions[neighbour] = expected;
                    queue.Enqueue(neighbour);
                }
            }

            if (jammed) return states;

            foreach (KeyValuePair<Gear, GearState> pair in directions)
            {
                states[pair.Key] = pair.Value;
            }

            return states;
        }

        private GearState Opposite(GearState direction)
        {
            switch (direction)
            {
                case GearState.Clockwise: return GearState.CounterClockwise;
                case GearState.CounterClockwise: return GearState.Clockwise;
                default: throw new ArgumentException("Only spinning states have an opposite.", nameof(direction));
            }
        }
    }
}
