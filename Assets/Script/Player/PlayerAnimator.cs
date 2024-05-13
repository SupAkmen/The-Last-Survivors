using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator anim;
    PlayerMovement playerController;
    SpriteRenderer spriteRenderer;


    private void Start()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(playerController.moveDir.x !=0 || playerController.moveDir.y !=0)
        {
            anim.SetBool("Move", true);
        }    
        else
        {
            anim.SetBool("Move", false);
         }

        SpriteDirectionChecker();
    }

    void SpriteDirectionChecker()
    {
        if(playerController.lastHorizontal <0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }    


}
