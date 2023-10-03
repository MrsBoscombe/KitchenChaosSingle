using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
// ends in SO for Scriptable Object
public class KitchenObjectSO : ScriptableObject
{
    public Transform prefab;    // access directly, which is the point of a scriptable object - never write to it
    public Sprite sprite;
    public string objectName;
}
