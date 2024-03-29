using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    // Singleton pattern avoids needing to create a Serialized field that
    // would need to be dragged in on each counter! (Will be changed to multiplayer later)
    // See Player for setup of Property for Singleton pattern access
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start(){
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e){
        if (e.selectedCounter == baseCounter){
            Show();
        }
        else{
            Hide();
        }
    }

    private void Show(){
        foreach (GameObject visualGameObject in visualGameObjectArray){
            visualGameObject.SetActive(true);
        }
    }

    private void Hide(){
        foreach (GameObject visualGameObject in visualGameObjectArray){
            visualGameObject.SetActive(false);
        }
    }

}
