using UnityEngine;

namespace Events
{
    public class IntroEvent_RemoveText : ScriptedEvent
    {
        [SerializeField] private GameObject mainWallText;
        protected override void OnStart()
        {
        }

        protected override void OnEnteredSide1()
        {
            
        }

        protected override void OnEnteredSide2()
        {
            mainWallText.SetActive(false);
            Complete();
        }
        
        protected override void OnUpdate()
        {
        
        }

        protected override void OnEnd()
        {
        }
    }
}
