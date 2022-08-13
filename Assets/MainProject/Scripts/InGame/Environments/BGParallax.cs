using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree.BG
{
    public class BGParallax : MonoBehaviour
    {
        private float  startpos;
        float length;

        public GameObject maincam;
        public float parallaxEffect;

        void Start()
        {
            //Init();
        }

        private void FixedUpdate()
        {
            //AdjustLocation();
        }

        private void OnPreRender()
        {
             //AdjustLocation();
        }

        private void OnRenderObject()
        {
            if (init == false)
                return;
            AdjustLocation();
        }
        bool init = false;
        public void Init()
        {
            init = true;
            float dist = (maincam.transform.position.x * parallaxEffect); //메인캠과 떨어질 거리

            transform.position = new Vector3(maincam.transform.position.x,
              transform.position.y, transform.position.z);
            startpos = transform.position.x;
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        void AdjustLocation()
        {
            float temp = (maincam.transform.position.x * (1 - parallaxEffect));//
            float dist = (maincam.transform.position.x * parallaxEffect); //메인캠과 떨어질 거리

            transform.position = new Vector3(startpos + dist,
                transform.position.y, transform.position.z);

            if (temp > startpos + length) 
                startpos += length;
            else if (temp < startpos - length) 
                startpos -= length;

           
        }
    }
}
