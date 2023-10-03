using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]  // So we can see items of this type in the Unity Editor
    public struct KitchenObjectSO_GameObject{
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectsList;

    private void Start(){
        plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnIngredientAdded;
        foreach(KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectsList){
            kitchenObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObjectOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e){
        foreach(KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectsList){
            if (kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO){
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }

    }
}
