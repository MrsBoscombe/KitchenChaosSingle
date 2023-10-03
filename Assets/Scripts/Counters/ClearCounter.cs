using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{    
    public override void Interact(Player player){
        // Only spawn a new kitchen object if there isn't one already on the counter
        if (!HasKitchenObject()){
            // There is no object on the counter
            if (player.HasKitchenObject()){
                // Player is carrying something - drop the object on the counter
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else{
                // Player is not carrying anything
            }
           
        }
        else{
            // There is a kitchen object here
            if (player.HasKitchenObject()){
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)){
                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
                        GetKitchenObject().DestroySelf();
                    }
                }
                else{
                    // Player is not carrying a plate, but something else
                     if (GetKitchenObject().TryGetPlate(out plateKitchenObject)){
                        // There is a plate on the counter
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())){
                            // Try to add what the player is holding to the plate
                            player.GetKitchenObject().DestroySelf();
                        }
                     }

                }
            }
            else{
                // Player is not carrying anything - Give the object to the player
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    
}
