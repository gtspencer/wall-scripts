using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;

    // [SerializeField] private MMF_Player screenshakeFeedback;

    private void Awake()
    {
        if (Instance != null) Destroy(this.gameObject);

        Instance = this;
    }

    public void ShakeScreen()
    {
        // screenshakeFeedback.PlayFeedbacks();
    }
}
