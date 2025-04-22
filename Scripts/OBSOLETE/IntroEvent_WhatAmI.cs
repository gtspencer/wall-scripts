using System.Collections;
using UnityEngine;

namespace Events
{
    public class IntroEvent_WhatAmI : ScriptedEvent
    {
        private bool initialWaitingDone;
        protected override void OnStart()
        {
            WaitForSeconds(3f, DoneInitialWaiting);
        }

        private void DoneInitialWaiting()
        {
            WaitForSideEntrance(SideTriggers.Side.Side1, InitialSide1Entry);
        }

        private void InitialSide1Entry()
        {
            initialWaitingDone = true;
            MainWallCanvas.Instance.WriteImmediateSide2("I've been here a long time.\nAbout as long as I can remember actually.");
        }

        private int side1EntranceCount = 0;
        protected override void OnEnteredSide1()
        {
            if (!initialWaitingDone) return;

            side1EntranceCount++;
            
            var message = "";

            switch (side1EntranceCount)
            {
                case 1:
                    message = "Haha.  Get it?  Because walls are made of stone.";
                    break;
                case 2:
                    message = "I actually don't know what I'm made of.";
                    break;
            }
            
            MainWallCanvas.Instance.WriteImmediateSide2(message);
        }

        private int side2EntranceCount = 0;
        protected override void OnEnteredSide2()
        {
            if (!initialWaitingDone) return;
            
            side2EntranceCount++;
            var message = "";

            switch (side2EntranceCount)
            {
                case 1:
                    message = "And I remember everything: my memory is set in stone!";
                    break;
                case 2:
                    message = "Well, not all walls.  Not me.";
                    break;
                case 3:
                    message = "If I had to guess, though, I'd say wall.  I'm just made of wall.";
                    break;
                case 4:
                    Complete();
                    break;
            }
            
            MainWallCanvas.Instance.WriteImmediateSide1(message);
        }
        
        protected override void OnUpdate()
        {
        
        }

        protected override void OnEnd()
        {
        }
    }
}
