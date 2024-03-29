using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    private void Update(){

        // Not really needed on such a simple scene. Only to showcase
        // what you would do if you need a loading scene...
        if (isFirstUpdate){
            isFirstUpdate = false;

            Loader.LoaderCallback();
        }
    }
}
