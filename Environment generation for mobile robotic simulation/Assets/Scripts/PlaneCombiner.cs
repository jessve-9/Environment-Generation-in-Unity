using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCombiner
{
    public static float[,] CombineMaxValues(float[,] plane1, float[,] plane2){
        int width = plane1.GetLength (0);
        int height = plane1.GetLength (1);
        float[,] combine = new float[width, height]; //Alocate data
        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                combine[x,y] = Mathf.Max(plane1[x,y], plane2[x,y]);
            }
        }
        return combine;
    }
}
