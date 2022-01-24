using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CreateMesh : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    public int xSize = 50;
    public int zSize = 50;

    Color[] color;
    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
    }

    void CreateShape(){
        vertices = new Vector3[(xSize+1)*(zSize+1)];
        for(int i = 0, z = 0; z<=zSize; z++){
            for(int x = 0; x <= xSize; x++){
                

                    //--------------------- Insert Perlin noise y value ---------------------
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }   
        }


        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tri = 0;

        for(int z = 0; z< zSize; z++){
            for(int x = 0; x < xSize; x++){
                triangles[tri + 0] = vert + 0;
                triangles[tri + 1] = vert + xSize + 1;
                triangles[tri + 2] = vert + 1;
                triangles[tri + 3] = vert + 1;
                triangles[tri + 4] = vert + xSize + 1;
                triangles[tri + 5] = vert + xSize + 2;

                vert++;
                tri+=6;
            }
            vert++;
        }
    }

    void UpdateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}