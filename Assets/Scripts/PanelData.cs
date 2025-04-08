using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PanelData", menuName = "ScriptableObjects/PanelData", order = 2)]
public class PanelData:ScriptableObject
{
    [SerializeField] private static int amount;
    public List<PanelDataForOne> panels = new(amount);
}

[System.Serializable]
public class PanelDataForOne
{
    public float actualValue;
    public float nextValue;
    public float silverCost;
    public float goldCost;
    public int currentLevel;
    public int costLevel;
    public int permanentLevel;
    [SerializeField] [CanBeNull] public EvolveData evolveData;
}

[System.Serializable]
public class EvolveData
{
    public bool _isEvolved=false;
    
    public List<GameObject> _evolveObjects;
    public int _evolveLevel;
    public List<float> _evolvePrices;
    public List<float> _evolveDuration;
    public int evolveCounter;
}