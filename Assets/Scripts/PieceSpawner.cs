using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public GameObject piecePrefab;
    public GameObject spawnablecanvas;

    void Start()
    {
        SpriteRenderer bgRenderer = spawnablecanvas.GetComponent<SpriteRenderer>();

        float randomX = Random.Range(bgRenderer.bounds.min.x, bgRenderer.bounds.max.x);
        float randomY = Random.Range(bgRenderer.bounds.min.y, bgRenderer.bounds.max.y);
        float z = spawnablecanvas.transform.position.z - 1f;
        Instantiate(piecePrefab, new Vector3(randomX, randomY, z), Quaternion.identity);
    }
}