using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EscapeBoxController : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private TextMeshProUGUI _wallText;

    private bool _doorClosed;
    
    public void OnPlayerEntered()
    {
        if (_doorClosed) return;

        _doorClosed = true;
        
        _door.Close();

        
    }
}
