using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneFunctions
{
    public static float[,] CombineMaxValues(float[,] plane1, float[,] plane2){
        int width = plane1.GetLength (0);
        int length = plane1.GetLength (1);
        float[,] combine = new float[width, length];
        for(int z = 0; z < length; z++){
            for(int x = 0; x < width; x++){
                combine[x,z] = Mathf.Max(plane1[x,z], plane2[x,z]);
            }
        }
        return combine;
    }

    public static float[,] CombineMinValues(float[,] plane1, float[,] plane2){
        int width = plane1.GetLength (0);
        int length = plane1.GetLength (1);
        float[,] combine = new float[width, length];
        for(int z = 0; z < length; z++){
            for(int x = 0; x < width; x++){
                combine[x,z] = Mathf.Min(plane1[x,z], plane2[x,z]);
            }
        }
        return combine;
    }
    public static float[,] CreateTiltZ(float[,] plane1, float tilt){
        int width = plane1.GetLength (0);
        int length = plane1.GetLength (1);
        float[,] combine = new float[width, length];
        float tiltHeight = Mathf.Tan(tilt*Mathf.PI/180f);
        for(int z = 0; z < length; z++){
            for(int x = 0; x < width; x++){
                combine[x,z] = plane1[x,z]+z*tiltHeight;
            }
        }
        return combine;
    }

    public static float[,] CreateTiltX(float[,] plane1, float tilt){
        int width = plane1.GetLength (0);
        int length = plane1.GetLength (1);
        float[,] combine = new float[width, length];
        float tiltHeight = Mathf.Tan(tilt*Mathf.PI/180f);
        for(int z = 0; z < length; z++){
            for(int x = 0; x < width; x++){
                combine[x,z] = plane1[x,z]+x*tiltHeight;
            }
        }
        return combine;
    }

    public static float[,] PlaneTexture(float[,] plane, float[,] texture){
        int width = plane.GetLength (0);
        int length = plane.GetLength (1);
        float[,] combine = new float[width, length];

        float maxHeightTexure = float.MinValue;
        for (int y = 0; y < length; y++) {
			for (int x = 0; x < width; x++) {
				maxHeightTexure = Mathf.Max(maxHeightTexure, texture[x,y]);
			}
		}
        float textureHeightOffset = maxHeightTexure/2;

        for(int z = 0; z < length; z++){
            for(int x = 0; x < width; x++){
                combine[x,z] = plane[x,z] + texture[x,z] - textureHeightOffset;
            }
        }
        return combine;
    }

    public static float[,] PlaneElevated(float[,] plane, float elevationHeight){
        int width = plane.GetLength (0);
        int length = plane.GetLength (1);
        float[,] combine = new float[width, length];
        for(int z = 0; z < length; z++){
            for(int x = 0; x < width; x++){
                combine[x,z] = plane[x,z]+elevationHeight;
            }
        }
        return combine;
    }

    public static float[,] AddFalloffMap(float[,] plane, float[,] fallofMap){
        int width = plane.GetLength (0);
        int length = plane.GetLength (1);
        float[,] combine = new float[width, length];
        float maxHeight = float.MinValue;
        for(int z = 0; z < length; z++){
            for(int x = 0; x < width; x++){
                maxHeight = Mathf.Max(maxHeight, plane[x,z]); 
            }
        }

        for(int z = 0; z < length; z++){
            for(int x = 0; x < width; x++){
                combine[x,z] = plane[x,z] - fallofMap[x,z]*maxHeight;
            }
        }
        return combine;
    }


}
