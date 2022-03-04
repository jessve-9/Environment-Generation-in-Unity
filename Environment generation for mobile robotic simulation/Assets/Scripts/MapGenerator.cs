using UnityEngine;
using System.Collections;
using System;

public class MapGenerator : MonoBehaviour { 
	public enum MapType {ShortCycle, BigPictureView};
	public MapType mapType;

	int mapWidth = 250;
	int mapLength = 250;

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

	//public TerrainType[] regions;

	[Range(0,45)]
	public float tiltZ;

	[Range(0,45)]
	public float tiltX;

	public bool useFalloff;
	float[,] falloffMap;

	private float[,] dirtMeshMap;
	private float[,] gravelMeshMap;
	[Range(0,1)]
	public float groundVariation = 0.5f;

	/*
	private float[,] dirtIntervals;		//Used in interval calculation
	private float[,] gravelIntervals;
	*/

	public void GenerateMap() {

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
		Color grey2 = new Color(0.20f,0.20f,0.20f,1f);			//Grus färg är det tänkt

        Color[] colourMap = new Color[scaledMapWidth * scaledMapLength];
		//Color[] colourMapGravel = new Color[scaledMapWidth * scaledMapLength];
		for (int z = 0; z < scaledMapLength; z++) {
			for (int x = 0; x < scaledMapWidth; x++) {
				colourMap[z*scaledMapWidth+x] = Color.Lerp(darkBrown, grey, (combinedMap[x,z] - heightOffsetGround)/(maxHeight-heightOffsetGround));
				//colourMapGravel[z*scaledMapWidth+x] = Color.Lerp(grey2, grey, (combinedMap[x,z] - heightOffsetGround)/(maxHeight-heightOffsetGround));
			}
		}

		MapDisplay display = FindObjectOfType<MapDisplay> ();

		dirtMeshMap = Friction.frictionMap(mapWidth, mapLength, combinedMap, true, groundVariation);
		gravelMeshMap = Friction.frictionMap(mapWidth, mapLength, combinedMap, false, groundVariation);
		
		/*
		dirtIntervals = intervalCalc(dirtMeshMap);			//Calculate mesh x cordinate index interval
		gravelIntervals = intervalCalc(gravelMeshMap);
		*/

		//display.DrawMesh (CreateMesh.CreateShape (combinedMap), TextureGenerator.TextureFromColourMap (colourMap, scaledMapWidth, scaledMapLength), true);		//Draws the entire map wtith one material
		display.DrawMesh (CreateMesh.CreateShape (dirtMeshMap /*, dirtIntervals //future arg for better mesh creation?*/), TextureGenerator.TextureFromColourMap (colourMap, scaledMapWidth, scaledMapLength), true);
		display.DrawMesh (CreateMesh.CreateShape (gravelMeshMap /*, gravelIntervals //future arg for better mesh creation?*/), TextureGenerator.TextureFromColourMap (colourMap, scaledMapWidth, scaledMapLength), false);
	}

	void OnValidate() {
		if (mapWidth < 1) {
			mapWidth = 1;
		}
		if (mapLength < 1) {
			mapLength = 1;
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

		falloffMap = FalloffGenerator.GenerateFalloffMap(mapWidth, mapLength);
	}

	public void RandomizeOnMapType() {
		if (mapType == MapType.ShortCycle){
			mapWidth = 250;
			mapLength = 250;

			System.Random rnd = new System.Random();
			//Ground
			noiseScaleGround = 0.06f;
			octavesGround = 5;
			persistanceGround = 0.77f;
			lacunarityGround = 4f;
			seedGround = rnd.Next(0, 99999);
			heightMultiplierGround = 0.8f;
			heightOffsetGround = 0f;

			//Elevated ground
			heightOffsetElevated = 13f;

			//Hills
			noiseScaleHills = 100f;
			octavesHills = 4;
			persistanceHills = 0.11f;
			lacunarityHills = 4.45f;
			seedHills = rnd.Next(0, 99999);
			heightMultiplierHills = 50f;
			heightOffsetHills = -23f;
		}
	}

	private static float[,] intervalCalc(float[,] mapMesh){
		int[] xzSavePoint = {0, 0};
		while(mapMesh[xzSavePoint[0],xzSavePoint[1]]<0){
			while(mapMesh[xzSavePoint[0],xzSavePoint[1]]<0){
				xzSavePoint[0] += 1;
			}
		xzSavePoint[1] += 1;;
		}
		return mapMesh;
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