using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] Movement playerMovement;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Start()
    {
        playerMovement.OnJumpImpact += PlayerMovement_OnJumpImpact;
    }

    private void OnDisable()
    {
        playerMovement.OnJumpImpact -= PlayerMovement_OnJumpImpact;
    }

    private void PlayerMovement_OnJumpImpact(object sender, System.EventArgs e)
    {
        Landing();
    }

    public void Landing()
    {
        animator.SetTrigger("JumpLanding");
    }


}
