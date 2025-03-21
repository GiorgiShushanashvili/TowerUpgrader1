using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GoogleSheetData", menuName = "ScriptableObjects/GoogleSheetData", order = 1)]
public class GoogleDataSheetForStats:ScriptableObject
{
    public List<SheetValues> sheetValues=new();
    public List<SheetSilverCosts> silverCosts=new();
    public List<SheetGoldCosts> goldCosts=new();
}

[System.Serializable]
public class SheetGoldCosts
{
    public List<float> goldCosts=new();
}

[System.Serializable]
public class SheetSilverCosts
{
    public List<float> silverCosts=new();
}

[System.Serializable]
public class SheetValues
{
    public List<float> values=new();
}  
