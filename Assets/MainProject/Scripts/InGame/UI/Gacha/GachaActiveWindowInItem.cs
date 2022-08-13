using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace BlackTree
{
    public class GachaActiveWindowInItem : MonoBehaviour
    {
        [SerializeField] public GachaRewardUISlot GachaGetWeaponSlotPrefab;
        [SerializeField] public GachaRewardUISlot GachaGetItemSlotPrefab;
        [HideInInspector] public List<GachaRewardUISlot> CurrenctGachalist = new List<GachaRewardUISlot>();
        public Transform parent;
        public Button CloseBtn;
        private void Awake()
        {
            CloseBtn.onClick.AddListener(() => this.gameObject.SetActive(false));
            this.gameObject.SetActive(false);
        }

        public void Clear()
        {
            foreach (var _data in CurrenctGachalist)
            {
                Destroy(_data.gameObject);
            }
            CurrenctGachalist.Clear();
        }
        public void Popup()
        {
            this.gameObject.SetActive(true);
            StartCoroutine(StartOpenBoxEvent());
        }

        IEnumerator StartOpenBoxEvent()
        {
            CloseBtn.gameObject.SetActive(false);
            if (CurrenctGachalist.Count>0)
            {
                for(int i=0; i< CurrenctGachalist.Count; i++)
                {
                    CurrenctGachalist[i].gameObject.SetActive(false);
                }
            

                for (int i = 0; i < CurrenctGachalist.Count; i++)
                {
                    CurrenctGachalist[i].gameObject.SetActive(true);
                    CurrenctGachalist[i].transform.SetAsLastSibling();
                    yield return null;
                    yield return null;
                }
            }

            CloseBtn.gameObject.SetActive(true);
        }
    }
}

