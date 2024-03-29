using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{


    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;


    private void Awake(){
        resumeButton.onClick.AddListener(() => {
            // this is a lambda expression used instead of creating a function
            // Unpause the game
            
            KitchenGameManager.Instance.TogglePauseGame();
        });

        optionsButton.onClick.AddListener(() => {
            // this is a lambda expression used instead of creating a function
            // Hide this UI
            Hide();
            // Show the Options UI
            OptionsUI.Instance.Show(Show);
        });

        mainMenuButton.onClick.AddListener(() => {
            // this is a lambda expression used instead of creating a function
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
    private void Start()
    {
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManagerOnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManagerOnGameUnpaused;
        Hide();
    }

    private void KitchenGameManagerOnGamePaused(object sender, EventArgs e){
        Show();
        resumeButton.Select();
    }

    private void KitchenGameManagerOnGameUnpaused(object sender, EventArgs e){
        Hide();
    }
    private void Show(){
        gameObject.SetActive(true);
    }

    private void Hide(){
        gameObject.SetActive(false);
    }
    
}
