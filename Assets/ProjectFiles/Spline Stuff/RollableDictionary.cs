using System.Linq;
using AYellowpaper.SerializedCollections;
using System;
using Meta.XR.ImmersiveDebugger.UserInterface;

[Serializable]
public class RollableDictionary<T>
{
	
	public SerializedDictionary<T, int> Options = new SerializedDictionary<T, int>();
	public T defaultItem = default(T);

	public int GetNumberOfOptions()
	{
		if (Options == null)
		{
			Options = new SerializedDictionary<T, int>();

		}
		return Options.Count;

	}

	public T GetWeightedOption()
	{
		if (Options.Count == 0)
		{
			return defaultItem;
		}

		if (Options.Count == 1)
		{
			foreach (var pair in Options.ToList())
			{
				return pair.Key;
			}
		}

		int totalWeight = 0;
		foreach (var pair in Options)
		{
			totalWeight += pair.Value;
		}
		
		if(totalWeight <= 0){
			return defaultItem;
		}

		int randomWeight = UnityEngine.Random.Range(0, totalWeight);
		int cumulative = 0;
		foreach (var pair in Options)
		{
			cumulative += pair.Value;
			if (randomWeight < cumulative)
			{
				return pair.Key;
			}
		}

		return defaultItem;
	}

}