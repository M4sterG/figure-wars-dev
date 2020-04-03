using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    using UnityEngine.EventSystems;
 
    public static class ExtensiveTriggers
    {
        public static void AddListener (this EventTrigger trigger, EventTriggerType eventType, System.Action<PointerEventData> listener)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener(data => listener.Invoke((PointerEventData)data));
            trigger.triggers.Add(entry);
        }
    }
}