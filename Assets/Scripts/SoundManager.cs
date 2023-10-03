using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    private float volume = 1f;

    private void Awake(){
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    private void Start(){
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManagerOnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManagerOnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounterOnAnyCut;
        Player.Instance.PickedSomething += PlayerPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounterOnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounterOnAnyObjectTrashed;

    }

    private void CuttingCounterOnAnyCut(object sender, EventArgs e){
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManagerOnRecipeSuccess(object sender, EventArgs e){
        PlaySound(audioClipRefsSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManagerOnRecipeFailed(object sender, EventArgs e){
        PlaySound(audioClipRefsSO.deliveryFail, DeliveryCounter.Instance.transform.position); 
    }

    private void PlayerPickedSomething(object sender, EventArgs e){
        Player player = sender as Player;
        PlaySound(audioClipRefsSO.objectPickup, player.transform.position);
    }

    private void BaseCounterOnAnyObjectPlacedHere(object sender, EventArgs e){
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }
    
    private void TrashCounterOnAnyObjectTrashed(object sender, EventArgs e){
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f){
        AudioSource.PlayClipAtPoint(audioClip, position, volume * volumeMultiplier);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f){
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume * volumeMultiplier);
    }

    public void PlayCountdownSound(){
        PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }

    public void PlayFootstepsSound(Vector3 position, float volumeMultiplier){
        PlaySound(audioClipRefsSO.footstep, position, volume * volumeMultiplier);
    }

    public void ChangeVolume(){
        volume += .1f;

        if (volume > 1f){
            volume = 0f;
        }        
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public void PlayWarningSound(Vector2 position){
        PlaySound(audioClipRefsSO.warning, position);
    }

    public float GetVolume(){
        return volume;
    }
}
