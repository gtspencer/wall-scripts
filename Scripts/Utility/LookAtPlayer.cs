using StarterAssets;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class LookAtPlayer : MonoBehaviour
{
    private Transform player; // Assign this in the inspector
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = FirstPersonController.Instance.PlayerCamera.transform;
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator && player)
        {
            animator.SetLookAtWeight(1.0f); // Full weight for head rotation
            animator.SetLookAtPosition(player.position);
        }
    }
}