using System;
using System.Collections.Generic;

/// <summary>
/// Randomized Prim's algorithm
/// </summary>
public class TMPrim : MazeTransformator {
	
	public void Transform (Random rnd, Maze m)
	{
		// variables
		List<Wall> walls = new List<Wall> (); // list of walls to process
		HashSet<Room> Visited = new HashSet<Room>(); // already visited rooms

		// 1. Start with a grid full of walls
		TMFull.I.Transform(rnd, m);

		// 2. Pick a cell, mark it as part of the maze. Add the walls of the cell to the wall list.
		Room start = m.Rooms[rnd.Next(m.Width), rnd.Next(m.Height), rnd.Next(m.Depth)];

		Visited.Add(start);
		foreach (Wall w in start.Walls) { if (w != null) { walls.Add (w); } }

		// 3. while there are walls in the list:
		while (walls.Count != 0) {
			
			// 3.1. Pick a random wall from the list. ...
			int i = rnd.Next (walls.Count);
			Wall w = walls[i];
			Room ra = w.Rooms[0];
			Room rb = w.Rooms[1];
			// ... If only one of the two cells that the wall divides is visited, then:
			bool ia = Visited.Contains(ra);
			bool ib = Visited.Contains(rb);
			if ((ia && ! ib) || (! ia && ib)) {
				
				// 3.2.1 make the wall a passage and mark the unvisited cell as part of the maze.
				Direction da = (ra.Walls [w.Axis] == w ? w.Axis : w.Axis.Opposite);
				Direction db = da.Opposite;
				ra.set (rb, da);
				rb.set (ra, db);
				Visited.Add(ra);
				Visited.Add(rb);

				// 3.2.2 add the neighboring walls to the wall list.
				if (ia) {
					foreach (Wall wb in rb.Walls) { if (wb != null && wb != w) { walls.Add (wb); } }
				} else {
					foreach (Wall wa in ra.Walls) { if (wa != null && wa != w) { walls.Add (wa); } }
				}
			}

			// 3.2. Remove the wall from the list
			walls.RemoveAt(i);
		}

	}

	// singleton
	private TMPrim(){}
	private static TMPrim instance;
	public static TMPrim I {
		get {
			if (instance == null) { instance = new TMPrim(); }
			return instance;
		}
	}
}
