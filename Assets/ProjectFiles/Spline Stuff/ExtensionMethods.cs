using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods{

    public static Vector3 Round(this Vector3 vector, float scale){
        return new Vector3(
            Round(vector.x, scale),
            Round(vector.y, scale),
            Round(vector.z, scale)
        );
    }

    public static float Round(float value, float scale = 1f){
        return Mathf.Round(value / scale) * scale;
    }

    public static float RoundF(this float value, float scale = 1f){
        return Mathf.Round(value / scale) * scale;
    }

    public static Vector2 RadialRandomRange(float R, float r = 0, float arc = 360, float offset = 0){
        Vector2 newPosition = Vector2.zero;
        if(r == 0  && arc == 360){
            newPosition = Random.insideUnitCircle * R;
        }else{
            float distance = Random.Range(r,R);
            float tetha = Random.Range(0,arc) + offset;
            newPosition.x = Mathf.Cos(tetha * Mathf.Deg2Rad) * distance;
            newPosition.y = Mathf.Sin(tetha * Mathf.Deg2Rad) * distance;
        }
        return newPosition;
    }

    public static float AtLeast(this float f, float min){
        return Mathf.Max(f,min);
    }

    public static int AtLeast(this int f, int min){
        return Mathf.Max(f, min);
    }

    public static float NoMoreThan(this float f, float min){
        return Mathf.Min(f, min);
    }

    public static Vector3 WithHeightOf(this Vector3 vector, Vector3 heightTarget){
        return new Vector3(vector.x, heightTarget.y, vector.z);
    }

}

public static class ColorUtilities {

	/// <summary>
	/// Returns a random color. 
	/// Set any parameter you dont care to -1
	/// </summary>
	/// <param name="red"></param>
	/// <param name="green"></param>
	/// <param name="blue"></param>
	/// <param name="alpha"></param>
	/// <returns></returns>
	public static Color GetRandomColor(float red = -1, float green = -1, float blue = -1, float alpha = -1){
		return new Color(
			red != -1 ? red : Random.Range(0.0f,1.0f),
			green != -1 ? green : Random.Range(0.0f,1.0f),
			blue != -1 ? blue : Random.Range(0.0f,1.0f),
			alpha != -1 ? alpha : Random.Range(0.0f,1.0f)
		);
	}

	public static Color MakeClear(this Color original){
		return new Color(original.r, original.g, original.b , 0.0f);
	}
}
