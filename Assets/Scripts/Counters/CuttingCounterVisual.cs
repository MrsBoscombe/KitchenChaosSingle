using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CuttingCounterVisual : MonoBehaviour
{
    private const string CUT = "Cut";
    [SerializeField] CuttingCounter cuttingCounter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start(){
        cuttingCounter.OnCut += CuttingCounterOnCut;
    }

    private void CuttingCounterOnCut(object sender, EventArgs e){
        animator.SetTrigger(CUT);
    }
}
