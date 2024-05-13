using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManagerX : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    public LayerMask terrainMask;
    //PlayerMovement playerController;
    public GameObject currentChunk;
    public Vector3 playerLastPosition;

    [Header("Optimazation")]
    public List<GameObject> spawnedChunks;
    GameObject lastestChunk;
    public float maxOpDis;
    float opDist;
    float optimizerCooldown;
    public float optimizerCoolDownDur;
    void Start()
    {
        // playerController = FindObjectOfType<PlayerMovement>();
        playerLastPosition = player.transform.position;
    }

    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    void ChunkChecker()
    {
        if (!currentChunk)
        {
            return;
        }
        
        Vector3 moveDir = player.transform.position - playerLastPosition;
        playerLastPosition = player.transform.position;

        string directionName = GetDirectionName(moveDir);

        CheckAndSpawnTrunk(directionName);

        // Sinh ra dia hinh khi cac huong di chuyen cheo se sinh ra cac dia hinh xung quanh
        if (directionName.Contains("Right"))
        {
            CheckAndSpawnTrunk("Right");
        }
        if (directionName.Contains("Left"))
        {
            CheckAndSpawnTrunk("Left");
        }


        #region Use player's movement to load map
        //  if (playerController.moveDir.x > 0 && playerController.moveDir.y == 0)
        //  {
        //      if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
        //      {
        //          noTerrainPosition = currentChunk.transform.Find("Right").position;  //Right
        //          SpawnChunk();
        //      }
        //  }
        //  else if (playerController.moveDir.x < 0 && playerController.moveDir.y == 0)
        //  {
        //      if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
        //      {
        //          noTerrainPosition = currentChunk.transform.Find("Left").position;    //Left
        //          SpawnChunk();
        //      }
        //  }
        //  else if (playerController.moveDir.y > 0 && playerController.moveDir.x == 0)
        //  {
        //      if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
        //      {
        //          noTerrainPosition = currentChunk.transform.Find("Up").position; //Up
        //          SpawnChunk();
        //      }
        //  }
        //  else if (playerController.moveDir.y < 0 && playerController.moveDir.x == 0)
        //  {
        //      if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
        //      {
        //          noTerrainPosition = currentChunk.transform.Find("Down").position;    //Down
        //          SpawnChunk();
        //      }
        //  }
        //  else if (playerController.moveDir.x > 0 && playerController.moveDir.y > 0)
        //  {
        //      if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Up").position, checkerRadius, terrainMask))
        //      {
        //          noTerrainPosition = currentChunk.transform.Find("Right Up").position;   //Right up
        //          SpawnChunk();
        //      }
        //  }
        //  else if (playerController.moveDir.x > 0 && playerController.moveDir.y < 0)
        //  {
        //      if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Down").position, checkerRadius, terrainMask))
        //      {
        //          noTerrainPosition = currentChunk.transform.Find("Right Down").position;  //Right down
        //          SpawnChunk();
        //      }
        //  }
        //  else if (playerController.moveDir.x < 0 && playerController.moveDir.y > 0)
        //  {
        //      if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Up").position, checkerRadius, terrainMask))
        //      {
        //          noTerrainPosition = currentChunk.transform.Find("Left Up").position;  //Left up
        //          SpawnChunk();
        //      }
        //  }
        //  else if (playerController.moveDir.x < 0 && playerController.moveDir.y < 0)
        //  {
        //      if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Down").position, checkerRadius, terrainMask))
        //      {
        //          noTerrainPosition = currentChunk.transform.Find("Left Down").position; //Left down
        //          SpawnChunk();
        //      }
        //  }

        #endregion
    }

    void CheckAndSpawnTrunk(string direction)
    {
        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(direction).position, checkerRadius, terrainMask))
        {
            SpawnChunk(currentChunk.transform.Find(direction).position);
        }
    }

    string GetDirectionName(Vector3 direction)
    {
        direction = direction.normalized;

        if (direction.x > 0.5f)
        {
            return "Right";
        }
        else if (direction.x < 0.5f)
        {
            return "Left";
        }
        else
        {
            return "Không xác định";
        }
    }

    void SpawnChunk(Vector3 spawnPosition)
    {
        int rand = Random.Range(0, terrainChunks.Count);
        lastestChunk = Instantiate(terrainChunks[rand], spawnPosition, Quaternion.identity);
        spawnedChunks.Add(lastestChunk);
    }

    void ChunkOptimizer()
    {
        optimizerCooldown -= Time.deltaTime;

        if (optimizerCooldown <= 0)
        {
            optimizerCooldown = optimizerCoolDownDur;
        }
        else
        {
            return;
        }
        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > maxOpDis)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
