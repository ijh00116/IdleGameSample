using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BlackTree
{
    public class InGameFader : Common.MonoSingleton<InGameFader>
    {
        IEnumerator CallbackCoroutiner;
        Image FadeImage;
        public float fadeSpeed;
        WaitForSeconds waitforfade;
        protected override void Init()
        {
            base.Init();
            FadeImage = GetComponent<Image>();
            FadeImage.color = new Color(0, 0, 0, 0);
            waitforfade = new WaitForSeconds(fadeSpeed);
        }

        protected override void Release()
        {
            base.Release();
        }

        public void FadeGame(IEnumerator Callback)
        {
            CallbackCoroutiner = Callback;

            StartCoroutine(FadeStart());
        }

        IEnumerator FadeStart()
        {
            FadeImage.DOFade(1.0f, fadeSpeed);

            yield return waitforfade;

            yield return StartCoroutine(CallbackCoroutiner);

            FadeImage.DOFade(0.0f, fadeSpeed);

            yield return waitforfade;

        }
    }

}
