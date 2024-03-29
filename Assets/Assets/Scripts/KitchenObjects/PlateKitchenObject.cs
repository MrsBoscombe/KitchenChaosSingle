using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs{
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Start(){
        // CodeMonkey does this in OnAwake...
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO){
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)){
            // not a valid ingredient
            //Debug.Log(kitchenObjectSO + "invalid");
            return false;
        }
        // Valid ingredient - but do we already have one on our plate?
        if (kitchenObjectSOList.Contains(kitchenObjectSO)){
            //Debug.Log(kitchenObjectSO + "duplicate");
            return false;
        }
        else{
            kitchenObjectSOList.Add(kitchenObjectSO);
            //Debug.Log(kitchenObjectSO + "added successfully");
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs{
                kitchenObjectSO = kitchenObjectSO
            });
            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList(){
        return kitchenObjectSOList;
    }
}
