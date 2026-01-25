using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.UIElements;
using DG.Tweening;
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
        Timer.OnTimerEnd += spawnPieces;
        PuzzleController.OnLeavePuzzle += HandlePuzzleEnd; 
    }

    private void HandlePuzzleEnd(bool success)
    {
        if (success)
        {
            spawnPieces();
        }
    }

    private void OnDisable()
    {
        PuzzlePieceDrop.OnPieceCollected -= HandlePieceCollected;
        Timer.OnTimerEnd -= spawnPieces;
        PuzzleController.OnLeavePuzzle -= HandlePuzzleEnd;
    }

    private void spawnPieces()
    {
        StartCoroutine(DelaySpawning());
    }

    IEnumerator DelaySpawning()
    {
        piecesRemaining = spriteList.Count;
        SpriteRenderer bgRenderer = spawnablecanvas.GetComponent<SpriteRenderer>();
        for (int i = 0; i < spriteList.Count; i++)
        {
            float randomX = UnityEngine.Random.Range(bgRenderer.bounds.min.x, bgRenderer.bounds.max.x);
            float randomY = UnityEngine.Random.Range(bgRenderer.bounds.min.y, bgRenderer.bounds.max.y);

            GameObject newPiece = Instantiate(piecePrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
            newPiece.GetComponent<SpriteRenderer>().sprite = spriteList[i];
            newPiece.GetComponent<SpriteRenderer>().color = Helper.TransparentColor(newPiece.GetComponent<SpriteRenderer>().color);
            newPiece.GetComponent<SpriteRenderer>().DOFade(1f, 0.5f);
            yield return new WaitForSeconds(6f);
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