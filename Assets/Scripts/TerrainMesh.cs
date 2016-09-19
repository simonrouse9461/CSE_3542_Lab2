using UnityEngine;
using System.Collections;
using System;

public class TerrainMesh : MonoBehaviour {

	public float width;
	public float length;
	public int xDensity;
	public int zDensity;
	public float textureDensity;
	public float steepness;
	public float uniformity;
	public int detailLevel;

	MeshFilter mFilter;
	MeshCollider mCollider;

	float unitWidth
	{
		get { return width / xDensity; }
	}
	float unitLength
	{
		get { return length / zDensity; }
	}

	float[,] GenerateNoise(int mapWidth, int mapLength,float amplitude, float scale, float[,] background = null)
	{
		float[,] noiseMap = new float[mapWidth, mapLength];
		float xOffset = UnityEngine.Random.value;
		float yOffset = UnityEngine.Random.value;

		for (int y = 0; y < mapLength; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				float sampleX = x / scale + xOffset;
				float sampleY = y / scale + yOffset;

				float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
				noiseMap[x, y] = amplitude * (perlinValue - 0.5f);
				if (background != null)
				{
					noiseMap[x, y] += background[x, y];
				}
			}
		}
		return noiseMap;
	}

	Mesh GenerateTerrainMesh()
	{
		var mesh = new Mesh();

		// Generate noise map
		var noiseMap = new float[xDensity, zDensity];
		var amp = steepness;
		var scale = uniformity;
		for (int i = 0; i < detailLevel; i++, amp /= 2, scale /= 2)
		{
			noiseMap = GenerateNoise(xDensity, zDensity, amp, scale, noiseMap);
		}

		// Vertices and UVs
		var verticies = new Vector3[xDensity * zDensity];
		var uvs = new Vector2[xDensity * zDensity];

		// Creating hills and valleys
		for (var z = 0; z < zDensity; z++)
		{
			for (var x = 0; x < xDensity; x++)
			{
				float xPos = (x - xDensity / 2) * unitWidth;
				float zPos = (z - zDensity / 2) * unitLength;
				int index = z * xDensity + x;
				//float height = 100*(float)(Math.Exp(-(Math.Pow(xPos - width / 2, 2) / 800+Math.Pow(zPos - length / 2, 2)/800)));
				float height = noiseMap[x, z];
				verticies[index] = new Vector3(xPos, height, zPos);
				uvs[z * xDensity + x] = new Vector2(x * textureDensity / xDensity, z * textureDensity / zDensity);
			}
		}

		// Triangles
		var triangles = new int[2 * 3 * (xDensity - 1) * (zDensity - 1)];
		var triIndex = 0;

		for (var z = 0; z < zDensity - 1; z++)
		{
			for (var x = 0; x < xDensity - 1; x++)
			{
				int LL = z * xDensity + x;
				int LR = z * xDensity + x + 1;
				int UL = (z + 1) * xDensity + x;
				int UR = (z + 1) * xDensity + x + 1;

				triangles[triIndex + 0] = LL;
				triangles[triIndex + 1] = UL;
				triangles[triIndex + 2] = LR;

				triangles[triIndex + 3] = UL;
				triangles[triIndex + 4] = UR;
				triangles[triIndex + 5] = LR;

				triIndex += 6;
			}
		}

		mesh.vertices = verticies;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();

		return mesh;
	}

	void CreateTerrain()
	{
		var mesh = GenerateTerrainMesh();
		mFilter.mesh = mesh;
		mCollider.sharedMesh = mesh;
	}

	// Use this for initialization
	void Start()
	{
		mFilter = GetComponent<MeshFilter>();
		mCollider = GetComponent<MeshCollider>();
		CreateTerrain();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			CreateTerrain();
		}
	}
}
