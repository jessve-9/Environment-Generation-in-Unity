using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapLength, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, float heightMultiplier, float heightOffset){
        float[,] noiseMap = new float[mapWidth, mapLength];



        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++){
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetZ = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets [i] = new Vector2 (offsetX, offsetZ);
        }


        if (scale <= 0){
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfLength = mapLength / 2f;


        for (int z = 0; z < mapLength; z++){
            for (int x = 0; x < mapWidth; x++){

                float amplitude = 10;
                float frequency = 1;
                float noiseHeight = 0;

                for(int i = 0; i < octaves; i++){
                    float sampleX = (x-halfWidth) / scale*frequency + octaveOffsets[i].x;
                    float sampleZ = (z-halfLength) / scale*frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise (sampleX, sampleZ) *2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x,z] = noiseHeight;
            }
        }

        for (int z = 0; z < mapLength; z++){
            for (int x = 0; x < mapWidth; x++){
                noiseMap[x, z] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap[x,z]);
                noiseMap[x,z] *= heightMultiplier;
                noiseMap[x,z] += heightOffset;
            }
        }
        
        return noiseMap;
    }
}
