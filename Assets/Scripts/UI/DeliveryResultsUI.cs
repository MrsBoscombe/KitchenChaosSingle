using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultsUI : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;

    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failedSprite;
    private const string DELIVERY_FAILED = "DELIVERY\nFAILED";
    private const string DELIVERY_SUCCESS = "DELIVERY\nSUCCESS";
    private const string POPUP = "Popup";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start(){
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManagerOnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManagerOnRecipeFailed;
        //gameObject.SetActive(false);
        Hide();
    }

    private void DeliveryManagerOnRecipeSuccess(object sender, EventArgs e){
        Show();
        //gameObject.SetActive(true);
        backgroundImage.color = successColor;
        iconImage.sprite = successSprite;
        messageText.text = DELIVERY_SUCCESS;
        animator.SetTrigger(POPUP);
        //Debug.Log("Succcess");
    }

    private void DeliveryManagerOnRecipeFailed(object sender, EventArgs e){
        Show();
        //gameObject.SetActive(true);
        backgroundImage.color = failedColor;
        iconImage.sprite = failedSprite;
        messageText.text = DELIVERY_FAILED;
        animator.SetTrigger(POPUP);
        //Debug.Log("Failed");
    }
    private void Show(){
        gameObject.SetActive(true);
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

}
