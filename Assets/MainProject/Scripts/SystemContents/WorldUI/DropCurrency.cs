using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BlackTree
{
    public class DropCurrency : MonoBehaviour
    {
        [SerializeField]float jumppower;
        [SerializeField] int jumnum;
        [SerializeField] float during;

        [SerializeField] SpriteRenderer CurrencyImage;
        [SerializeField] List<Sprite> CurrencySprite = new List<Sprite>();
        public void Init(bool gold)
        {
            if (gold)
                CurrencyImage.sprite = CurrencySprite[0];
            else
                CurrencyImage.sprite = CurrencySprite[1];
        }

        public void Test()
        {
            transform.localPosition = Vector3.zero;
            float randomvalue = Random.Range(-2, 2);
            this.transform.DOJump(new Vector3(this.transform.position.x+randomvalue,
                this.transform.position.y, this.transform.position.z), UnityEngine.Random.Range(2.5f,3.5f), Random.Range(1,3), Random.Range(during-0.3f, during+0.3f));
        }
        void Test_0()
        {
            transform.localPosition = Vector3.zero;
            this.transform.DOPunchPosition(new Vector3(0, 1, 0), jumppower, jumnum, during);
        }
    }

}
