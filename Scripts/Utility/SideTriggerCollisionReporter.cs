using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SideTriggerCollisionReporter : MonoBehaviour
{
    public UnityEvent PlayerEntered = new UnityEvent();
    public UnityEvent PlayerExited = new UnityEvent();
    public UnityEvent PlayerStayed = new UnityEvent();

    private bool playerIsWall;

    private bool _paused;

    public void TogglePaused(bool paused)
    {
        _paused = paused;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_paused) return;
        
        if (!other.CompareTag("Player") && !playerIsWall) return;

        if (!other.CompareTag("WallCamera") && playerIsWall) return;
        
        PlayerEntered.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (_paused) return;
        
        if (!other.CompareTag("Player") && !playerIsWall) return;
        
        if (!other.CompareTag("WallCamera") && playerIsWall) return;
        
        PlayerExited.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_paused) return;
        
        if (!other.CompareTag("Player") && !playerIsWall) return;
        
        if (!other.CompareTag("WallCamera") && playerIsWall) return;
        
        PlayerStayed.Invoke();
    }

    public void PlayerSwitchedToWall()
    {
        playerIsWall = true;
    }

    public void PlayerSwitchedToPlayer()
    {
        playerIsWall = false;
    }
}
