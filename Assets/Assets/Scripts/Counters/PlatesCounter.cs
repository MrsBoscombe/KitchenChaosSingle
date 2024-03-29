using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4.0f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    private void Start() {
        spawnPlateTimer = 0f;
        platesSpawnedAmount = 0;
    }

    private void Update(){
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax){
            // reset the timer
            spawnPlateTimer = 0f;
            if (KitchenGameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax){
                platesSpawnedAmount++;
                // spawn a plate visual
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
            
        }

    }

    public override void Interact(Player player){
        if (!player.HasKitchenObject()){
            // player is empty handed
            if (platesSpawnedAmount > 0){
                // There's at least one plate here
                platesSpawnedAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                //KitchenObject kitchenObject = KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                //kitchenObject.gameObject.SetActive(true); // to force plate to be active - somehow after starting the deliverycounter, the plate wouldn't appear in the player's hand!!!!
                // note to above - moved SetActive to the SpawnKitchenObject method
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
        //Debug.Log("PlatesCounter Interact called with " + platesSpawnedAmount + " plates available");
    }
}
