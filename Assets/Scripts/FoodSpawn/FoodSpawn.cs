using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawn : MonoBehaviour
{
    public List<Transform> goalpoints;            // Destination
    public DragonFly foodPrefab;                    // Prefab to spawn
    public SoundRandomizer deathSounds;

    public float DragonFlySpawnFrequencyBase = 3f;   // Base spawn interval (seconds)
    public float dragonFlyBaseSpeed = 3f;            // Base speed

    public float humanSpawnRandomness = 0.5f;    // % variation (0.5 = 50%)
    public float humanSpeedRandomness = 0.2f;    // % variation (0.5 = 50%)

    public FoodSpawnerAnchor currentAnchor; // Current target anchor
    public float speed = 3f;

    private float nextSpawnTime;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnDragonfly();
            ScheduleNextSpawn();
        }

        if (currentAnchor == null)
            return;

        // Move toward the anchor
        Vector3 dir = (currentAnchor.transform.position - transform.position);
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            // Arrived at anchor
            transform.position = currentAnchor.transform.position;

            // Move to next anchor
            if (currentAnchor.nextGoalPoint != null)
            {
                currentAnchor = currentAnchor.nextGoalPoint;
            }
            else
            {
                // Reached the end, optionally destroy
                Destroy(gameObject);
            }
        }
        else
        {
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }
    }

    private void SpawnDragonfly()
    {
        DragonFly newFood = Instantiate(foodPrefab, transform.position, Quaternion.identity);



        // Pick a random goalpoint from the list
        if (goalpoints != null && goalpoints.Count > 0)
        {
            int randomIndex = Random.Range(0, goalpoints.Count);
            newFood.goalPoint = goalpoints[randomIndex];
        }


        // Randomize speed based on base  randomness
        float speedFactor = Random.Range(1f - humanSpeedRandomness, 1f + humanSpeedRandomness);
        newFood.speed = dragonFlyBaseSpeed * speedFactor;
        newFood.deathSounds = deathSounds;
    }

    private void ScheduleNextSpawn()
    {
        float spawnFactor = Random.Range(1f - humanSpawnRandomness, 1f + humanSpawnRandomness);
        nextSpawnTime = Time.time + DragonFlySpawnFrequencyBase * spawnFactor;
    }
}