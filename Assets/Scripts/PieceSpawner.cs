using UnityEngine;
using System.Collections.Generic;

public class PieceSpawner : MonoBehaviour
{
    public GameObject piecePrefab;
    public GameObject spawnablecanvas;
    public List<Sprite> spriteList;
    

    void Start()
    {
        spawnPieces();
    }


    private void spawnPieces(){

        foreach (Sprite s in spriteList){
        SpriteRenderer bgRenderer = spawnablecanvas.GetComponent<SpriteRenderer>();
        float randomX = Random.Range(bgRenderer.bounds.min.x, bgRenderer.bounds.max.x);
        float randomY = Random.Range(bgRenderer.bounds.min.y, bgRenderer.bounds.max.y);
        Instantiate(piecePrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
        }
         
    }
}