using UnityEngine;
using System.Collections;

public class Room {
	public Maze Maze { get; private set; }
	public Wall[] Walls { get; private set; }
	public Room[] Rooms { get; private set; }
	public Vector3 Pos { get; private set; }

	public Room(Vector3 pos) {
		Pos = pos;
		Walls = new Wall[6];
		Rooms = new Room[6];
	}

	public void set(Room r, Direction o) { Rooms[o] = r; Walls[o] = null; }
	public void set(Wall w, Direction o) { Walls[o] = w; Rooms[o] = null; }


	public Direction Direction(Room r) {
		for (int d = 0; d < Rooms.Length ; d++) {
			Room _r = Rooms[d];
			if (_r != null && _r == r) {
				return d;
			}
		}
		return null;
	}

	public Direction Direction(Wall w) {
		for (int d = 0; d < Walls.Length ; d++) {
			Wall _w = Walls[d];
			if (_w != null && _w == w) {
				return d;
			}
		}
		return null;
	}


	public bool IsConnected(Room r) { return IsAdjacent (r); }
	public bool IsConnected(Wall w) { return IsIncident (w); }


	/// <summary>
	/// is it one of the walls of this room
	/// </summary>
	public bool IsIncident(Wall w) {
		return w.IsIncident (this);
	}

	/// <summary>
	/// is the room directly connected to this room (without a wall between)?
	/// </summary>
	public bool IsAdjacent(Room r) {
		foreach ( Room _r in Rooms ) {
			if (_r == r) { return true; }
		}
		return false;
	}

	/// <summary>
	/// is there a wall between the room and this room?
	/// </summary>
	public bool IsNeighbor(Room r) {
		foreach ( Wall w in Walls ) {
			Room _r = w.OtherRoom (this);
			if (_r == r) { return true; }
		}
		return false;
	}

	public override string ToString() {
		return "Room(" + Pos.x + "," + Pos.y + "," + Pos.z + ")";
	}
}
