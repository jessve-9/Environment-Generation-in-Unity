using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh};
	public DrawMode drawMode;

	public int mapWidth;
	public int mapLength;

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

	[Range(0,Mathf.PI/4)]
	public float tiltZ;

	[Range(0,Mathf.PI/4)]
	public float tiltX;

	public void GenerateMap() {
		float[,] noiseGround = Noise.GenerateNoiseMap (mapWidth, mapLength, seedGround, noiseScaleGround, octavesGround, persistanceGround, lacunarityGround, offsetGround, heightMultiplierGround, heightOffsetGround);
        float[,] elevatedGround = Noise.GenerateNoiseMap (mapWidth, mapLength, seedGround, noiseScaleGround, octavesGround, persistanceGround, lacunarityGround, offsetGround, heightMultiplierGround, heightOffsetGround+heightOffsetElevated);
        float[,] noiseHills = Noise.GenerateNoiseMap (mapWidth, mapLength, seedHills, noiseScaleHills, octavesHills, persistanceHills, lacunarityHills, offsetHills, heightMultiplierHills, heightOffsetHills);

		noiseHills = PlaneCombiner.PlaneTexture(noiseHills, noiseGround);
        float[,] combinedMap = PlaneCombiner.CombineMaxValues(noiseGround, noiseHills);
        combinedMap = PlaneCombiner.CombineMinValues(combinedMap, elevatedGround);
        float maxHeight = float.MinValue;
		combinedMap = PlaneCombiner.CreateTiltZ(combinedMap, tiltZ);
		combinedMap = PlaneCombiner.CreateTiltX(combinedMap, tiltX);

        //Get highets value in combinedMap
        for (int z = 0; z < mapLength; z++) {
			for (int x = 0; x < mapWidth; x++) {
				maxHeight = Mathf.Max(maxHeight, combinedMap[x,z]);
			}
		}

        Color darkBrown = new Color(0.34f,0.21f,0.08f,1f);
        Color grey = new Color(0.24f,0.24f,0.24f,1f);

        Color[] colourMap = new Color[mapWidth * mapLength];
		for (int z = 0; z < mapLength; z++) {
			for (int x = 0; x < mapWidth; x++) {
				colourMap[z*mapWidth+x] = Color.Lerp(darkBrown, grey, (combinedMap[x,z] - heightOffsetGround)/(maxHeight-heightOffsetGround));
			}
		}

		MapDisplay display = FindObjectOfType<MapDisplay> ();
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (combinedMap));
		} else if (drawMode == DrawMode.ColourMap) {
			display.DrawTexture (TextureGenerator.TextureFromColourMap (colourMap, mapWidth, mapLength));
		} else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh (CreateMesh.CreateShape (combinedMap), TextureGenerator.TextureFromColourMap (colourMap, mapWidth, mapLength));

        }
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
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}