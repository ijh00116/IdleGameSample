using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BlackTree.UI
{
    public class CustomButtonEventTrigger : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnter(gameObject); });
            AddEvent(gameObject, EventTriggerType.PointerDown, delegate { PointerDown(gameObject); });
        }

        // Update is called once per frame
        void Update()
        {

        }

        void AddEvent(GameObject obj,EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            var eventTrigger = new EventTrigger.Entry();
            eventTrigger.eventID = type;
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        void OnEnter(GameObject obj)
        {
          
        }

        void PointerDown(GameObject obj)
        {
           
        }
    }

}
