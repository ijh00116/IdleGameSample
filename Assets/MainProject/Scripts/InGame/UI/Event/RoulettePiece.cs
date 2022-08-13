using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class RoulettePiece : MonoBehaviour
    {
        public Image backgroundImage;

        public Image rewardIcon;
        public Text rewardAmount;

        public RouletteTabledata myData;
        [HideInInspector]public Sprite mySprite;

        [HideInInspector] public int index;

        [HideInInspector] public RouletteController _controller;
        public void SetValues(RouletteController controller, int pieceNo)
        {
            _controller = controller;

            index = pieceNo;
            myData = controller.PiecesOfWheel[pieceNo];

            if (controller.useCustomBackgrounds)
            {
                backgroundImage.color = Color.white;
                backgroundImage.sprite = controller.CustomBackgrounds[pieceNo];
            }
            if(myData.reward_type==RewardType.REWARD_BOX)
            {
                rewardAmount.text = "1";
            }
            else
            {
                rewardAmount.text = myData.reward_count.ToString();
            }
            
          
            mySprite = Common.InGameManager.Instance.UIIconImageList[myData.reward_type];
            rewardIcon.sprite = mySprite;
        }
    }

}
