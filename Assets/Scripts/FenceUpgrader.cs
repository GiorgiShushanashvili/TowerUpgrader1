using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class FenceUpgrader:MonoBehaviour
{
        public event Action<int> OnFenceToUpgrade;
        public event Action<int> OnEvolveToBeDone;
        
        
        Camera cam;
        public LayerMask mask;

        [SerializeField] private GameObject upgrader;
        [SerializeField] private GameObject evolvePanel;
        
        [SerializeField] private GameObject[] _fenceWalls;
        
        [SerializeField] DataForSeparate[] dataForSeparate;
        [SerializeField] GoogleDataSheetForStats towerDataSheet;
        [SerializeField] private Coin goldCoins;

        [SerializeField] Button upgradeButtons;
        [SerializeField] Button evolveButtons;

        
        private GameObject targetObject;
        private int previosuIndexForUpgrade=0;
        //public bool[] _allowForNextUpgrades=new bool[dataForSeparate.Length];


        private int buttonIndex;
        private Vector3 worldPos;
        private Vector3 screenPos;
        
        private void Start()
        {
                upgrader.SetActive(false);
                evolvePanel.SetActive(false);
                cam = Camera.main;
                
                SetInitialSTats();
                upgradeButtons.onClick.AddListener(()=>UpgradePanelStats(buttonIndex));
                evolveButtons.onClick.AddListener(()=>Evolve(buttonIndex));
        }

        private void OnEnable()
        {
                OnEvolveToBeDone += ConvertToEvolve;
        }

        private void OnDisable()
        {
                OnEvolveToBeDone-= ConvertToEvolve;
        }

        private void Update()
        {
                if (Input.GetMouseButtonDown(0))
                {
                        ChooseFence();
                }
        }

        #region Upgrade
        private void UpgradePanelStats(int index)
        {
                if (dataForSeparate[index]._allowForUpgrade&&dataForSeparate[index].silverCost<=goldCoins.coinAmount)
                {
                        dataForSeparate[index].permanentLevel++;
                        SetStats(index, dataForSeparate[index].permanentLevel);
                        goldCoins.ReduceCoins(dataForSeparate[index].goldCost);
                        OnFenceToUpgrade?.Invoke(index);
                        dataForSeparate[index].evolveData.evolveCounter++;
                        if (dataForSeparate[index].evolveData.evolveCounter == 5)
                        {
                                dataForSeparate[index]._allowForUpgrade = false;
                                OnEvolveToBeDone?.Invoke(index);
                        }
                }
        }
        

        private void SetInitialSTats()
        {
                for (int i = 0; i < dataForSeparate.Length; i++)
                {
                        SetStats(i, dataForSeparate[i].permanentLevel);
                }
        }
        
        private void SetStats(int index,int currLvl)
        {
                dataForSeparate[index].actualValue= towerDataSheet.sheetValues[1].values[currLvl];
                if (currLvl + 1 < towerDataSheet.sheetValues[1].values.Count)
                {
                        dataForSeparate[index].nextValue=towerDataSheet.sheetValues[1].values[currLvl+1];
                        dataForSeparate[index].goldCost=towerDataSheet.goldCosts[1].goldCosts[currLvl];
                }
                else
                {
                        dataForSeparate[index].nextValue = float.PositiveInfinity;
                        dataForSeparate[index].goldCost = float.PositiveInfinity;
                }
        }
        #endregion

        #region Evolve

        public void ConvertToEvolve(int index)
        {
                if (!dataForSeparate[index]._allowForUpgrade)
                {
                        upgrader.SetActive(false);
                        evolvePanel.SetActive(true);
                        evolvePanel.transform.position = screenPos;
                }
        }

        public void Evolve(int index)
        {
                if (dataForSeparate[index].evolveData._evolvePrices[0] <= goldCoins.coinAmount)
                {
                        dataForSeparate[index].evolveData._evolveLevel++;
                        dataForSeparate[index].evolveData.evolveCounter = 0;
                        upgrader.SetActive(true);
                        upgrader.transform.position = screenPos;
                        evolvePanel.SetActive(false);
                        dataForSeparate[index]._allowForUpgrade = true;
                }
        }

        #endregion
        
        #region Raycast
        public void ChooseFence()
        {
                Detector();
                for (int i = 0; i < _fenceWalls.Length; i++)
                {
                        if (targetObject != null&&targetObject.CompareTag(_fenceWalls[i].tag))
                        {
                                buttonIndex = i;
                                worldPos=_fenceWalls[i].transform.position + new Vector3(0, 0.7f, 0);
                                screenPos = cam.WorldToScreenPoint(worldPos);
                                
                                OnFenceToUpgrade?.Invoke(i); // სტარტში რომ ჩაიწეროს უი ელემენტებში სტატები
                                switch (dataForSeparate[i]._allowForUpgrade)
                                {
                                        case true:
                                                Debug.Log(333);
                                                evolvePanel.SetActive(false);
                                                upgrader.SetActive(true);
                                                upgrader.transform.position = screenPos;
                                                break;
                                        case false:
                                                Debug.Log(333);
                                                upgrader.SetActive(false);
                                                evolvePanel.transform.position = screenPos;
                                                evolvePanel.SetActive(true);
                                                break;
                                }
                        }
                }
        }
        
        public void Detector()
        {
                Vector3 mousPos = Input.mousePosition;
                mousPos.x = Mathf.Clamp(mousPos.x, 0, Screen.width);
                mousPos.y = Mathf.Clamp(mousPos.y, 0, Screen.height);
                mousPos.z = 100f;
                mousPos= cam.ScreenToWorldPoint(mousPos);
                Debug.DrawRay(transform.position,mousPos-transform.position,Color.red,0.5f);
                
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, mask))
                {
                        targetObject= hit.collider.gameObject;
                }
                else
                {
                        targetObject= null;
                        evolvePanel.SetActive(false);
                        upgrader.SetActive(false);
                }
        }
        #endregion
}