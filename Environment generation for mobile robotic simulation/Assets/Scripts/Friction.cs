using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction  {

    //friction specific noise map generation values
    private int fricSeed = 1;
    private int fricOctaves = 1;
    private float fricPersistance = 1;
    private float fricLacunarity = 1;
    private float fricHeightMultiplier = 1;
    private float fricHeightOffset = 1;

    public float[,] frictionMap(int mapWidth, int mapLength, float scale, Vector2 offset){
        float[,] fricMap = new float[mapWidth, mapLength];
        fricMap = Noise.GenerateNoiseMap(mapWidth, mapLength, fricSeed,  scale, fricOctaves, fricPersistance, fricLacunarity, offset, fricHeightMultiplier, fricHeightOffset);
        return fricMap;
    }

}
