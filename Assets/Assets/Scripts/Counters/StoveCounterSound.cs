using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private StoveCounter stoveCounter;
    private float warningSoundTimer;
    private float warningSoundTimerMax = 0.2f;
    private bool playWarningSound = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }

    private void Start(){
        stoveCounter.OnStateChanged += StoveCounterOnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounterOnProgressChanged;
        warningSoundTimer = warningSoundTimerMax;
    }

    public void StoveCounterOnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e){
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound){
            audioSource.Play();
        }
        else{
            audioSource.Pause();
        }

    }

    public void StoveCounterOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e){
        
        float burnShowProgressAmount = 0.5f;
        playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
    }

    private void Update(){
        if (playWarningSound){
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer <= 0f){
                warningSoundTimer = warningSoundTimerMax;
                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }
    
}
