using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingUI : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";
    [SerializeField]  private StoveCounter stoveCounter;
    private Animator animator;

    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void Start(){
        stoveCounter.OnProgressChanged += StoveCounterOnProgressChanged;
        animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounterOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e){
        float burnShowProgressAmount = 0.5f;

        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        animator.SetBool(IS_FLASHING, show);
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

}
