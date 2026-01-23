using UnityEngine;

public class Crow : MonoBehaviour
{
    public DragonFly target;

    [Header("Targeting")]
    public float retargetInterval = 0.25f;
    private float nextRetargetTime;

    [Header("Movement")]
    public float chaseForce = 20f;   // how strongly the crow accelerates
    public float maxSpeed = 10f;     // crow speed cap
    public float verticalAssist = 0.1f; // helps fight gravity

    private Rigidbody rb;

    public GameObject crowSprite;
    public GameObject beak;
    private bool facingRight = true; // Keep track of facing direction

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        FindNewTarget();
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            if (Time.time >= nextRetargetTime)
                FindNewTarget();
            return;
        }

        ChaseTarget();
        HandleSpriteFlip();
    }

    private void ChaseTarget()
    {
        Vector3 dir = (target.transform.position - beak.transform.position).normalized;

        // Apply chase force
        rb.AddForce(new Vector3(dir.x, dir.y + verticalAssist, dir.z) * chaseForce);

        // Limit speed
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }

    private void FindNewTarget()
    {
        nextRetargetTime = Time.time + retargetInterval;

        var allTargets = FindObjectsByType<DragonFly>(FindObjectsSortMode.None);
        if (allTargets.Length == 0)
        {
            target = null;
            return;
        }

        // Track closest and second-closest
        DragonFly closest = null;
        DragonFly secondClosest = null;

        float closestDist = float.MaxValue;
        float secondDist = float.MaxValue;

        foreach (var df in allTargets)
        {
            float dist = (df.transform.position - beak.transform.position).sqrMagnitude;

            if (dist < closestDist)
            {
                // shift the old closest to second
                secondClosest = closest;
                secondDist = closestDist;

                closest = df;
                closestDist = dist;
            }
            else if (dist < secondDist)
            {
                secondClosest = df;
                secondDist = dist;
            }
        }

        // Decide which to pick
        if (secondClosest != null && Random.value < 0.5f)
        {
            // 50% chance to pick second closest
            target = secondClosest;
        }
        else
        {
            // Otherwise pick closest
            target = closest;
        }
    }
    void HandleSpriteFlip()
    {
        float xVel = rb.linearVelocity.x;

        // Small dead zone to prevent jitter near zero velocity
        const float flipThreshold = 0.1f;

        if (xVel > flipThreshold && !facingRight)
        {
            facingRight = true;
            crowSprite.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            //Debug.Log("facing right");
        }
        else if (xVel < -flipThreshold && facingRight)
        {
            facingRight = false;
            crowSprite.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            //Debug.Log("facing left");
        }
    }
}
