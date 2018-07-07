using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;
public class HidingSpace : MonoBehaviour, iInteractable
{

    CanvasGroup ourCanvasGroup;

    Image playerBreathImage;
    AudioSource audioSource;

    AudioClip hyperventilationSound;

    float hyperventilationDuration = 4.0f;
    public Room parentRoom;

    public LayerMask defaultLayer;

    public LayerMask hidingLayer;

    public Transform hidingTransform;

    public Transform unhiddenSpace;
    bool alreadyHiding = false;
    public static event Action<MonoBehaviour> PlayerHiding;

    bool orbInHidingPlace;
    bool hidingSpace;

    public List<Sprite> sprites = new List<Sprite>();

    public enum SpriteVariations
    {
        Wood
    }
    public SpriteVariations ourSpriteVariation;

    SpriteRenderer spriteRenderer;
    GenerateNewBounds boundsGenerator;

    public static event Action BreathedTooLoud;

    public void BreathedTooLoudWrapper(){
        if(BreathedTooLoud != null){
            BreathedTooLoud();
        }
    }
    void Awake()
    {
        boundsGenerator = GetComponent<GenerateNewBounds>();
        parentRoom.EnemyEnteredAdjacentRoom += HyperventilationHandlerWrapper; 
        parentRoom.EnemyExitedAdjacentRoom += StopHyperventilating;
        //TODO: put the above back in after testing
        spriteRenderer = GetComponent<SpriteRenderer>();
        // for(int i = 0; i < sprites.Count; i++){
        //     if((int)ourSpriteVariation == i){
        //         spriteRenderer.sprite = sprites[i];
        //     }
        // }
        if (spriteRenderer.sprite != null)
        {
            gameObject.AddComponent<PolygonCollider2D>();
            // boundsGenerator.GenerateNewColliderSize();
        }

    }
    public void PlayerHidingWrapper()
    {
        alreadyHiding = true;
        GameHandler.playerGO.transform.position = hidingTransform.position;
        GameHandler.playerGO.layer = LayerMask.NameToLayer("HidingSpace");//LayerMask.(int)hidingLayer;
        if (PlayerHiding != null)
        {
            PlayerHiding(this);
        }
    }

    void StopHyperventilating(Room room){
        enemyNearby = false;
    }
    bool enemyNearby;
    int maxHyperventilationInterval;
    int minHyperventilationInterval;

    void HyperventilationHandlerWrapper(Room room){
        enemyNearby = true;
        StartCoroutine(HyperventilationHandler());
    }
    public IEnumerator HyperventilationHandler()
    {

        int hyperventilationInterval = 0;
        hyperventilationInterval = UnityEngine.Random.Range(minHyperventilationInterval, maxHyperventilationInterval);
        yield return new WaitForSeconds(hyperventilationInterval);
        while (alreadyHiding)
        {
            if(enemyNearby == false){
                break;
            }
            audioSource.clip = hyperventilationSound;
            audioSource.Play();
            audioSource.volume = 0.3f;
            audioSource.DOFade(1.0f, hyperventilationDuration);
            audioSource.DOPitch(2.0f, hyperventilationDuration);
            yield return StartCoroutine(PromptCalm());
            if(hyperventilationStrike == 2){
                break;
            }
            //audioSource.PlayOneShot(hyperventilationSound);
            hyperventilationInterval = UnityEngine.Random.Range(minHyperventilationInterval, maxHyperventilationInterval);
            yield return new WaitForSeconds(hyperventilationInterval);
        }
        if(hyperventilationStrike == 2){
              BreathedTooLoudWrapper();
              hyperventilationStrike = 0;
        }
    }

    
    

    List<KeyCode> ourKeyCodes = new List<KeyCode>();
    List<KeyCode> falseKeyCodes = new List<KeyCode>();

    bool CheckKeyCode(KeyCode codePressed){
        bool correctCode = true;
        if(falseKeyCodes.Contains(codePressed)){
            //if the keycode is a member of the false keycodes, meaning it was the wrong key
            correctCode = false;
        }
        return correctCode;
    } 

    bool waitingForPrompt;
    KeyCode lastHitKey;
    void OnGui()
    {
        if (waitingForPrompt)
        {
            if (Input.anyKeyDown)
            {
                lastHitKey = Event.current.keyCode;
            }
        }
    }

    KeyCode GrabKeyCodes(){
        int index = UnityEngine.Random.Range(0, ourKeyCodes.Count);
        KeyCode chosenKeyCode = ourKeyCodes[index];
        foreach(KeyCode code in ourKeyCodes){
            if(code == chosenKeyCode){
                continue;
            }
            falseKeyCodes.Add(code);
        }
        return chosenKeyCode;
    }
    public IEnumerator PromptCalm(){
        waitingForPrompt = true;
        float startTime = Time.time;
        float hitDurationWindow = 0.5f;
        bool hitSuccess = false;
        KeyCode ourKeyCode = GrabKeyCodes();
        while (Time.time < startTime + hitDurationWindow)
        {
            playerBreathImage.fillAmount -= Time.deltaTime / hitDurationWindow;
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(ourKeyCode))
                {
                    playerBreathImage.color = Color.cyan;
                    hitSuccess = true;
                    break;
                }
                if (!CheckKeyCode(lastHitKey))
                {

                    playerBreathImage.color = Color.red;
                    break;
                }
               
            }
            yield return null;
        }
        waitingForPrompt = false;
        if(!hitSuccess){ 
            hyperventilationStrike++;
        }
    }

    int hyperventilationStrike = 0;


    public static event Action<MonoBehaviour> PlayerNoLongerHiding;
    public void PlayerStoppedHidingWrapper()
    {
        alreadyHiding = false;
        GameHandler.playerGO.transform.position = unhiddenSpace.position;
        GameHandler.playerGO.layer = GameHandler.defaultPlayerLayer;
        if (PlayerNoLongerHiding != null)
        {
            PlayerNoLongerHiding(this);
        }

    }

    public void OnHoverMe(Player player)
    {
        Debug.Log("Press E to hide");
    }

    public void OnStopHoverMe(Player player)
    {

    }

    public void OnInteractWithMe(Player player)
    {
        if (!alreadyHiding)
        {
            PlayerHidingWrapper();
        }
        else
        {
            PlayerStoppedHidingWrapper();
        }

    }

    void HidePlayer(Player player)
    {
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
