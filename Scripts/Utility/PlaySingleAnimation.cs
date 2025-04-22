using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlaySingleAnimation : MonoBehaviour
{
    private Animator _animator;
    private PlayableGraph playableGraph;
    [SerializeField] private AnimationClip animationClip;
    [SerializeField] private float _speedMultiplier = 1f;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        
        // Create a PlayableGraph
        playableGraph = PlayableGraph.Create("AnimationGraph");
        var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", _animator);
        var playableClip = AnimationClipPlayable.Create(playableGraph, animationClip);
        
        playableClip.SetSpeed(_speedMultiplier);

        // Connect and play
        playableOutput.SetSourcePlayable(playableClip);
        playableGraph.Play();
    }

    private void OnDisable()
    {
        playableGraph.Destroy();
    }
}
