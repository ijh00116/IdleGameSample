using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class RankingUISlot : MonoBehaviour
    {
        [SerializeField] Text rankNumber;

        [SerializeField] Image RankOneBG;
        [SerializeField] Image RankTwoBG;
        [SerializeField] Image RankThreeBG;

        [SerializeField] Image RankOneImage;
        [SerializeField] Image RankTwoImage;
        [SerializeField] Image RankThreeImage;
        [SerializeField] Image RankNumberImage;

        [SerializeField] Image ProfilIcon;
        [SerializeField] Text UserName;
        [SerializeField] Text ScoreText;

        [SerializeField] Sprite FirstSprite;
        [SerializeField] Sprite SecondSprite;
        [SerializeField] Sprite ThirdSprite;
        [SerializeField] Sprite NormalSprite;
        public void Init(UserInfoForPVP userinfo,int rank,bool pvp)
        {
            rankNumber.text = (rank+1).ToString();
            UserName.text = userinfo.NickName;
            if (pvp)
            {
                ScoreText.text = string.Format("{0}\n{1}", "랭크점수", userinfo.RankScore);
            }
            else
            {
                int scenario=(int)(userinfo.RankScore/1000);
                int chapter= (int)(userinfo.RankScore / 100)-scenario*10;
                int stage= userinfo.RankScore-(scenario*1000+chapter*100);
                ScoreText.text = string.Format("시나리오:{0}\n챕터:{1}스테이지:{2}", scenario, chapter, stage);
            }
            RankOneBG.gameObject.SetActive(false);
            RankTwoBG.gameObject.SetActive(false);
            RankThreeBG.gameObject.SetActive(false);
            RankOneImage.gameObject.SetActive(false);
            RankTwoImage.gameObject.SetActive(false);
            RankThreeImage.gameObject.SetActive(false);
            RankNumberImage.gameObject.SetActive(false);

            if (rank == 0)
            {
                RankOneBG.gameObject.SetActive(true);
                RankOneImage.gameObject.SetActive(true);
            }
            else if (rank == 1)
            {
                RankTwoBG.gameObject.SetActive(true);
                RankTwoImage.gameObject.SetActive(true);
            }
            else if (rank == 2)
            {
                RankThreeBG.gameObject.SetActive(true);
                RankThreeImage.gameObject.SetActive(true);
            }
            else
            {
                RankNumberImage.gameObject.SetActive(true);
            }
        }
    }

}
