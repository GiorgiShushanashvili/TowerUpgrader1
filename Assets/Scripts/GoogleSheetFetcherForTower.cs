using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GoogleSheetFetcherForPlayer : MonoBehaviour
{
    public string publishedSheetURL; // Published Google Sheet URL (CSV format)
    public GoogleDataSheetForStats googleDatasheet;    // ScriptableObject to store data
    public Button resetStatsButton;  // Reference to the UI Button

    private void Start()
    {
        // Assign the button's onClick event to call the FetchData method
        if (resetStatsButton != null)
        {
            resetStatsButton.onClick.AddListener(OnResetStatsButtonClicked);
        }
        else
        {
            Debug.LogError("Reset Stats Button is not assigned!");
        }
    }

    // Method to handle button click
    private void OnResetStatsButtonClicked()
    {
        StartCoroutine(FetchDataFromGoogleSheet());
    }

    private IEnumerator FetchDataFromGoogleSheet()
    {
        Debug.Log("Fetching data from Google Sheet...");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(publishedSheetURL))
        {
            // Add a timestamp or random query parameter to prevent caching
            string urlWithCacheBust = $"{publishedSheetURL}?t={System.DateTime.UtcNow.Ticks}";
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.disposeDownloadHandlerOnDispose = true;
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching data: " + webRequest.error);
            }
            else
            {
                string csvData = webRequest.downloadHandler.text;
                Debug.Log("CSV Data fetched: " + csvData);
                ParseCSVData(csvData);
                SaveData();
            }
        }
    }

    private void ParseCSVData(string csvData)
    {
        Debug.Log("Parsing CSV Data...");
        // Clear the existing lists before adding new data
        googleDatasheet.sheetValues.Clear();
        googleDatasheet.silverCosts.Clear();
        googleDatasheet.goldCosts.Clear();
        Debug.Log("Cleared existing stats lists.");

        string[] lines = csvData.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length <= 1)
        {
            Debug.LogError("No data found in the Google Sheet.");
            return;
        }

        for (int i = 1; i < lines.Length; i++) // Skip the header
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue; // Skip empty lines

            string[] columns = line.Split(',');

            // Parse each column and add to the corresponding list
            if (columns.Length >= 2)
            {
                if (float.TryParse(columns[0].Trim(), out float tower))
                {
                    googleDatasheet.sheetValues.Add(new SheetValues());
                    googleDatasheet.sheetValues[0].values.Add(tower);
                    Debug.Log($"Damage at line {i}: {tower}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Damage value at line {i + 1}: {columns[0]}");
                }

                if (float.TryParse(columns[1].Trim(), out float silverCostForTower))
                {
                    googleDatasheet.silverCosts.Add(new SheetSilverCosts());
                    googleDatasheet.silverCosts[0].silverCosts.Add(silverCostForTower);
                    Debug.Log($"Range at line {i}: {silverCostForTower}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Range value at line {i + 1}: {columns[1]}");
                }

                if (float.TryParse(columns[2].Trim(), out float goldCostForTower))
                {
                    googleDatasheet.goldCosts.Add(new SheetGoldCosts());
                    googleDatasheet.goldCosts[0].goldCosts.Add(goldCostForTower);
                    Debug.Log($"Speed at line {i}: {goldCostForTower}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Speed value at line {i + 1}: {columns[2]}");
                }

                
                if (float.TryParse(columns[3].Trim(), out float firstFence))
                {
                    Debug.Log(3333);
                    googleDatasheet.sheetValues.Add(new SheetValues());
                    googleDatasheet.sheetValues[1].values.Add(firstFence);
                    Debug.Log($"Target at line {i}: {firstFence}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Target value at line {i + 1}: {columns[3]}");
                }
                
                if (float.TryParse(columns[4].Trim(), out float firstFencetSilverCost))
                {
                    googleDatasheet.silverCosts.Add(new SheetSilverCosts());
                    googleDatasheet.silverCosts[1].silverCosts.Add(firstFencetSilverCost);
                    Debug.Log($"Target at line {i}: {firstFencetSilverCost}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Target value at line {i + 1}: {columns[4]}");
                }
                
                if (float.TryParse(columns[5].Trim(), out float firstFenceGoldCost))
                {
                    googleDatasheet.goldCosts.Add(new SheetGoldCosts());
                    googleDatasheet.goldCosts[1].goldCosts.Add(firstFenceGoldCost);
                    Debug.Log($"Target at line {i}: {firstFenceGoldCost}");
                }
                else
                {
                    //counter++;
                    Debug.LogError($"Failed to parse Target value at line {i + 1}: {columns[5]}");
                }
                
                
                if (float.TryParse(columns[6].Trim(), out float secondFence))
                {
                    googleDatasheet.sheetValues.Add(new SheetValues());
                    googleDatasheet.sheetValues[2].values.Add(secondFence);
                    Debug.Log($"Target at line {i}: {secondFence}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Target value at line {i + 1}: {columns[6]}");
                }
                
                if (float.TryParse(columns[7].Trim(), out float secondFenceSilverCost))
                {
                    googleDatasheet.silverCosts.Add(new SheetSilverCosts());
                    googleDatasheet.silverCosts[2].silverCosts.Add(secondFenceSilverCost);
                    Debug.Log($"Target at line {i}: {secondFenceSilverCost}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Target value at line {i + 1}: {columns[7]}");
                }
                
                if (float.TryParse(columns[8].Trim(), out float secondFenceGoldCost))
                {
                    googleDatasheet.goldCosts.Add(new SheetGoldCosts());
                    googleDatasheet.goldCosts[2].goldCosts.Add(secondFenceGoldCost);
                    Debug.Log($"Target at line {i}: {secondFenceGoldCost}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Target value at line {i + 1}: {columns[8]}");
                }
                
                
                
                /*if (float.TryParse(columns[9].Trim(), out float criticilMultiplayer))
                {
                    googleDatasheet.sheetValues.Add(new SheetValues());
                    googleDatasheet.sheetValues[3].values.Add(criticilMultiplayer);
                    Debug.Log($"Target at line {i}: {criticilMultiplayer}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Target value at line {i + 1}: {columns[9]}");
                }
                
                if (float.TryParse(columns[10].Trim(), out float criticilMultiplayerSilverCost))
                {
                    googleDatasheet.silverCosts.Add(new SheetSilverCosts());
                    googleDatasheet.silverCosts[3].silverCosts.Add(criticilMultiplayerSilverCost);
                    Debug.Log($"Target at line {i}: {criticilMultiplayerSilverCost}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Target value at line {i + 1}: {columns[10]}");
                }
                
                if (float.TryParse(columns[11].Trim(), out float criticilMultiplayerGoldCost))
                {
                    googleDatasheet.goldCosts.Add(new SheetGoldCosts());
                    googleDatasheet.goldCosts[3].goldCosts.Add(criticilMultiplayerGoldCost);
                    Debug.Log($"Target at line {i}: {criticilMultiplayerGoldCost}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Target value at line {i + 1}: {columns[11]}");
                }
                
                
                
                if (float.TryParse(columns[12].Trim(), out float splash))
                {
                    googleDatasheet.sheetValues.Add(new SheetValues());
                    googleDatasheet.sheetValues[4].values.Add(splash);
                    Debug.Log($"Target at line {i}: {splash}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Target value at line {i + 1}: {columns[12]}");
                }
                
                if (float.TryParse(columns[13].Trim(), out float splashSilverCost))
                {
                    googleDatasheet.silverCosts.Add(new SheetSilverCosts());
                    googleDatasheet.silverCosts[4].silverCosts.Add(splashSilverCost);
                    Debug.Log($"Target at line {i}: {splashSilverCost}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Target value at line {i + 1}: {columns[13]}");
                }
                
                if (float.TryParse(columns[14].Trim(), out float splashGoldCost))
                {
                    googleDatasheet.goldCosts.Add(new SheetGoldCosts());
                    googleDatasheet.goldCosts[4].goldCosts.Add(splashGoldCost);
                    Debug.Log($"Target at line {i}: {splashGoldCost}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Target value at line {i + 1}: {columns[14]}");
                }
                
                
                
                if (float.TryParse(columns[15].Trim(), out float attackRange))
                {
                    googleDatasheet.sheetValues.Add(new SheetValues());
                    googleDatasheet.sheetValues[5].values.Add(attackRange);
                    Debug.Log($"Target at line {i}: {attackRange}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Target value at line {i + 1}: {columns[15]}");
                }
                
                if (float.TryParse(columns[16].Trim(), out float attackRangeSilverCost))
                {
                    googleDatasheet.silverCosts.Add(new SheetSilverCosts());
                    googleDatasheet.silverCosts[5].silverCosts.Add(attackRangeSilverCost);
                    Debug.Log($"Target at line {i}: {attackRangeSilverCost}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Target value at line {i + 1}: {columns[16]}");
                }
                
                if (float.TryParse(columns[17].Trim(), out float attackRangeGoldCost))
                {
                    googleDatasheet.goldCosts.Add(new SheetGoldCosts());
                    googleDatasheet.goldCosts[5].goldCosts.Add(attackRangeGoldCost);
                    Debug.Log($"Target at line {i}: {attackRangeGoldCost}");
                }
                else
                {
                    Debug.LogError($"Failed to parse Target value at line {i + 1}: {columns[17]}");
                }*/
            }
            else
            {
                Debug.LogError($"Insufficient columns at line {i + 1}: {line}");
            }
        }
        Debug.Log("Data fetched and initialized successfully!");
    }

    private void SaveData()
    {
        if (googleDatasheet == null)
        {
            Debug.LogError("DamageData is null. Cannot save.");
            return;
        }

        Debug.Log($"Saving data to ScriptableObject: {googleDatasheet.name}");
        EditorUtility.SetDirty(googleDatasheet);  // Mark the object as dirty
        AssetDatabase.SaveAssets();          // Ensure Unity saves the changes
        AssetDatabase.Refresh();             // Refresh the AssetDatabase to reflect changes
        Debug.Log("ScriptableObject saved successfully!");
    }
}
