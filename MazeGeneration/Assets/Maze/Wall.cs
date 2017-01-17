using UnityEngine;
using System.Collections.Generic;

public class Wall {

	private static bool _static_initialized_wall = false;
	private static Dictionary<Direction, Quaternion> Rotations = new Dictionary<Direction, Quaternion>();

	private static void static_init() {
		if (_static_initialized_wall) { return; }
		_static_initialized_wall = true;

		// precalc rotations
		foreach (Direction d in Direction.All) {
			// determine axis to rotate the wall against
			Vector3 axis = Vector3.Cross (Vector3.forward, d);
			if (axis == Vector3.zero) { axis = Vector3.right; }

			// angle to rotate
			float angle = Vector3.Angle (Vector3.forward, d);

			// save rotation
			Rotations[d] = Quaternion.AngleAxis(angle, axis);
		}
	}


	public Direction Axis { get; private set; }
	public Vector3 Pos { get; private set; }
	public Quaternion Rot { get; private set; }

	public Room[] Rooms { get; private set; }

	public Wall (Direction axis, Room r1, Room r2) {
		static_init();

		Axis = axis;
		Rooms = new Room[]{ r1, r2 };
		Pos = r1.Pos + (Vector3) Axis * 0.5f;
		Rot = Rotations[Axis];
	}

	public bool IsConnected(Room r) { return IsIncident (r); }
	public bool IsConnected(Wall w) { return IsTouching (w); }


	/// <summary>
	/// does the room have this wall connected?
	/// </summary>
	public bool IsIncident(Room r) {
		return (r == Rooms[0]) || (r == Rooms[1]);
	}

	/// <summary>
	/// is the wall connected to the same room?
	/// </summary>
	public bool IsAdjacent(Wall w) {
		return w.IsIncident(Rooms[0]) || w.IsIncident(Rooms[1]);
	}

	/// <summary>
	/// is there a direct contact to the wall
	/// </summary>
	public bool IsTouching(Wall w) {
		return Axis != w.Axis && IsAdjacent(w);
	}

	public Room OtherRoom(Room r) {
		if (r == Rooms[0]) return Rooms[1];
		if (r == Rooms[1]) return Rooms[0];
		return null;
	}

}
