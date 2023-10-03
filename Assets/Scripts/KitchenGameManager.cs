using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KitchenGameManager : MonoBehaviour
{

    public static KitchenGameManager Instance { get; private set;}

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    private enum State{
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }
    private State state;
    // private float waitingToStartTimer = 1.0f;
    private float countdownToStartTimer = 3.0f;
    private float gamePlayingTimer = 0f;  
    private float gamePlayingTimerMax = 300f;
    private bool isGamePaused = false;

    private void Awake(){
        state = State.WaitingToStart;
        Instance = this;
    }

    private void Start(){
        GameInput.Instance.OnPauseAction += GameInputOnPauseAction;
        GameInput.Instance.OnInteractAction += GameInputOnInteractAction;
    }

    private void Update(){
        switch (state){
            case State.WaitingToStart:
                
                
                /*waitingToStartTimer -= Time.deltaTime;

                if (waitingToStartTimer < 0f){
                    state = State.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                */
                break;

            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;

                if (countdownToStartTimer < 0f){
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;

                if (gamePlayingTimer < 0f){
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }

                break;

            case State.GameOver:

                break;
        }
        //Debug.Log(state);
    }
    public bool IsGamePlaying(){
        return state == State.GamePlaying;
    }
    
    public bool IsCountDownToStartActive(){
        return state == State.CountdownToStart;
    }

    public bool IsWaitingToStartActive(){
        return state == State.WaitingToStart;
    }

    public bool IsGameOver(){
        return state == State.GameOver;
    }

    public float GetCountdownToStartTimer(){
        return countdownToStartTimer;
    }

    public float GetGamePlayingTimerNormalized(){
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    public void GameInputOnPauseAction(object sender, EventArgs e){
        TogglePauseGame();
    }
    public void TogglePauseGame(){
        // CodeMonkey renamed this to something like TogglePause
        isGamePaused = !isGamePaused;

        if (isGamePaused){
            // First change Time.deltaTime to 0, so no time passes
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else{
            // set Time.deltaTime back to 1
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
       
    }

    private void GameInputOnInteractAction(object sender, EventArgs e){
        if (state == State.WaitingToStart){
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
