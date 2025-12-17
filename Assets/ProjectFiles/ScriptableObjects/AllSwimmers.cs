using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AllSwimmers", menuName = "Scriptable Objects/AllSwimmers")]
public class AllSwimmers : ScriptableObject
{
    public List<GameObject> AllPeople;
}
