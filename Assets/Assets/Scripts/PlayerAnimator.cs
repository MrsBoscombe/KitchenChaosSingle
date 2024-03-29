using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private Animator playerAnim;
    [SerializeField] private Player player;
    

    private void Awake(){
        playerAnim = GetComponent<Animator>();
    }

    private void Update(){
        playerAnim.SetBool(IS_WALKING, player.IsWalking());
    
    }
}
