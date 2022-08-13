using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class BuffChecker : MonoBehaviour
    {
        [SerializeField] Image silhouette;
        [SerializeField] Image onImage;
        [SerializeField] Text  durationTimetext;

        float attackcurrenttime;
        float attackMaxTime;
        bool IsAttackImprove;

        float CurrentImprovedValue;

        private void Awake()
        {
            silhouette.gameObject.SetActive(true);
            onImage.gameObject.SetActive(false);
            durationTimetext.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            
        }

        private void Update()
        {
            if (IsAttackImprove)
            {
                float timer = attackMaxTime - attackcurrenttime;

                int minutes = Mathf.FloorToInt(timer / 60F);
                int seconds = Mathf.FloorToInt(timer - minutes * 60);
                string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
                durationTimetext.text = niceTime;
            }
        }

        private void FixedUpdate()
        {
            if (IsAttackImprove)
            {
                attackcurrenttime += Time.deltaTime;
                if (attackcurrenttime > attackMaxTime)
                {
                    IsAttackImprove = false;
                    //enforcementUI와 같이 캐릭터별로 SetAbilityValue 해야 하나 테이블 데이터 아직 안나와서 세팅 아직 안할 예정
                    //Common.InGameManager.Instance.GetPlayerData.ImprovedAttackPower -= CurrentImprovedValue;
                    attackMaxTime = 0;

                    silhouette.gameObject.SetActive(true);
                    onImage.gameObject.SetActive(false);
                    durationTimetext.gameObject.SetActive(false);
                }
#if UNITY_EDITOR
                Debug.Log(attackcurrenttime);
#endif
            }
        }

        public void BuffStart(float value,float durationTime,Attributes att)
        {
            silhouette.gameObject.SetActive(false);
            onImage.gameObject.SetActive(true);
            durationTimetext.gameObject.SetActive(true);

            CurrentImprovedValue = value;
            if (IsAttackImprove==false)
            {
                //enforcementUI와 같이 캐릭터별로 SetAbilityValue 해야 하나 테이블 데이터 아직 안나와서 세팅 아직 안할 예정
                //Common.InGameManager.Instance.GetPlayerData.ImprovedAttackPower += value;
                attackcurrenttime = 0;
            }
            else
            {

            }
            
            attackMaxTime += durationTime;
            IsAttackImprove = true;
        }
    }
}
