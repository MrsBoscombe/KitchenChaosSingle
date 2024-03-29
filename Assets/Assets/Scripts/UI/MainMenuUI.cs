using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake(){
        playButton.onClick.AddListener(() => {
            // this is a lambda expression used instead of creating a function
            // Click
            //SceneManager.LoadScene(1);
            Loader.Load(Loader.Scene.GameScene);
        });

        quitButton.onClick.AddListener(() => {
            // this is a lambda expression used instead of creating a function
            // Click
            Application.Quit(); // This will not work when running game from editor, but will work when game is run from a build
        });

        Time.timeScale = 1f;    // Making sure that time is set correctly when coming here from the Pause Menu
    }
}
