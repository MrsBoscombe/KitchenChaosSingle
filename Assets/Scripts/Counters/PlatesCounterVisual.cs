using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualGameObjectList;

    private void OnAwake(){
        //plateVisualGameObjectList = new List<GameObject>();   // caused null reference issues
    }

    private void Start(){
        plateVisualGameObjectList = new List<GameObject>();
        platesCounter.OnPlateSpawned += PlatesCounterOnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounterOnPlateRemoved;
    }

    private void PlatesCounterOnPlateSpawned(object sender, EventArgs e){
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        
        float plateOffsetY = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }

    private void PlatesCounterOnPlateRemoved(object sender, EventArgs e){
        if (plateVisualGameObjectList.Count > 0){
            GameObject removedPlateVisual = plateVisualGameObjectList[^1];
            plateVisualGameObjectList.Remove(removedPlateVisual);
            Destroy(removedPlateVisual); //.Destroy();
        }
        
    }
}
