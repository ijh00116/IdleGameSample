using UnityEngine;

namespace BlackTree
{
    public class CinemachineSwitcher : MonoBehaviour
    {
        private Animator animator;
        private bool NormalCamera = true;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            
        }

        public void ChangeCamera(bool Boss)
        {
            if(Boss == false)
            {
                animator.Play("Normal");
            }
            else
            {
                animator.Play("Boss");
            }
            NormalCamera = !Boss;
        }

        void SwitchPriority()
        {
          
            NormalCamera = !NormalCamera;
        }
    }

}
