using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake(){
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start(){
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManagerOnRecipeSpawned;
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManagerOnRecipeCompleted;

        UpdateVisual();
    }

    private void UpdateVisual(){
        foreach(Transform child in container){
            if (child == recipeTemplate){
                continue;
            }
            Destroy(child.gameObject);
        }

        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList()){
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true); 
            recipeTransform.gameObject.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }

    private void DeliveryManagerOnRecipeSpawned(object sender, EventArgs e){

        UpdateVisual();
    }

    private void DeliveryManagerOnRecipeCompleted(object sender, EventArgs e){

        UpdateVisual();
    }
}
