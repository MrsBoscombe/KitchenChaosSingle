using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader{

    public enum Scene{
        MainMenuScene,
        LoadingScene,
        GameScene
    }

    private static Scene  targetScene;

    public static void Load(Scene targetScene){
        Loader.targetScene = targetScene;
        // Load loading scene first
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
        // Need to wait at least 1 frame to make this visible

        
    }

    public static void LoaderCallback(){
        SceneManager.LoadScene(targetScene.ToString());
    }

}
