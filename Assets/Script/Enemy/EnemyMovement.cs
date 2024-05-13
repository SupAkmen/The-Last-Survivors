using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;

    Vector2 knockbackVelocity;
    float knockbackDuration;

    bool facingLeft = false; // Biến để kiểm tra hướng của enemy

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    void Update()
    {
        if (knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            // Xác định hướng của player so với enemy
            if (player.position.x < transform.position.x && !facingLeft)
            {
                Flip(); // Nếu player ở bên trái enemy và enemy không đang nhìn về bên trái, thì quay đầu về bên trái
            }
            else if (player.position.x > transform.position.x && facingLeft)
            {
                Flip(); // Nếu player ở bên phải enemy và enemy đang nhìn về bên trái, thì quay đầu về bên phải
            }

            transform.position = Vector2.MoveTowards(transform.position, player.position, enemy.currentMoveSpeed * Time.deltaTime);
        }
    }

    public void KnockBack(Vector2 velocity, float duration)
    {
        if (knockbackDuration > 0) return;

        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }

    // Hàm để quay đầu của enemy
    void Flip()
    {
        facingLeft = !facingLeft; // Đảo ngược trạng thái của facingLeft

        Vector3 scale = transform.localScale;
        scale.x *= -1; // Đảo ngược hướng của scale theo trục x
        transform.localScale = scale;
    }
}
