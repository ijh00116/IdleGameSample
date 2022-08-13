using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class EffectText : MonoBehaviour
    {
        [SerializeField] Text Name;
        [SerializeField] Text value;
        [SerializeField] Text nextvalue;
        public void SetText(string name,string _Value,string nextValue,bool active=false)
        {
            this.gameObject.SetActive(active);
            if (active == false)
                return;
            
            Name.text = name;
            value.text = _Value;
            nextvalue.text = nextValue;

        }
    }

}
