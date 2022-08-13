using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class ArtifactUIETC : MonoBehaviour
    {
        [SerializeField] Button ChooseArtifact;

        [HideInInspector] public RelicInventoryObject _artifactInventory;

        [SerializeField] Text RelicCount;

        public void Init()
        {
            _artifactInventory = Common.InGameManager.Instance.RelicInventory;
            ChooseArtifact.onClick.AddListener(TouchChooseArtifactButton);

            CheckPossessRelic();
        }

        public void Release()
        {
            ChooseArtifact.onClick.RemoveAllListeners();
        }

        void TouchChooseArtifactButton()
        {
            if(_artifactInventory==null)
                _artifactInventory= Common.InGameManager.Instance.RelicInventory;

            int index = 0;
            while(true)
            {
                index = Random.Range(0, _artifactInventory.GetSlots.Count);
                if (_artifactInventory.GetSlots[index].item.Unlocked==false)
                    break;
            }
            _artifactInventory.GetSlots[index].AddAmount(1);

            CheckPossessRelic();
        }

        bool CheckPossessRelic()
        {
            int CurrentUnlockedCount = 0;
            for (int i = 0; i < _artifactInventory.GetSlots.Count; i++)
            {
                if (_artifactInventory.GetSlots[i].item.Unlocked==true)
                {
                    CurrentUnlockedCount++;
                }
            }
            RelicCount.text = string.Format("<color=yellow>{0}</color>/<color=white >{1}</color>", CurrentUnlockedCount, _artifactInventory.GetSlots.Count);

            if (CurrentUnlockedCount >= _artifactInventory.GetSlots.Count)
            {
                ChooseArtifact.gameObject.SetActive(false);
                return true;
            }
            
            return false;
        }
    }
}
