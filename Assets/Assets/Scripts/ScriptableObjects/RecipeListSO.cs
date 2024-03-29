using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu()] // We only need one recipe List, so we don't need to add it to the menu
// Benefit to this set up is it creates one list containing all of the recipes in the system
// as opposed to creating different lists in multiple objects that need access to all of the 
// recipes in the system
public class RecipeListSO : ScriptableObject
{
    public List<RecipeSO> recipeSOList;
}
