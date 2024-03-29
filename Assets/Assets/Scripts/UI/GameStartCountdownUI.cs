using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Runtime.CompilerServices;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private Animator animator;
    private int previousCountdownNumber;
    private const string NUMBER_POPUP = "NumberPopup"; 

    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void Start(){
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManagerOnStateChanged;
        previousCountdownNumber = -1;
        Hide(); //  by default
    }

    private void KitchenGameManagerOnStateChanged(object sender, EventArgs e){
        if (KitchenGameManager.Instance.IsCountDownToStartActive()){
            Show();
        }
        else{
           Hide(); 
        }
    }

    private void Show(){
        gameObject.SetActive(true);
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();

        if (previousCountdownNumber != countdownNumber){
            // initiate the animation
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountdownSound();
        }
    }

}
