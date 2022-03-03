using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction  {

    //friction specific noise map generation values
    /*private int fricSeed = 2;
    private int fricOctaves = 4;
    private float fricPersistance = 4;
    private float fricLacunarity = 4;
    private float fricHeightMultiplier = 1;
    private float fricHeightOffset = 0;
    private float fricScale = 10; 
    Vector2 offset = new Vector2(0,0);*/

    //Perlin noise arrays for the meshes
    //private float[,] dirtMap;
    //private float[,] gravelMap;


    public static float[,] frictionMap(int mapWidth, int mapLength, float[,] combinedMap, bool map){

        int fricSeed = 2;
        int fricOctaves = 3;
        float fricPersistance = 1;
        float fricLacunarity = 1;
        float fricHeightMultiplier = 1;
        float fricHeightOffset = 0;
        float fricScale = 10; 
        Vector2 offset = new Vector2(0,0);

        float[,] dirtMap = new float[combinedMap.GetLength(0),combinedMap.GetLength(1)];
        float[,] gravelMap = new float[combinedMap.GetLength(0),combinedMap.GetLength(1)];
        float[,] fricMap = Noise.GenerateNoiseMap(mapWidth, mapLength, fricSeed,  fricScale, fricOctaves, fricPersistance, fricLacunarity, offset, fricHeightMultiplier, fricHeightOffset);
        if(map) {
            float[,] dMap = DirtMap(dirtMap, fricMap, combinedMap);
            return dMap;
        }else {
            float[,] gMap = GravelMap(gravelMap, fricMap, combinedMap);
            return gMap;
        }
        /*for (int z = 0; z < fricMap.GetLength(0); z++) {
            for (int x = 0; x < fricMap.GetLength(1); x++) {
                if(fricMap[x,z]>0.5) {
                    dirtMap[x,z]=combinedMap[x,z];
                    gravelMap[x,z]=-1f;
                }else {
                    gravelMap[x,z]=combinedMap[x,z];
                    dirtMap[x,z]=-1f;
                }
                
                //Debug.Log("Gravel: " + gravelMap[x,z]);
                //Debug.Log("Dirt: " + dirtMap[x,z]);
            }
        }*/
        //Debug.Log("combMap Length : " + combinedMap.Length);
        //Debug.Log("fricMap Length : " + fricMap.Length);
    }

    public static float[,] DirtMap(float[,] dirtMap, float[,] fricMap, float[,] combinedMap) {
        for (int z = 0; z < fricMap.GetLength(0); z++) {
            for (int x = 0; x < fricMap.GetLength(1); x++) {
                if(fricMap[x,z]>0.5) {
                    dirtMap[x,z]=combinedMap[x,z];
                }else {
                    dirtMap[x,z]=combinedMap[x,z]-0.1f;
                }
            }
        }
        return dirtMap;
    }

    public static float[,] GravelMap(float[,] gravelMap, float[,] fricMap, float[,] combinedMap) {
        for (int z = 0; z < fricMap.GetLength(0); z++) {
            for (int x = 0; x < fricMap.GetLength(1); x++) {
                if(fricMap[x,z]>0.5) {
                    gravelMap[x,z]=combinedMap[x,z]-0.1f;
                }else {
                    gravelMap[x,z]=combinedMap[x,z];
                }
            }
        }
        return gravelMap;
    }
}

    

/*
float[,] combinedMap;
float[,] dirtMap;
float[,] gravelMap;
for (int z = 0; z < scaledMapLength; z++) {
    for (int x = 0; x < scaledMapWidth; x++) {
        if(fricMap[z,x]>0.5) {
            dirtMap.append(combinedMap[x,z]);
            gravelMap.append(MinValue);
        }else {
            gravelMap.append(combinedMap[x,z]);
            dirtMap.append(MinValue);
        }*/


