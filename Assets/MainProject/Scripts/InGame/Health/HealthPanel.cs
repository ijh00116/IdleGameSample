using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlackTree.InGame;

namespace BlackTree.UI
{
    public class HealthPanel : MonoBehaviour
    {
        [SerializeField] private Image slider;
        [SerializeField] private float waitTime;
        [Header("scale effect")]
        [SerializeField] private Vector3 maxScaleSize;
        [SerializeField] private float scaleSpeed;

        [Header("shake effect")]
        [SerializeField] private float maxShakeRotation;
        [SerializeField] private float shakeSpeed;
        Health health;

        private IUiEffect _effect;
        private IUiEffect _effect2;
        private YieldInstruction _wait;
        private void Awake()
        {
            _wait = new WaitForSeconds(waitTime);
            _effect = new ScaleRectEffect(GetComponent<RectTransform>(), maxScaleSize,scaleSpeed,_wait);
            _effect2 = new ShakeRectEffect(GetComponent<RectTransform>(), maxShakeRotation, shakeSpeed);
        }

        private void OnEnable()
        {
            slider.fillAmount = 1.0f;
        }

        private void OnDisable()
        {
            
        }

        private void OnDestroy()
        {
            if(health!=null)
            health.hpbarCallback -= HandleHealthChanged;
        }

        public void HandleHealthChanged(float currentHealth)
        {
            slider.fillAmount = currentHealth;

            StopAllCoroutines();
            StartCoroutine(_effect.Execute());
            StartCoroutine(_effect2.Execute());
        }
    }

}
