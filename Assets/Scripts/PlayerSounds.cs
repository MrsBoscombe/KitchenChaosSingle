using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footstepTimer;
    private float footstepTimerMax = .2f;

    private void Awake()
    {
        player = GetComponent<Player>();
        footstepTimer = footstepTimerMax;   // may not be necessary
    }

    private void Update(){
        footstepTimer -= Time.deltaTime;

        if (footstepTimer < 0){
            footstepTimer = footstepTimerMax;
            if (player.IsWalking()){
                float volume = 2f;
                SoundManager.Instance.PlayFootstepsSound(player.transform.position, volume);
            }
        }
    }

}
