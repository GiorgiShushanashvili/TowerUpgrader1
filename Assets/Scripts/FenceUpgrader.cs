using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class FenceUpgrader:MonoBehaviour
{
        public event Action<EvolveInvokeData> OnEvolveSTart;
        public event Action<int> OnFenceToUpgrade;
        public event Action<int> OnEvolveToBeDone;

       // private Timer timer = new Timer();
        [SerializeField] public Wall[] timers;
        
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
        //private int previosuIndexForUpgrade=0;
        //public bool[] _allowForNextUpgrades=new bool[dataForSeparate.Length];


        private int buttonIndex;
        private Vector3 worldPos;
        private Vector3 screenPos;
        
        private void Start()
        {
                /*for (int i = 0; i < dataForSeparate.Length; i++)
                {
                        if(dataForSeparate[i] != null)
                }*/
                
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
                /*if (WorkTimer.Instance.evolved)
                {
                        RefreshDatAFterEvolve(0);
                }*/
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
                if (dataForSeparate[index].evolveData._evolvePrices[dataForSeparate[index].evolveData._evolveLevel] <= goldCoins.coinAmount)
                {
                        OnEvolveSTart?.Invoke(new EvolveInvokeData()
                        {
                                index = index,
                                duration = Convert.ToInt32(dataForSeparate[index].evolveData._evolveDuration[dataForSeparate[index].evolveData._evolveLevel])
                        });
                        //timers[index].StartActualTimer();
                        timers[index].StartTimer(Convert.ToInt32(dataForSeparate[index].evolveData._evolveDuration[dataForSeparate[index].evolveData._evolveLevel]));
                        WorkTimer.Instance.AddNewProcess(Convert.ToInt32(dataForSeparate[index].evolveData._evolveDuration[dataForSeparate[index].evolveData._evolveLevel]));

                        /*if (WorkTimer.Instance.evolved)
                        {
                                RefreshDatAFterEvolve(index);
                        }*/
                        WorkTimer.Instance.OnProcessEnd += RefreshDatAFterEvolve;
                }
        }

        public void RefreshDatAFterEvolve(int index)
        {
                dataForSeparate[index].evolveData._evolveLevel++;
                dataForSeparate[index].evolveData.evolveCounter = 0;
                upgrader.SetActive(true);
                upgrader.transform.position = screenPos;
                evolvePanel.SetActive(false);
                dataForSeparate[index]._allowForUpgrade = true;
                //dataForSeparate[index].evolveData._isEvolved = false;
                WorkTimer.Instance.evolved = false;
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
                                //timer.StartActualTimer();
                                
                                //timer.StartTimer(dataForSeparate[i].evolveData._evolveDuration[dataForSeparate[i].evolveData._evolveLevel]);
                                
                                buttonIndex = i;
                                worldPos=_fenceWalls[i].transform.position + new Vector3(0, 0.7f, 0);
                                screenPos = cam.WorldToScreenPoint(worldPos);
                                
                                OnFenceToUpgrade?.Invoke(i); // სტარტში რომ ჩაიწეროს უი ელემენტებში სტატები
                                switch (dataForSeparate[i]._allowForUpgrade)
                                {
                                        case true:
                                                evolvePanel.SetActive(false);
                                                upgrader.SetActive(true);
                                                upgrader.transform.position = screenPos;
                                                break;
                                        case false:
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