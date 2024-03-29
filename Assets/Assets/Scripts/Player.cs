
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public event EventHandler PickedSomething;
    
    // using a static property instead of a field
    // get is public, set is private
    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    // This class is created to allow us to add the selectedCounter to the arguments sent to the event
    public class OnSelectedCounterChangedEventArgs : EventArgs{
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7.0f;          // SerializeField allows editing from the Unity Editor
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private KitchenObject kitchenObject;
    private bool isWalking;   
    private Vector3 lastInteractDirection;
    private BaseCounter selectedCounter;

    private void Awake()
    {
        if (Instance != null){
            Debug.LogError("There is more than one player instance!");
        }
        Instance = this;
    }

    private void Start(){
        // adding GaneInputOnInteractAction to the gameInput's event listener
        gameInput.OnInteractAction += GameInputOnInteractAction;
        gameInput.OnInteractAlternateAction += GameInputOnInteractAlternateAction;
    }

    private void GameInputOnInteractAction(object sender, EventArgs e){

        if (KitchenGameManager.Instance.IsGamePlaying() && selectedCounter != null){
            selectedCounter.Interact(this);
        }
    }

    private void GameInputOnInteractAlternateAction(object sender, EventArgs e){
         if (KitchenGameManager.Instance.IsGamePlaying() && selectedCounter != null){
            selectedCounter.InteractAlternate(this);
        }
    }


    private void Update(){
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking(){
        return isWalking;
    }

    private void HandleInteractions(){
        
        // Set up empty 2D vector for movement
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // adjust the 2D movement Vector to our 3D space
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero){
            lastInteractDirection = moveDir;
        }
        float interactDistance = 2f;
        // could also have created a RaycastHit variable before this call
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, countersLayerMask)){
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)){
                // Has ClearCounter
                if (baseCounter != selectedCounter){
                    SetSelectedCounter(baseCounter);
                }
            }
            else{
                SetSelectedCounter(null);
            }
        }
        else{
            SetSelectedCounter(null);
        }
        //Debug.Log(selectedCounter);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter){
        this.selectedCounter = selectedCounter;
         // Fire off the event that the selected counter has changed
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs{ 
            selectedCounter = this.selectedCounter // "this." is unnecessary, C# will automatically determine which selectedCounter is which in the assignment
        });
        //Debug.Log($"SetSelectedCounter called: {selectedCounter}");
    }

    private void HandleMovement(){
        // Set up empty 2D vector for movement
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // adjust the 2D movement Vector to our 3D space
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = Time.deltaTime * moveSpeed;

        // Check to see if anything is in the way before we apply movement
        float playerRadius = .7f;     // estimated playerSize
        float playerHeight = 2f;    // estimated playerHeight
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,  playerRadius, moveDir, moveDistance);

        // the player is walking if the moveDir is not (0, 0, 0)
        isWalking = moveDir != Vector3.zero;

        // Move the player without physics
        // Time.deltaTime alleviates framerate issues

        if (!canMove){
            // Cannot move in the desired direction
            // Test for X direction
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -0.5f || moveDir.x > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,  playerRadius, moveDirX, moveDistance);

            if (canMove){
                // can move only on x direction
                moveDir = moveDirX;
            }
            else{
                // Cannot move in the desired direction
                // Test for Z direction
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -0.5f || moveDir.z > 0.5f)  && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,  playerRadius, moveDirZ, moveDistance);
                if (canMove){
                    // can move only on z direction
                    moveDir = moveDirZ;
                }
            }
        }
        
        if (canMove){
            transform.position = transform.position += moveDir * moveDistance;      
        }

        float rotationSpeed = 10f;
        // point the player in the direction it is moving / Slerp interpolates / smooths out the rotation over time
        if (moveDir != Vector3.zero){
            transform.forward = Vector3.Slerp(transform.position, moveDir, Time.deltaTime * rotationSpeed);   
        }
 
    }

    public Transform GetKitchenObjectFollowTransform(){
        return kitchenObjectHoldPoint; 
    }

    public void SetKitchenObject(KitchenObject kitchenObject){
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null){
            PickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject(){
        return kitchenObject;
    }

    public void ClearKitchenObject(){
        kitchenObject = null;
    }

    public bool HasKitchenObject(){
        /*if (kitchenObject == null){
            Debug.Log("Player's kitchen object is null");
        }
        else{
            Debug.Log("Player's kitchen object is " + kitchenObject);
        }*/
        return kitchenObject != null;
    }
}
