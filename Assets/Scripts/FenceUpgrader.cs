using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FenceUpgrader:MonoBehaviour
{
        Camera cam;
        public LayerMask mask;
        
        
        [SerializeField] private GameObject[] _fenceWalls;
        [SerializeField] private GameObject[] fences;
        
        
        //[SerializeField] RayCastScript rayCastScript;
        
        [SerializeField] DataForSeparate[] dataForSeparate;
        [SerializeField] GoogleDataSheetForStats towerDataSheet;

        [SerializeField] Button[] upgradeButtons;
        [SerializeField] Button[] evolveButtons;

        private int previosuIndexForUpgrade=0;
        private void Start()
        {
                cam = Camera.main;
                
                SetInitialSTats();
                for (int i = 0; i < upgradeButtons.Length; i++)
                {
                        int index = i;
                        //fenceButton[index].onClick.AddListener(() =>ChooseFence(index));
                        upgradeButtons[i].onClick.AddListener(()=>UpgradePanelStats(index));
                }
        }


        private void Update()
        {
                if (Input.GetMouseButtonDown(0))
                {
                        ChooseFence();
                }
        }

        private void UpgradePanelStats(int index)
        {
                dataForSeparate[index].permanentLevel++;
                SetStats(index,dataForSeparate[index].permanentLevel);
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

        
        public void ChooseFence()
        {
                 var targetObject =Detector();

                 for (int i = 0; i < _fenceWalls.Length; i++)
                 {
                         if(targetObject.CompareTag(_fenceWalls[i].tag))
                         {
                                 fences[previosuIndexForUpgrade].gameObject.SetActive(false);
                                 previosuIndexForUpgrade = i;
                                 fences[previosuIndexForUpgrade].gameObject.SetActive(true); 
                         }
                         /*if (targetObject.tag == _fenceWalls[i].tag)
                         {
                                 fences[previosuIndexForUpgrade].gameObject.SetActive(false);
                                 previosuIndexForUpgrade = i;
                                 fences[previosuIndexForUpgrade].gameObject.SetActive(true); 
                         }*/
                 }
        }
        
        public GameObject Detector()
        {
                GameObject gg = new GameObject();
        
                Vector3 mousPos = Input.mousePosition;
                mousPos.x = Mathf.Clamp(mousPos.x, 0, Screen.width);
                mousPos.y = Mathf.Clamp(mousPos.y, 0, Screen.height);
                mousPos.z = 100f;
                mousPos= cam.ScreenToWorldPoint(mousPos);
                Debug.DrawRay(transform.position,mousPos-transform.position,Color.red);

                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, mask))
                {
                        gg= hit.collider.gameObject; 
                }
                /*if (Input.GetMouseButtonDown(0))
                {
                        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 100, mask))
                        {
                                gg= hit.collider.gameObject; 
                        }
                }*/
                return gg;
        }
}