using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
        
    public const int DEFAULT_MOVESPEED = 5;
    [HideInInspector]
    public float lastHorizontal;
    [HideInInspector]
    public float lastVertical;
    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector]
    public Vector2 lastMovedVector;

 
    Rigidbody2D rb;
    PlayerStats player;

    
    void Start()
    {
        player = GetComponent<PlayerStats>();
        rb =  GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1, 0); // dat huong mat dinh la huong ben phai
    }

   
    void Update()
    {
        InputManager();
    }

    private void FixedUpdate()
    {
        Move();
    }



    void InputManager()
    {
        if(GameManager.instance.isGameOver) // neu gam ket thuc nguoi choi ko the dung bo dieu khien nguoi choi 
        {
            return;
        }
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        if(moveDir.x !=0)
        {
            lastHorizontal = moveDir.x;
            lastMovedVector = new Vector2(lastHorizontal, 0f);
        } 
        if(moveDir.y !=0)
        {
            lastVertical = moveDir.y;
            lastMovedVector = new Vector2(0f,lastVertical);
        }
        if(moveDir.x !=0 && moveDir.y !=0)
        {
            lastMovedVector = new Vector2(lastHorizontal, lastVertical);
        }
    }    



    void Move()
    {
        if (GameManager.instance.isGameOver) // neu gam ket thuc nguoi choi ko the dung bo dieu khien nguoi choi 
        {
            return;
        }
        rb.velocity = moveDir * DEFAULT_MOVESPEED * player.Stats.moveSpeed;
    }    
}
