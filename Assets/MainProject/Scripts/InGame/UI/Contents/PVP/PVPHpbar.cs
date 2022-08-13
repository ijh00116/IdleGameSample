using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DLL_Common.Common;
using DG.Tweening;

namespace BlackTree
{
    public class PVPHpbar : MonoBehaviour
    {
        [SerializeField] Slider hpBar;
        public void Init()
        {
            Message.AddListener<UI.Event.pvpHpDamgaged>(hpBarChange);
        }

        public void Release()
        {
            Message.RemoveListener<UI.Event.pvpHpDamgaged>(hpBarChange);
        }
        public void StartPvp()
        {
            hpBar.value = 0.5f;
        }
        void hpBarChange(UI.Event.pvpHpDamgaged msg)
        {
            float barValue = hpBar.value;

            if(msg.charactertype==InGame.CharacterType.PvpPlayer)
            {
                barValue -= msg.sliderValue;
            }
            else
            {
                barValue += msg.sliderValue;
            }
            hpBar.value = barValue;

            //hpBar.DOValue(barValue, 0.1f);
        }
    }

}
