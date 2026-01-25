using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public static event Action<InteractableObject> RegisterObject;
    public static event Action<InteractableObject, bool> CloseToObject; // object, isClose
    [SerializeField] private Sprite destroyedSprite;
    private bool isActive = false;
    private bool isPlayerClose = false;
    private bool isShaking = false;
    private float speed = 8.0f; //how fast it shakes
    private float amount = 0.05f; //how much it shakes
    Vector2 startingPos;
    private EventInstance knockSfx;
    private EventInstance dogLeave;
    // maybe an array, objecthandler or w/e
    private GameObject toolTip;
    private TextMeshPro toolTipText;
    private bool puzzleUnlocked = false;
    private float nextKnockTime;
    private void Start()
    {
        toolTip = transform.GetChild(0).gameObject;
        toolTip.SetActive(false);
        toolTipText = toolTip.GetComponentInChildren<TextMeshPro>();
        RegisterObject?.Invoke(this);
        startingPos.x = transform.position.x;
        startingPos.y = transform.position.y;
        knockSfx = AudioManager.Instance.CreateInstance(FMODEvents.Instance.NockNock);
        knockSfx.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        dogLeave = AudioManager.Instance.CreateInstance(FMODEvents.Instance.DogDeath);
        dogLeave.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
    }

    private void OnEnable()
    {
        PieceSpawner.OnAllPiecesCollected += SetPuzzleAvailable;
        PuzzleController.OnLeavePuzzle += OnPuzzleFinish;
        Timer.OnTimerEnd += OnPuzzleFail;
    }

    private void OnPuzzleFinish(bool success)
    {
        if (success)
        {
            puzzleUnlocked = false;
        }
    }

    private void OnPuzzleFail()
    {
        puzzleUnlocked = false;
    }

    private void SetPuzzleAvailable()
    {
        puzzleUnlocked = true;
        toolTipText.text = "PRESS E";
    }

    public void ActivateItem()
    {
        // START PLAYING KNOCKING/WHATEVER SFX here
        isActive = true;
        GetComponent<SpriteRenderer>().color = Color.yellow;
        if (isPlayerClose)
        {
            Debug.Log("was close");
            CloseToObject.Invoke(this, true);
            toolTip.SetActive(true);
        }
        isShaking = true;

    }

    private void OnDisable()
    {
        PieceSpawner.OnAllPiecesCollected -= SetPuzzleAvailable;
        PuzzleController.OnLeavePuzzle -= OnPuzzleFinish;
        Timer.OnTimerEnd -= OnPuzzleFail;
    }
    private void Update()
    {
        if (isShaking)
        {
            transform.position = new Vector2((startingPos.x + Mathf.Sin(Time.time * speed) * amount), (startingPos.y + (Mathf.Sin(Time.time * speed) * amount)));
            if (!AudioManager.IsPlaying(knockSfx) && Time.time > nextKnockTime)
            {
                knockSfx.start();
                nextKnockTime = Time.time + UnityEngine.Random.Range(3f, 10f);
            }
        }
    }
    public void DeactivateItem(bool success)
    {
        isActive = false;
        toolTip.SetActive(false);
        // stop SFX as well
        isShaking = false;
        transform.position = startingPos;
        knockSfx.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        if (success)
        {
            // all good we are safe, stop shaking
        }
        else
        {
            // destroyed, swap sprite etc etc
            GetComponent<SpriteRenderer>().sprite = destroyedSprite;
            StartCoroutine(DelayDogLeave());
        }

    }

    private IEnumerator DelayDogLeave()
    {
        yield return new WaitForSeconds(0.4f);
        dogLeave.start();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.name.Contains("Player")) return;
        isPlayerClose = true;
        if (isActive)
        {
            CloseToObject.Invoke(this, true);
            if (puzzleUnlocked)
            {
                toolTipText.text = "PRESS E";
            }
            else
            {
                toolTipText.text = "GATHER THE PIECES";
            }
            toolTip.SetActive(true);
        }
    }

    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        if (!collision.name.Contains("Player")) return;
        isPlayerClose = false;
        if (isActive)
        {
            toolTip.SetActive(false);
            CloseToObject.Invoke(this, false);
        }
    }
}
