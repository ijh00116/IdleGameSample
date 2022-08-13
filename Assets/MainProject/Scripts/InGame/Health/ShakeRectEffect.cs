using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree.UI
{
    public class ShakeRectEffect : IUiEffect
    {
        private RectTransform recttransform { get; }
        float WiggleSpeed{ get; }
        float MaxRotation { get; }
        public ShakeRectEffect(RectTransform rect, float maxrotation, float speed)
        {
            recttransform = rect;
            MaxRotation = maxrotation;
            WiggleSpeed = speed;
        }
        public IEnumerator Execute()
        {
            var rotateTo = new Quaternion
            {
                eulerAngles = new Vector3(0, 0, MaxRotation)
            };
            var currentRotation = recttransform.rotation.z;
            var nextRotation = MaxRotation * -1f;

            var time = 0f;
            
            while (Mathf.Abs(nextRotation)>0.15f)
            {
                time += Time.deltaTime * WiggleSpeed;
                var newrotation = Mathf.Lerp(currentRotation, nextRotation, time);
                rotateTo.eulerAngles = new Vector3(0, 0, newrotation);
                recttransform.rotation= rotateTo;
                if(time>=1)
                {
                    currentRotation = nextRotation;
                    nextRotation = (nextRotation * 0.9f) * -1;
                    time = 0;
                }
                yield return null;
            }

            rotateTo.eulerAngles = new Vector3(0, 0, 0);
            recttransform.rotation = rotateTo;
        }

    }

}
