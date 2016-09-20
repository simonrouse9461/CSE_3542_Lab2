using UnityEngine;
using System.Collections;

public class ObjectMesh : MonoBehaviour {

	public float width;
	public float length;
	public float height;

	MeshFilter mFilter;
	MeshCollider mCollider;

	// Use this for initialization
	void Start () {
		
		mFilter = GetComponent<MeshFilter>();
		mCollider = GetComponent<MeshCollider>();

		var mesh = new Mesh();

		var points = new Vector3[]
		{
			new Vector3(-1 * width, -1 * height, -1 * length),
			new Vector3(1 * width, -1 * height, -1 * length),
			new Vector3(-1 * width, 1 * height, -1 * length),
			new Vector3(1 * width, 1 * height, -1 * length),
			new Vector3(-1 * width, -1 * height, 1 * length),
			new Vector3(1 * width, -1 * height, 1 * length),
			new Vector3(-1 * width, 1 * height, 1 * length),
			new Vector3(1 * width, 1 * height, 1 * length),
		};

		var verticies = new Vector3[]
		{
			points[0], points[2], points[3], points[1],
			points[0], points[4], points[6], points[2],
			points[0], points[1], points[5], points[4],
			points[7], points[6], points[4], points[5],
			points[7], points[5], points[1], points[3],
			points[7], points[3], points[2], points[6],
		};

		var triangles = new int[36];

		var triSeq = new [] { 0, 1, 2, 2, 3, 0 };

		for (var i = 0; i < 6; i++)
		{
			for (var j = 0; j < 6; j++)
			{
				triangles[i * 6 + j] = i * 4 + triSeq[j];
			}
		}

		var UVs = new Vector2[24];

		var uvSeq = new []
		{
			new Vector2(0, 0),
			new Vector2(0, 1),
			new Vector2(1, 1),
			new Vector2(1, 0),
		};

		for (var i = 0; i < 6; i++)
		{
			for (var j = 0; j < 4; j++)
			{
				UVs[i * 4 + j] = uvSeq[j];
			}
		}

		mesh.vertices = verticies;
		mesh.triangles = triangles;
		mesh.uv = UVs;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();

		mFilter.mesh = mesh;
		mCollider.sharedMesh = mesh;
		mCollider.convex = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
