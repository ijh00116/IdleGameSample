using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree.UI
{
    public class ScaleRectEffect : IUiEffect
    {
        private RectTransform recttransform { get; }
        Vector3 MaxSize { get; }
        float ScaleSpeed { get; }
        private YieldInstruction Wait { get; }
        public ScaleRectEffect(RectTransform rect,Vector3 maxsize,float scalespeed,YieldInstruction wait)
        {
            recttransform = rect;
            MaxSize = maxsize;
            ScaleSpeed = scalespeed;
            Wait = wait;
        }
        public IEnumerator Execute()
        {
            var time = 0f;
            var currentScale = recttransform.localScale;
            while(recttransform.localScale!=MaxSize)
            {
                time += Time.deltaTime * ScaleSpeed;
                var scale = Vector3.Lerp(currentScale, MaxSize, time);
                recttransform.localScale = scale;
                yield return null;
            }
            yield return Wait;

            time = 0f;
            currentScale = recttransform.localScale;
            while (recttransform.localScale != Vector3.one)
            {
                time += Time.deltaTime * ScaleSpeed;
                var scale = Vector3.Lerp(currentScale, Vector3.one, time);
                recttransform.localScale = scale;
                yield return null;
            }
        }

    }

}
