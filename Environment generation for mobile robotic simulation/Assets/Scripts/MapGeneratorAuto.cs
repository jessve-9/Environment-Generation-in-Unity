using UnityEngine;
using System.Collections;
using System;

public class MapGeneratorAuto : MonoBehaviour { 
	public void GenerateMap(int mapWidth, int mapLength, float noiseScaleGround, int octavesGround, float persistanceGround, float lacunarityGround, int seedGround, Vector2 offsetGround,
                            float heightMultiplierGround, float heightOffsetGround, float noiseScaleHills, int octavesHills, float persistanceHills, float lacunarityHills, int seedHills,
                            Vector2 offsetHills, float heightMultiplierHills, float heightOffsetHills, float heightOffsetElevated, float tiltZ, float tiltX, bool useFalloff, float[,] falloffMap) {

		if(mapLength <= 0){
			mapLength = 1;
		}
		else if(mapWidth <= 0){
			mapWidth = 1;
		}

		int scaledMapLength = Convert.ToInt32(mapLength * 1);		//Fixes scale from unity to pybullet. Now it is 1:1 meters
		int scaledMapWidth = Convert.ToInt32(mapWidth * 1);

		float[,] noiseGround = Noise.GenerateNoiseMap (scaledMapWidth, scaledMapLength, seedGround, noiseScaleGround, octavesGround, persistanceGround, lacunarityGround, offsetGround, heightMultiplierGround, heightOffsetGround);
        float[,] noiseHills = Noise.GenerateNoiseMap (scaledMapWidth, scaledMapLength, seedHills, noiseScaleHills, octavesHills, persistanceHills, lacunarityHills, offsetHills, heightMultiplierHills, heightOffsetHills);

		float[,] elevatedGround = PlaneFunctions.PlaneElevated(noiseGround, heightOffsetElevated);
		if (useFalloff){
			noiseHills = PlaneFunctions.AddFalloffMap(noiseHills, falloffMap);
		}
		noiseHills = PlaneFunctions.PlaneTexture(noiseHills, noiseGround);
        float[,] combinedMap = PlaneFunctions.CombineMaxValues(noiseGround, noiseHills);
        combinedMap = PlaneFunctions.CombineMinValues(combinedMap, elevatedGround);
        float maxHeight = float.MinValue;
		combinedMap = PlaneFunctions.CreateTiltZ(combinedMap, tiltZ);
		combinedMap = PlaneFunctions.CreateTiltX(combinedMap, tiltX);

        //Get highets value in combinedMap
		for (int z = 0; z < scaledMapLength; z++) {
			for (int x = 0; x < scaledMapWidth; x++) {
				maxHeight = Mathf.Max(maxHeight, combinedMap[x,z]); 
			}
		}

        Color darkBrown = new Color(0.34f,0.21f,0.08f,1f);
        Color grey = new Color(0.24f,0.24f,0.24f,1f);

        Color[] colourMap = new Color[scaledMapWidth * scaledMapLength];
		for (int z = 0; z < scaledMapLength; z++) {
			for (int x = 0; x < scaledMapWidth; x++) {
				colourMap[z*scaledMapWidth+x] = Color.Lerp(darkBrown, grey, (combinedMap[x,z] - heightOffsetGround)/(maxHeight-heightOffsetGround));
			}
		}

		MapDisplay display = FindObjectOfType<MapDisplay> ();
		display.DrawMesh (CreateMesh.CreateShape (combinedMap), TextureGenerator.TextureFromColourMap (colourMap, scaledMapWidth, scaledMapLength), true);

	}

	// void OnValidate() {
	// 	if (mapWidth < 1) {
	// 		mapWidth = 1;
	// 	}
	// 	if (mapLength < 1) {
	// 		mapLength = 1;
	// 	}
	// 	if (lacunarityGround < 1) {
	// 		lacunarityGround = 1;
	// 	}
	// 	if (octavesGround < 0) {
	// 		octavesGround = 0;
	// 	}
    //     if (lacunarityHills < 1) {
	// 		lacunarityHills = 1;
	// 	}
	// 	if (octavesHills < 0) {
	// 		octavesHills = 0;
	// 	}

	// 	falloffMap = FalloffGenerator.GenerateFalloffMap(mapWidth, mapLength);
	// }

	public void RandomizeOnMapType(float heightOffsetElevated, float tiltZ, float tiltX, bool useFalloff) {
		//if (mapType == MapType.ShortCycle){
        if(true){
			int mapWidth = 250;
			int mapLength = 250;

			System.Random rnd = new System.Random();
			//Ground
			float noiseScaleGround = 0.06f;
			int octavesGround = 5;
			float persistanceGround = 0.77f;
			float lacunarityGround = 4f;
			int seedGround = rnd.Next(0, 99999);
			float heightMultiplierGround = 0.8f;
			float heightOffsetGround = 0f;

			//Elevated ground
			//float heightOffsetElevated = 13f;

			//Hills
			float noiseScaleHills = 100f;
			int octavesHills = 4;
			float persistanceHills = 0.11f;
			float lacunarityHills = 4.45f;
			int seedHills = rnd.Next(0, 99999);
			float heightMultiplierHills = 50f;
			float heightOffsetHills = -23f;

            //Random stuff
            Vector2 offsetGround = new Vector2(0,0);
            Vector2 offsetHills = new Vector2(0,0);
            //float tiltZ = 0f;
	        //float tiltX = 0f;
	        //bool useFalloff = true;
            float[,] falloffMap = FalloffGenerator.GenerateFalloffMap(mapWidth, mapLength);
            GenerateMap(mapWidth, mapLength, noiseScaleGround, octavesGround, persistanceGround, lacunarityGround, seedGround, offsetGround,
                            heightMultiplierGround, heightOffsetGround, noiseScaleHills, octavesHills, persistanceHills, lacunarityHills, seedHills,
                            offsetHills, heightMultiplierHills, heightOffsetHills, heightOffsetElevated, tiltZ, tiltX, useFalloff, falloffMap);
		}
	}
}

/*
[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}
*/