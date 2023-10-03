using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    // One event for all of the Counters when an object is placed
    public static event EventHandler OnAnyObjectPlacedHere;

    public static void ResetStaticData(){
        OnAnyObjectPlacedHere = null;
    }

    // Kitchen object that is on the top of the counter - can be any object as player can move objects
    // from one counter to another
    private KitchenObject kitchenObject;

    [SerializeField] private Transform counterTopPoint;
 
    public virtual void Interact(Player player){
        //Debug.LogError("BaseCounter.Interact!!!");
        // keep accidentally hitting keys at the wrong time!
        Debug.Log("BaseCounter.Interact!!!");

    }

    public virtual void InteractAlternate(Player player){
        //Debug.LogError("BaseCounter.InteractAlternate!!!");
        // keep accidentally hitting keys at the wrong time!
        Debug.Log("BaseCounter.InteractAlternate!!!");
        
    }


    public Transform GetKitchenObjectFollowTransform(){
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject){
        this.kitchenObject = kitchenObject;
        if (this.kitchenObject != null){
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject(){
        return kitchenObject;
    }

    public void ClearKitchenObject(){
        kitchenObject = null;
    }

    public bool HasKitchenObject(){
        return kitchenObject != null;
    }

    public Transform GetCounterTopPoint(){
        return counterTopPoint;
    }

    public void SetCounterTopPoint(Transform counterTopPoint){
        this.counterTopPoint = counterTopPoint;
    }
}
