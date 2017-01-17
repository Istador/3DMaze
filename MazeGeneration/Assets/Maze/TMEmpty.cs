using System;

/// <summary>
/// Transforms a given Maze to be completely without walls
/// </summary>
public class TMEmpty : MazeTransformator {

	private void connect(Room r, Room rx, Direction d) {
		rx.set (r, d);
		r.set (rx, d.Opposite);
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
	private TMEmpty(){}
	private static TMEmpty instance;
	public static TMEmpty I {
		get {
			if (instance == null) { instance = new TMEmpty(); }
			return instance;
		}
	}
}
