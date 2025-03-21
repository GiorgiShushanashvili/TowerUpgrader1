using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager:MonoBehaviour
{
    [SerializeField] private GameObject tower;
    
    [SerializeField] private Button[] _upgradeButtons;
    [SerializeField] private GoogleDataSheetForStats towerStats;
    [SerializeField] private PanelData towerPanel;
    [SerializeField] private Coin goldCoins;

    [SerializeField] private GameObject[] _evolveButtons;

    private void UpgradeStatsPanelStats(int index)
    {
        if (towerPanel.panels[index].goldCost <= goldCoins.coinAmount && towerPanel.panels[index].permanentLevel + 1 <
            towerStats.sheetValues[index].values.Count)
        {
            goldCoins.ReduceCoins(towerPanel.panels[index].goldCost);
                
                
            towerPanel.panels[index].evolveData.evolveCounter++;
            if (towerPanel.panels[index].evolveData.evolveCounter == 5)
            {
                towerPanel.panels[index].evolveData._evolveLevel++;
                //towerPanel.panels[index].evolveData._evolveObjects[towerPanel.panels[index].evolveData._evolveLevel];
                towerPanel.panels[index].evolveData.evolveCounter = 0;
            }
        }
    }
}