using UnityEngine;
using System.Collections.Generic;

public sealed class Direction {
	
	public static readonly Direction Left  = new Direction(Vector3.left);
	public static readonly Direction Right = new Direction(Vector3.right);

	public static readonly Direction Down = new Direction(Vector3.down);
	public static readonly Direction Up   = new Direction(Vector3.up);

	public static readonly Direction Forward  = new Direction(Vector3.forward);
	public static readonly Direction Backward = new Direction(Vector3.back);

	public static readonly Direction X = Right;
	public static readonly Direction Y = Up;
	public static readonly Direction Z = Forward;

	private Vector3 v;
	private Direction(Vector3 v) { this.v = v; }

	public static implicit operator Vector3(Direction d) { return d.v; }

	public static Vector3 Vector(float x, float y, float z) {
		return X.v * x + Y.v * y + Z.v * z;
	}

	public static implicit operator int(Direction d) {
		if (d == Left) return 0;
		if (d == Right) return 1;
		if (d == Down) return 2;
		if (d == Up) return 3;
		if (d == Forward) return 4;
		if (d == Backward) return 5;
		return -1;
	}

	public static implicit operator Direction(int i) {
		switch (i) {
		case 0: return Left;
		case 1: return Right;
		case 2: return Down;
		case 3: return Up;
		case 4: return Forward;
		case 5: return Backward;
		default: return null;
		}
	}

	public static Direction OppositeOf(Direction d) {
		if (d == Left) return Right;
		if (d == Right) return Left;
		if (d == Down) return Up;
		if (d == Up) return Down;
		if (d == Forward) return Backward;
		if (d == Backward) return Forward;
		return null;
	}


	public static implicit operator string(Direction d) {
		if (d == Left) return "Left";
		if (d == Right) return "Right";
		if (d == Down) return "Down";
		if (d == Up) return "Up";
		if (d == Forward) return "Forward";
		if (d == Backward) return "Backward";
		return "Unknown";
	}

	public override string ToString() {
		return this;
	}

	public Direction Opposite { get { return Direction.OppositeOf (this); } }

	public static IEnumerable<Direction> All { get {
			yield return Left;
			yield return Right;
			yield return Down;
			yield return Up;
			yield return Forward;
			yield return Backward;
		}
	}
}
