using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class GachaWindowUI : MonoBehaviour
    {
        [SerializeField] Button WeaponTab;
        [SerializeField] Button WingTab;
        [SerializeField] Button PetTab;
        [SerializeField] Button S_RelicTab;

        [SerializeField] GameObject WeaponTabSelected;
        [SerializeField] GameObject WingTabSelected;
        [SerializeField] GameObject PetTabSelected;
        [SerializeField] GameObject S_RelicTabSelected;

        [SerializeField] GameObject WeaponBg;
        [SerializeField] GameObject WingBg;
        [SerializeField] GameObject SrelicBg;
        [SerializeField] GameObject PetBg;

        [SerializeField] GachaContentUI WeaponWindow;
        [SerializeField] GachaContentUI WingWindow;
        [SerializeField] GachaContentUI PetWindow;
        [SerializeField] GachaContentUI S_RelicWindow;

        [SerializeField] GachaActiveWindow gachaactiveUI;
        public void Init()
        {
            WeaponTab.onClick.AddListener(() => WindowOn(WeaponWindow.gameObject,WeaponBg,WeaponTabSelected));
            WingTab.onClick.AddListener(() => WindowOn(WingWindow.gameObject, WingBg,WingTabSelected));
            PetTab.onClick.AddListener(() => WindowOn(PetWindow.gameObject, PetBg,PetTabSelected));
            S_RelicTab.onClick.AddListener(() => WindowOn(S_RelicWindow.gameObject, SrelicBg,S_RelicTabSelected));


            WindowOn(WeaponWindow.gameObject, WeaponBg, WeaponTabSelected);

            WeaponWindow.GetComponent<GachaContentUI>().Init();
            WingWindow.GetComponent<GachaContentUI>().Init();
            PetWindow.GetComponent<GachaContentUI>().Init();
            S_RelicWindow.GetComponent<GachaContentUI>().Init();

            gachaactiveUI.Init();
        }

        public void Release()
        {
            WeaponWindow.GetComponent<GachaContentUI>().Release();
            WingWindow.GetComponent<GachaContentUI>().Release();
            PetWindow.GetComponent<GachaContentUI>().Release();
            S_RelicWindow.GetComponent<GachaContentUI>().Release();
            gachaactiveUI.Release();
        }

        void WindowOn(GameObject obj,GameObject bg,GameObject selectedimage)
        {
            WeaponWindow.gameObject.SetActive(false);
            WingWindow.gameObject.SetActive(false);
            PetWindow.gameObject.SetActive(false);
            S_RelicWindow.gameObject.SetActive(false);

            WeaponBg.SetActive(false);
            WingBg.SetActive(false);
            SrelicBg.SetActive(false);
            PetBg.SetActive(false);

            WeaponTabSelected.SetActive(false);
            WingTabSelected.SetActive(false);
            PetTabSelected.SetActive(false);
            S_RelicTabSelected.SetActive(false);

            obj.SetActive(true);
            bg.SetActive(true);
            selectedimage.SetActive(true);
        }

    }

}
