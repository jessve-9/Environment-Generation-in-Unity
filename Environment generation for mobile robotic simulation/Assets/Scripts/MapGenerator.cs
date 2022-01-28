using UnityEngine;
using System.Collections;
using System;

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh};
	public DrawMode drawMode;

	public int mapWidth;
	public int mapHeight;

	public float noiseScaleGround;

	public int octavesGround;
	[Range(0,1)]
	public float persistanceGround;
	public float lacunarityGround;

	public int seedGround;
	public Vector2 offsetGround;

    public float heightMultiplierGround;

    public float heightOffsetGround;


    public float noiseScaleHills;

	public int octavesHills;
	[Range(0,1)]
	public float persistanceHills;
	public float lacunarityHills;

	public int seedHills;
	public Vector2 offsetHills;

    public float heightMultiplierHills;

    public float heightOffsetHills;

	public bool autoUpdate;

    public float heightOffsetElevated;

	public TerrainType[] regions;

	public void GenerateMap() {

		if(mapHeight <= 0){
			mapHeight = 1;
		}
		else if(mapWidth <= 0){
			mapWidth = 1;
		}

		int scaledMapHeight = Convert.ToInt32(mapHeight * 0.6);		//Fixes scale from unity to pybullet. Now it is 1:1 meters
		int scaledMapWidth = Convert.ToInt32(mapWidth * 0.6);

		float[,] noiseGround = Noise.GenerateNoiseMap (scaledMapWidth, scaledMapHeight, seedGround, noiseScaleGround, octavesGround, persistanceGround, lacunarityGround, offsetGround, heightMultiplierGround, heightOffsetGround);
        float[,] elevatedGround = Noise.GenerateNoiseMap (scaledMapWidth, scaledMapHeight, seedGround, noiseScaleGround, octavesGround, persistanceGround, lacunarityGround, offsetGround, heightMultiplierGround, heightOffsetGround+heightOffsetElevated);
        float[,] noiseHills = Noise.GenerateNoiseMap (scaledMapWidth, scaledMapHeight, seedHills, noiseScaleHills, octavesHills, persistanceHills, lacunarityHills, offsetHills, heightMultiplierHills, heightOffsetHills);

        float[,] combinedMap = PlaneCombiner.CombineMaxValues(noiseGround, noiseHills);
        combinedMap = PlaneCombiner.CombineMinValues(combinedMap, elevatedGround);
        float maxHeight = float.MinValue;

        //Get highets value in combinedMap
        for (int y = 0; y < scaledMapHeight; y++) {
			for (int x = 0; x < scaledMapWidth; x++) {
				maxHeight = Mathf.Max(maxHeight, combinedMap[x,y]);
			}
		}

		// Color[] colourMap = new Color[mapWidth * mapHeight];
		// for (int y = 0; y < mapHeight; y++) {
		// 	for (int x = 0; x < mapWidth; x++) {
		// 		float currentHeight = combinedMap [x, y];
		// 		for (int i = 0; i < regions.Length; i++) {
		// 			if (currentHeight <= regions [i].height * maxHeight) {
		// 				colourMap [y * mapWidth + x] = regions [i].colour;
		// 				break;
		// 			}
		// 		}
		// 	}
		// }

        Color darkBrown = new Color(0.34f,0.21f,0.08f,1f);
        Color lightGrey = new Color(0.62f,0.58f,0.56f,1f);

        Color[] colourMap = new Color[scaledMapWidth * scaledMapHeight];
		for (int y = 0; y < scaledMapHeight; y++) {
			for (int x = 0; x < scaledMapWidth; x++) {
				colourMap[y*scaledMapWidth+x] = Color.Lerp(darkBrown, lightGrey, (combinedMap[x,y] - heightOffsetGround)/(maxHeight-heightOffsetGround));
			}
		}

		MapDisplay display = FindObjectOfType<MapDisplay> ();
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (combinedMap));
		} else if (drawMode == DrawMode.ColourMap) {
			display.DrawTexture (TextureGenerator.TextureFromColourMap (colourMap, scaledMapWidth, scaledMapHeight));
		} else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh (CreateMesh.CreateShape (combinedMap), TextureGenerator.TextureFromColourMap (colourMap, scaledMapWidth, scaledMapHeight));

        }
	}

	void OnValidate() {
		if (mapWidth < 1) {
			mapWidth = 1;
		}
		if (mapHeight < 1) {
			mapHeight = 1;
		}
		if (lacunarityGround < 1) {
			lacunarityGround = 1;
		}
		if (octavesGround < 0) {
			octavesGround = 0;
		}
        if (lacunarityHills < 1) {
			lacunarityHills = 1;
		}
		if (octavesHills < 0) {
			octavesHills = 0;
		}
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}