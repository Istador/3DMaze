using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

	public float wallWidth = 2.0f;
	public float roomWidth = 8.0f;

	public int Width { get; private set; }
	public int Height { get; private set; }
	public int Depth { get; private set; }

	private Text SeedText;
	public string Seed = "";
	private System.Random rnd;

	public Room[,,] Rooms { get; private set; }

	private double resetCooldown = 0.0;

	protected void Start() {
		SeedText = GameObject.Find("Canvas").transform.FindChild("Seed").GetComponent<Text>();

		float size = 1.0f / (roomWidth + wallWidth);
		Width = Mathf.RoundToInt(transform.localScale.x * size);
		Height = Mathf.RoundToInt(transform.localScale.y * size);
		Depth = Mathf.RoundToInt(transform.localScale.z * size);

		Reset(Seed);
	}

	protected void Update() {
		if (resetCooldown <= 0.0) {
			if (Input.GetButton ("Fire2")) {
				resetCooldown = 0.2;
				Reset ("");
			}
		} else {
			resetCooldown = System.Math.Max(0.0, resetCooldown - Time.deltaTime);
		}
	}

	private void Reset(string seed) {
		// delete children
		while (transform.childCount > 0) {
			Transform child = transform.GetChild(0);
			child.SetParent(null);
			Destroy(child.gameObject);
		}

		if (seed == "") {
			rnd = new System.Random ();
			Seed = rnd.Next ().ToString ("X4");
		} else {
			Seed = seed;
		}
		SeedText.text = "Seed: " + Seed;

		Generate();
	}

	private void Generate() {
		// Reset randomness with seed to the initial value
		rnd = new System.Random(Seed.GetHashCode());

		// create Rooms
		Rooms = new Room[Width, Height, Depth];
		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				for (int z = 0; z < Depth; z++) {
					Rooms[x, y, z] = new Room (Direction.Vector (x, y, z));
				}
			}
		}

		TMPrim.I.Transform (rnd, this);

		GenerateWallObjects ();
	}

	private void GenerateWallObjects() {
		
		Material mat = Resources.Load("Materials/Wall", typeof(Material)) as Material;

		// precalc scale
		float wall_room_wall = roomWidth + wallWidth * 2f;
		float room_and_wall = roomWidth + wallWidth;
		Vector3 scale = new Vector3 (wall_room_wall, wall_room_wall, wallWidth);
		Vector3 center = transform.position - new Vector3 (Width, Height, Depth) * 0.5f * room_and_wall;

		// get all walls
		HashSet<Wall> walls = new HashSet<Wall> ();
		foreach (Room r in Rooms) {
			foreach (Wall w in r.Walls) { if (w != null) { walls.Add (w); } }
		}

		// for all walls
		foreach (Wall w in walls) {
			GameObject go = GameObject.CreatePrimitive (PrimitiveType.Cube);
			go.name = w.Pos + " Wall";
			go.transform.localRotation = w.Rot;
			go.transform.localScale = scale;
			go.transform.position = w.Pos * room_and_wall + center;
			go.transform.parent = transform;
			go.GetComponent<Renderer>().material = mat;
			//go.GetComponent<Renderer>().enabled = false;
		}

		// combine meshes
		/*
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
		meshFilters = Array.FindAll(meshFilters, (mf) => { return mf != meshFilter; });

		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		for (int i = 0; i < meshFilters.Length; i++) {
			combine [i].mesh = meshFilters [i].sharedMesh;
			combine [i].transform = meshFilters [i].transform.localToWorldMatrix;
			//meshFilters [i].gameObject.SetActive(false);
		}

		Mesh mesh = new Mesh ();
		mesh.CombineMeshes(combine, true, true);
		meshFilter.mesh.triangles = new int[0];
		meshFilter.mesh.vertices = new Vector3[0];
		meshFilter.mesh = mesh;
		*/

		// disable dummy box
		Destroy(transform.GetComponent<MeshRenderer>());
		Destroy(transform.GetComponent<MeshFilter>());

		gameObject.SetActive (true);
	}

}
