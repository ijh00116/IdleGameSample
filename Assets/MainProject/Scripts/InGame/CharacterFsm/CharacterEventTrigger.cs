using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using BlackTree.State;

namespace BlackTree.InGame
{
    public class CharacterEventTrigger : CharacterAbility
    {
        Vector3 mouseposition;
        Vector3 Screenposition;
        Camera maincam;
        protected override void Start()
        {
            base.Start();
            maincam = FindObjectOfType<Camera>();
        }

        protected override void ProcessAbility()
        {
            base.ProcessAbility();
            if(Input.GetMouseButtonDown(0))
            {
                mouseposition = Input.mousePosition;
                Screenposition = maincam.ScreenToWorldPoint(mouseposition);

                RaycastHit2D hit = Physics2D.Raycast(Screenposition, maincam.transform.forward, 100f);
                if(hit)
                {
                    DebugExtention.ColorLog("red", hit.transform.name);
                }
            }
        }

      
    }

}
