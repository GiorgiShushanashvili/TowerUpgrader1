using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager:MonoBehaviour
{
    [SerializeField] private GameObject tower;

    [SerializeField] private GameObject[] _upgradePanels;
    [SerializeField] private GameObject[] _evolvePanel;
    [SerializeField] private Button[] _evolveButtons;
    
    
    [SerializeField] private Button[] _upgradeButtons;
    [SerializeField] private GoogleDataSheetForStats towerStats;
    [SerializeField] private PanelData towerPanel;
    [SerializeField] private Coin goldCoins;


    private bool _allowForNextUpgrade = false;

    private GameObject created;
    private void Awake()
    {
        SetInitialValues();
        created=Instantiate(towerPanel.panels[0].evolveData
            ._evolveObjects[towerPanel.panels[0].evolveData._evolveLevel]);
    }

    private void Start()
    {
        for (int i = 0; i < _upgradeButtons.Length; i++)
        {
            int index = i;
            _upgradeButtons[i].onClick.AddListener(()=>UpgradePanelStats(index));
            //_upgradeButtons[i].onClick.AddListener(()=>); = false;
        }
    }

    private void SetInitialValues()
    {
        for (int i = 0; i < towerPanel.panels.Count; i++)
        {
            SetStates(i,towerPanel.panels[i].permanentLevel);
        }
    }
    
    private void SetStates(int index,int currLvl)
    {
        towerPanel.panels[index].actualValue=towerStats.sheetValues[index].values[currLvl];
        if (currLvl + 1 < towerStats.sheetValues[index].values.Count)
        {
            towerPanel.panels[index].nextValue=towerStats.sheetValues[index].values[currLvl+1];
            towerPanel.panels[index].goldCost=towerStats.goldCosts[index].goldCosts[currLvl];
        }
        else
        {
            towerPanel.panels[index].nextValue = float.PositiveInfinity;
            towerPanel.panels[index].goldCost = float.PositiveInfinity;
        }
    }
    
    
    private void UpgradePanelStats(int index)
    {
        Debug.Log(index);
        if (towerPanel.panels[index].goldCost<=goldCoins.coinAmount&&towerPanel.panels[index].permanentLevel + 1 < towerStats.sheetValues[index].values.Count)
        {
            Debug.Log(999);
            if (towerPanel.panels[index].evolveData.evolveCounter == 5)
            {
                _allowForNextUpgrade = true;
                UpgradeStatsPanelStats(index);
            }
            else
            {
                Debug.Log(888);
                towerPanel.panels[index].evolveData.evolveCounter++;
                towerPanel.panels[index].permanentLevel++;
                SetStates(index, towerPanel.panels[index].permanentLevel);
            }
        }
    }
    
    
    private void UpgradeStatsPanelStats(int index)
    {
        /*if (towerPanel.panels[index].goldCost <= goldCoins.coinAmount && towerPanel.panels[index].permanentLevel + 1 <
            towerStats.sheetValues[index].values.Count)*/
        if(_allowForNextUpgrade)
        {
            
            _evolvePanel[index].gameObject.SetActive(true);
            _upgradePanels[index].gameObject.SetActive(false);
            //towerPanel.panels[index].evolveData._evolveObjects[towerPanel.panels[index].evolveData._evolveLevel].SetActive(false);
            DestroyImmediate(created, true);
            towerPanel.panels[index].evolveData._evolveLevel++;
            Instantiate(towerPanel.panels[index].evolveData
                ._evolveObjects[towerPanel.panels[index].evolveData._evolveLevel]);
            //towerPanel.panels[index].evolveData._evolveObjects[towerPanel.panels[index].evolveData._evolveLevel].SetActive(true);
            //tower=towerPanel.panels[index].evolveData._evolveObjects[towerPanel.panels[index].evolveData._evolveLevel];
            //towerPanel.panels[index].evolveData._evolveObjects[towerPanel.panels[index].evolveData._evolveLevel];
            towerPanel.panels[index].evolveData.evolveCounter = 0;
            _allowForNextUpgrade = false;
        }
    }
}