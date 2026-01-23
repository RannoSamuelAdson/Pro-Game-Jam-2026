using UnityEngine;

public class DragonFly : MonoBehaviour
{
    public Transform goalPoint;
    public float speed;
    private bool facingRight = true; // Keep track of facing direction
    public GameObject sprite;
    public Rigidbody rb;
    public SoundRandomizer deathSounds;
    public float nutritionValue = 7;
    public GameObject deathEffectPrefab;
    void Update()
    {
        if (goalPoint == null)
            return;

        // Move toward the anchor
        Vector3 dir = (goalPoint.transform.position - transform.position);
        float distanceThisFrame = speed * Time.deltaTime;

        // Flip sprite based on horizontal direction
        HandleSpriteFlip(dir);

        if (dir.magnitude <= distanceThisFrame)
        {
            Destroy(gameObject);

        }
        else
        {
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }
    }
    void HandleSpriteFlip(Vector3 moveDirection)
    {
        // Flip sprite based on x component of the movement direction
        if (moveDirection.x > 0 && !facingRight)
        {
            facingRight = true;
            sprite.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (moveDirection.x < 0 && facingRight)
        {
            facingRight = false;
            sprite.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);

        bool hitPlayer = other.GetComponentInChildren<Player>() != null
                         || other.GetComponentInParent<Player>() != null;

        bool hitCrow = other.GetComponentInChildren<Crow>() != null;

        if (!hitPlayer && !hitCrow) return;

        // If player hit it ? add nutrition
        if (hitPlayer)
        {
            deathSounds.PlayRandomSound();

            // Find hunger bar & add nutrition
            HungerBar hb = FindFirstObjectByType<HungerBar>();
            if (hb != null)
            {
                hb.AddNutrition(nutritionValue);
            }
            else Debug.Log("No hunger bar found");
        }

        // Spawn effect
        Instantiate(deathEffectPrefab, transform.position, transform.rotation);

        // Remove dragonfly
        Destroy(gameObject);
    }
}
