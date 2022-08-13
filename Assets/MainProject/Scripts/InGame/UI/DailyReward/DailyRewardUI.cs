using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class DailyRewardUI : MonoBehaviour
    {
        [SerializeField] GameObject RewardSlotPrefab;
        [SerializeField] GameObject parent;
        [SerializeField] Button AttendenceBtn;
        public System.Action RewardAttendCallback;

        public void Init(bool rewardPossible)
        {
            for (int i=0; i<DTConstraintsData.RewardDay; i++)
            {
                var obj = Instantiate(RewardSlotPrefab, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(parent.transform,false);

                obj.GetComponent<DailyRewardSlotUI>().Init(this,i, rewardPossible);
            }
            AttendenceBtn.onClick.AddListener(PushAttend);
        }

        public void PushAttend()
        {
            RewardAttendCallback?.Invoke();
        }

    }

}
