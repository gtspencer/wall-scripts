using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInactiveOnStart : MonoBehaviour
{
    void Start()
    {
        this.gameObject.SetActive(false);
    }
}
