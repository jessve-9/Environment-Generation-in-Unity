using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    //Dirt
    public Renderer textureRenderDirt;
    public MeshFilter meshFilterDirt;
    public MeshRenderer meshRendererDirt;
    public MeshCollider meshColliderDirt;

    //Gravel
    public Renderer textureRenderGravel;
    public MeshFilter meshFilterGravel;
    public MeshRenderer meshRendererGravel;
    public MeshCollider meshColliderGravel;

    // public void DrawNoiseMap(float[,] noiseMap){
    //     int width = noiseMap.GetLength(0);
    //     int height = noiseMap.GetLength(1);

    //     Texture2D texture = new Texture2D(width, height);

    //     Color[] colourMap = new Color[width * height];

    //     for(int y = 0; y < height; y++){
    //         for(int x = 0; x < width; x++){
    //             colourMap[y*width+x] = Color.Lerp(Color.black, Color.white, noiseMap[x,y]);
    //         }
    //     }

    //     texture.SetPixels(colourMap);
    //     texture.Apply();

    //     textureRender.sharedMaterial.mainTexture = texture;
    //     textureRender.transform.localScale = new Vector3 (width, 1, height);
    // }

    public void DrawTexture(Texture2D texture){
        textureRenderDirt.sharedMaterial.mainTexture = texture;
        textureRenderDirt.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture, bool dirtOrGravel){
        if(dirtOrGravel==true){
            Debug.Log("dirt");
            meshFilterDirt.sharedMesh = meshData.CreateMesh();
            meshRendererDirt.sharedMaterial.mainTexture = texture;
            meshColliderDirt.sharedMesh = null;
            meshColliderDirt.sharedMesh = meshFilterDirt.sharedMesh;
        }else if(dirtOrGravel==false){
            Debug.Log("gravel");
            meshFilterGravel.sharedMesh = meshData.CreateMesh();
            meshRendererGravel.sharedMaterial.mainTexture = texture;
            meshColliderGravel.sharedMesh = null;
            meshColliderGravel.sharedMesh = meshFilterGravel.sharedMesh;
        }
    }    
}
