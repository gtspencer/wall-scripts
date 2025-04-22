using UnityEngine;

public class MinigameInputController : MonoBehaviour
{
    public Vector2 MoveVector = Vector2.zero;
    public Vector2 LookVector = Vector2.zero;
    
    public void OnUp_Up(bool look = false)
    {
        if (look)
        {
            LookVector.y = 0;
            return;
        }
        
        MoveVector.y = 0;
    }
    
    public void OnUp_Down(bool look = false)
    {
        if (look)
        {
            LookVector.y = 1;
            return;
        }
        
        MoveVector.y = 1;
    }
    
    public void OnDown_Up(bool look = false)
    {
        if (look)
        {
            LookVector.y = 0;
            return;
        }
        
        MoveVector.y = 0;
    }
    
    public void OnDown_Down(bool look = false)
    {
        if (look)
        {
            LookVector.y = -1;
            return;
        }
        
        MoveVector.y = -1;
    }
    
    public void OnLeft_Up(bool look = false)
    {
        if (look)
        {
            LookVector.x = 0;
            return;
        }
        
        MoveVector.x = 0;
    }
    
    public void OnLeft_Down(bool look = false)
    {
        if (look)
        {
            LookVector.x = -1;
            return;
        }
        
        MoveVector.x = -1;
    }
    
    public void OnRight_Up(bool look = false)
    {
        if (look)
        {
            LookVector.x = 0;
            return;
        }
        
        MoveVector.x = 0;
    }
    
    public void OnRight_Down(bool look = false)
    {
        if (look)
        {
            LookVector.x = 1;
            return;
        }
        
        MoveVector.x = 1;
    }
}
