using UnityEngine;
using System.Collections;

public static class FalloffGenerator {

	public static float[,] GenerateFalloffMap(int mapWidth, int mapLength) {
		float[,] map = new float[mapWidth,mapLength];

		for (int i = 0; i < mapWidth; i++) {
			for (int j = 0; j < mapLength; j++) {
				float x = i / (float)mapWidth * 2 - 1;
				float z = j / (float)mapLength * 2 - 1;

				float value = Mathf.Max (Mathf.Abs (x), Mathf.Abs (z));
				map [i, j] = Evaluate(value);
			}
		}

		return map;
	}

	static float Evaluate(float value) {
		float a = 4.7f;
		float b = 1.6f;

		return 1f - Mathf.Pow (value, a) / (Mathf.Pow (value, a) + Mathf.Pow (b - b * value, a));
	}
}