using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public static class CreateMesh
{
    public static MeshData CreateShape(float[,] heightMap){
        int width = heightMap.GetLength (0);
		int height = heightMap.GetLength (1);

        float topLeftX = (width - 1) / -2f;
		float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData (width, height);
        for(int i = 0, z = 0; z<height; z++){
            for(int x = 0; x < width; x++){
                meshData.vertices[i] = new Vector3(topLeftX+x, heightMap[x,z], topLeftZ-z);
                meshData.uvs [i] = new Vector2 (x / (float)width, z / (float)height);

                if (x < width - 1 && z < height - 1) {
                    meshData.AddTriangle (i, i + width + 1, i + width);
					meshData.AddTriangle (i + width + 1, i, i + 1);
                }
                
                i++;
            }   

        }

        return meshData;
    }

}
public class MeshData {
	public Vector3[] vertices;
	public int[] triangles;
	public Vector2[] uvs;

	int triangleIndex;

	public MeshData(int meshWidth, int meshHeight) {
		vertices = new Vector3[meshWidth * meshHeight];
		uvs = new Vector2[meshWidth * meshHeight];
		triangles = new int[(meshWidth-1)*(meshHeight-1)*6];
	}

	public void AddTriangle(int a, int b, int c) {
		triangles [triangleIndex] = a;
		triangles [triangleIndex + 1] = b;
		triangles [triangleIndex + 2] = c;
		triangleIndex += 3;
	}

	public Mesh CreateMesh() {
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals ();
		return mesh;
	}

}