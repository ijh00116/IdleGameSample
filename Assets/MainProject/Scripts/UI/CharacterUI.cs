using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class CharacterUI : MonoBehaviour
    {
        public GameObject LevelUpWindow;
        public Button LevelUpWindowBtn;
        public GameObject LevelUpWindowOn;
      
        private void Awake()
        {
            LevelUpWindowBtn.onClick.AddListener(() => { SetWindowLastSibling(LevelUpWindow); });
         

            LevelUpWindowOn.SetActive(false);
           

            SetWindowLastSibling(LevelUpWindow);
        }
        private void OnDestroy()
        {
            
        }
        void SetWindowLastSibling(GameObject obj)
        {
            LevelUpWindowOn.SetActive(false);
           
            if (obj == LevelUpWindow)
                LevelUpWindowOn.SetActive(true);

            obj.transform.SetAsLastSibling();
        }
    }

}
