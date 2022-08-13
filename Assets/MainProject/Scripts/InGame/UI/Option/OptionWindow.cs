using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class OptionWindow : MonoBehaviour
    {
        [SerializeField]Button CloseBtn;
        [SerializeField] Slider VolumeSlider;
        [SerializeField] Slider zoomSlider;
        [SerializeField] Button SavingBatteryMode;
        [SerializeField] Button OpenCafe;

        string CafeUrl = "https://cafe.naver.com/gloryknight";
        public void Init()
        {
            CloseBtn.onClick.AddListener(Closewindow);
            VolumeSlider.onValueChanged.AddListener(sliderValueListener);
            zoomSlider.onValueChanged.AddListener(ZoomsliderValueListener);
            SavingBatteryMode.onClick.AddListener(PopupSavingBatteryWindow);
            OpenCafe.onClick.AddListener(OpenUrl);

            Common.InGameManager.Instance.ZoomValueChange(Common.InGameManager.Instance.GetPlayerData.GlobalUser.ZoomValue);
            zoomSlider.value = Common.InGameManager.Instance.GetPlayerData.GlobalUser.ZoomValue;
        }

        public void Release()
        {
            CloseBtn.onClick.RemoveAllListeners();
            VolumeSlider.onValueChanged.RemoveAllListeners();
            zoomSlider.onValueChanged.RemoveAllListeners();
            SavingBatteryMode.onClick.RemoveAllListeners();
        }

        void Closewindow()
        {
            this.gameObject.SetActive(false);
        }

        public void sliderValueListener(float value)
        {
            SoundManager.Instance.volume = value;
        }

        public void ZoomsliderValueListener(float value)
        {
            Common.InGameManager.Instance.ZoomValueChange(value);
        }

        void PopupSavingBatteryWindow()
        {
            Application.targetFrameRate = 10;
            Message.Send<UI.Event.SavingBatteryMessage>(new UI.Event.SavingBatteryMessage(true));
        }

        void OpenUrl()
        {
            Application.OpenURL(CafeUrl);
        }
    }

}
