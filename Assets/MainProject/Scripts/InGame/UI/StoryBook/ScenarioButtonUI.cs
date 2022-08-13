using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class ScenarioButtonUI : MonoBehaviour
    {
        public Button ScenarioButton;
        //[SerializeField]Text ButtonText;
        Action<int> buttoncallback;
        int MyScenarioIndex;
        public void Setting(int index,Action<int> buttonCallback)
        {
            MyScenarioIndex = index;
            buttoncallback = buttonCallback;
            //ButtonText.text = (MyScenarioIndex + 1).ToString();
            ScenarioButton.onClick.AddListener(()=> { buttoncallback(MyScenarioIndex); } );
        }

        public void Push()
        {
            buttoncallback(MyScenarioIndex);
        }
    }

}
