using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction  {

    public static float[,] frictionMap(int mapWidth, int mapLength, float[,] combinedMap, bool map, float groundVariation){
        int fricSeed = 2;           //Values for the friction perlin noise heightmap
        int fricOctaves = 3;
        float fricPersistance = 1;
        float fricLacunarity = 1;
        float fricHeightMultiplier = 1;
        float fricHeightOffset = 0;
        float fricScale = 10;

        Vector2 offset = new Vector2(0,0);      //Should always be zero as offset

        float[,] dirtMap = new float[combinedMap.GetLength(0),combinedMap.GetLength(1)];
        float[,] gravelMap = new float[combinedMap.GetLength(0),combinedMap.GetLength(1)];
        float[,] fricMap = Noise.GenerateNoiseMap(mapWidth, mapLength, fricSeed,  fricScale, fricOctaves, fricPersistance, fricLacunarity, offset, fricHeightMultiplier, fricHeightOffset);

        if(map) {
            float[,] dMap = DirtMap(dirtMap, fricMap, combinedMap, groundVariation);
            return dMap;
        }else {
            float[,] gMap = GravelMap(gravelMap, fricMap, combinedMap, groundVariation);
            return gMap;
        }
    }

    public static float[,] DirtMap(float[,] dirtMap, float[,] fricMap, float[,] combinedMap, float groundVariation) {          //Eff? Questionable... but it works :)
        for (int z = 0; z < fricMap.GetLength(0); z++) {
            for (int x = 0; x < fricMap.GetLength(1); x++) {
                if(fricMap[x,z]>groundVariation) {
                    dirtMap[x,z]=combinedMap[x,z];
                }else {
                    dirtMap[x,z]=combinedMap[x,z]-0.1f;
                }
            }
        }
        return dirtMap;
    }

    public static float[,] GravelMap(float[,] gravelMap, float[,] fricMap, float[,] combinedMap, float groundVariation) {
        for (int z = 0; z < fricMap.GetLength(0); z++) {
            for (int x = 0; x < fricMap.GetLength(1); x++) {
                if(fricMap[x,z]>groundVariation) {
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


