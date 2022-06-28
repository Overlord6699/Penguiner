using System;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public void SetSpeed(float speed)
    {
        anim?.SetFloat("Speed", speed);
    }

    public void TriggerFall()
    {
        anim?.SetTrigger("Fall");
    }

    public void TriggerJump()
    {
        anim?.SetTrigger("Jump");
    }

    public void TriggerSlide()
    {
        anim?.SetTrigger("Slide");
    }

    public void SetGrounded(bool isGrounded)
    {
        anim?.SetBool("IsGrounded", isGrounded);
    }

    public void TriggerDeath()
    {
        anim?.SetTrigger("Death");
    }

    public void StartIdleAnimation()
    {
        anim?.SetTrigger("Idle");
    }

    public void TriggerRun()
    {
        anim?.SetTrigger("Running");
    }

    public void TriggerRespawn()
    {
        anim?.SetTrigger("Respawn");
    }
}
