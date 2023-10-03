using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start(){
        plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnIngredientAdded;
    }

    private void PlateKitchenObjectOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e){
       UpdateVisual();
    }

    private void UpdateVisual(){
        // Destroy existing icons before adding new ones
        foreach(Transform child in transform){
            if (child == iconTemplate){
                // Don't delete the template
                continue;
            }
            Destroy(child.gameObject);
        }
        // check to see what's on the plate and add icons on the UI
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()){
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.GetComponent<PlateSingleIconUI>().SetKitchenObjectSO(kitchenObjectSO);
            iconTransform.gameObject.SetActive(true);
        }
    }
}
