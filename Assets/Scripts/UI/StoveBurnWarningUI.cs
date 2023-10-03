using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField]  private StoveCounter stoveCounter;

    private void Start(){
        stoveCounter.OnProgressChanged += StoveCounterOnProgressChanged;
        Hide();
    }

    private void StoveCounterOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e){
        float burnShowProgressAmount = 0.5f;

        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        gameObject.SetActive(show);
        /*
        This just adds a function call with an extra decision which is completely unnecessary
        - although it is consistent with the rest of the scripts using Show/Hide methods...
        if (show){
            Show();
        }
        else{
            Hide();
        }
  
            */
    }

    private void Show(){
        gameObject.SetActive(true);
    }

    private void Hide(){
        gameObject.SetActive(false);
    }
}
