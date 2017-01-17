using System;

/// <summary>
/// Transforms a given Maze to be consisted entirely out of walls
/// </summary>
public class TMFull : MazeTransformator {

	private void connect(Room r, Room rx, Direction d) {
		Wall wx = new Wall(d, rx, r);
		rx.set (wx, d);
		r.set (wx, d.Opposite);
	}

	public void Transform (Random rnd, Maze m)
	{
		for (int x = 0; x < m.Width; x++) {
			for (int y = 0; y < m.Height; y++) {
				for (int z = 0; z < m.Depth; z++) {
					Room r = m.Rooms[x, y, z];

					if (x > 0) {
						connect(r, m.Rooms[x - 1, y, z], Direction.X);
					}
					if (y > 0) {
						connect(r, m.Rooms[x, y - 1, z], Direction.Y);
					}
					if (z > 0) {
						connect(r, m.Rooms[x, y, z - 1], Direction.Z);
					}
				}
			}
		}
	}

	// singleton
	private TMFull(){}
	private static TMFull instance;
	public static TMFull I {
		get {
			if (instance == null) { instance = new TMFull(); }
			return instance;
		}
	}

}
