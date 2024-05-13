using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    public float minDistanceBetweenEnemies = 2f; // Khoảng cách tối thiểu giữa các enemy
    public float adjustSpeed = 3f; // Tốc độ điều chỉnh vị trí của enemy

    // current state
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistance = 20f;
    Transform player;

    [Header("Damage FeedBack")]
    public Color damageColor = new Color(1, 0, 0, 1); // mau khi gay sat thuong
    public float damageFlashDuration = 0.2f; // thoi gian
    public float deathFadeTime = 0.6f;
    Color originalColor;
    SpriteRenderer sr;
    EnemyMovement movement;

    AudioManager audioManager;
    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHeath;
        currentDamage = enemyData.Damage;
    }

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        player = FindObjectOfType<PlayerStats>().transform;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        movement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, minDistanceBetweenEnemies);

        foreach (Collider2D enemyCollider in nearbyEnemies)
        {
            if (enemyCollider.gameObject != gameObject)
            {
                float distance = Vector3.Distance(transform.position, enemyCollider.transform.position);
                if (distance < minDistanceBetweenEnemies)
                {
                    Vector3 direction = (transform.position - enemyCollider.transform.position).normalized;
                    transform.position += direction * Time.deltaTime * adjustSpeed;
                    break; // Thoát khỏi vòng lặp sau khi điều chỉnh vị trí
                }
            }
        }

        if (Vector2.Distance(transform.position, player.position) >= despawnDistance)
        {
            ReturnEnemy();
        }
    }
    public void TakeDamage(float dmg, Vector2 sourcePositiion, float knockbackForce = 5f, float knockbackDuration = 0.2f)
    {
        currentHealth -= dmg;

        StartCoroutine(DamageFlash());

        // create the popup text when the enemy takes damage
        if (dmg > 0)
        {
            GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform);
        }

        if (knockbackForce > 0)
        {
            // lay huong day lui
            Vector2 dir = (Vector2)transform.position - sourcePositiion;
            movement.KnockBack(dir.normalized * knockbackForce, knockbackDuration);
        }

        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    public void AddDoT(float damagePerSecond, float duration)
    {
        if (GetComponentInChildren<DamageOverTime>() != null) return;

        gameObject.AddComponent<DamageOverTime>().damagePerSecond = damagePerSecond;
        gameObject.AddComponent<DamageOverTime>().duration = duration;
    }


    // hieu ung nhap nhay khi gay sat thuong

    IEnumerator DamageFlash()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originalColor;
    }
    public void Kill()
    {
        StartCoroutine(KillFade());
    }

    IEnumerator KillFade()
    {
        // wait for single frame
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0;
        float origAlpha = sr.color.a;

        // this is a loop that fires every frames

        while (t < deathFadeTime)
        {
            yield return w;

            t += Time.deltaTime;

            // set color for this frame 
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (1 - t / deathFadeTime) * origAlpha);
        }

        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
            audioManager.PlaySFX(audioManager.takeDame);
        }
    }
    private void OnDestroy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        if (es != null)
            es.OnEnemyKilled();
    }


    void ReturnEnemy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        transform.position = player.position + es.relativeSpawnPoints[Random.Range(0, es.relativeSpawnPoints.Count)].position;
    }
}
