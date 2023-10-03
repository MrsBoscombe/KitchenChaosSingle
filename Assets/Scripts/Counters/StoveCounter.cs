using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs: EventArgs{
        public State state;
    }
    public enum State{
        Idle,
        Frying,
        Fried,
        Burned
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private State state;    // current state of StoveTop
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private float burningTimer;


    private void Start(){
        state = State.Idle;
    }
    private void Update(){
        if (HasKitchenObject()){
            switch (state){
                case State.Idle:
                    // do nothing?
                    break;

                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax){
                        // Meat is fried
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state = State.Fried;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });
                        //Debug.Log("Object fried!");
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(fryingRecipeSO.output);
                        
                    }                
                    break;

                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });
                    
                    if (burningTimer > burningRecipeSO.burningTimerMax){
                        // Meat is fried
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;
                        //Debug.Log("Object burned!");
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });
                    }                
                    break;

                case State.Burned:
                    // and finally we've Burned the item(s), so reset the progress bar
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = 0f
                    });
                    break;
            }
            //Debug.Log(state);
    
        }
    }

    public override void Interact(Player player){
        // Only spawn a new kitchen object if there isn't one already on the counter
        if (!HasKitchenObject()){
            // There is no object on the counter
            if (player.HasKitchenObject()){
                // Validate that what the player has is "fryable"
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())){
                    // Player is carrying something - drop the object on the counter
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Frying;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                    });
                    fryingTimer = 0;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
                else{
                    // the player is carrying something that cannot be fried
                }
            }
            else{
                // Player is not carrying anything
            }
           
        }
        else{
            // See if the player already has an object
            if (player.HasKitchenObject()){
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)){
                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
                        GetKitchenObject().DestroySelf();
                    }

                    state = State.Idle;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = 0f
                    });
                }
            }
            else{
                // Player is not carrying anything - Give the object to the player
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = 0f
                });
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO){

        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null){
            return fryingRecipeSO.output;
        }
        else{
            return null;
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO){
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return (fryingRecipeSO != null);
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO){
        foreach(FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray){
            if (fryingRecipeSO.input == inputKitchenObjectSO){
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO){
        foreach(BurningRecipeSO burningRecipeSO in burningRecipeSOArray){
            if (burningRecipeSO.input == inputKitchenObjectSO){
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsFried(){
        return state == State.Fried;
    }

}
