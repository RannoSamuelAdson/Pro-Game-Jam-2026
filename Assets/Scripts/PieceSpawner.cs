using UnityEngine;
using System.Collections.Generic;
using System;
public class PieceSpawner : MonoBehaviour
{
    public GameObject piecePrefab;
    public GameObject spawnablecanvas;
    public List<Sprite> spriteList;

    private int piecesRemaining;

    public static event Action<int> OnCountChanged; 
    public static event Action OnAllPiecesCollected;

    void Start()
    {
        spawnPieces();
    }

    private void OnEnable()
    {
        PuzzlePieceDrop.OnPieceCollected += HandlePieceCollected;
    }

    private void OnDisable()
    {
        PuzzlePieceDrop.OnPieceCollected -= HandlePieceCollected;
    }

    private void spawnPieces()
    {
        piecesRemaining = spriteList.Count;
        SpriteRenderer bgRenderer = spawnablecanvas.GetComponent<SpriteRenderer>();

        for (int i = 0; i < spriteList.Count; i++)
        {
            float randomX = UnityEngine.Random.Range(bgRenderer.bounds.min.x, bgRenderer.bounds.max.x);
            float randomY = UnityEngine.Random.Range(bgRenderer.bounds.min.y, bgRenderer.bounds.max.y);

            GameObject newPiece = Instantiate(piecePrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
            newPiece.GetComponent<SpriteRenderer>().sprite = spriteList[i];
        }
    }

    private void HandlePieceCollected()
    {
        piecesRemaining--;
        OnCountChanged?.Invoke(piecesRemaining);

        if (piecesRemaining <= 0)
        {
            Debug.Log("Got dem all");
            OnAllPiecesCollected?.Invoke();
        }
    }
}