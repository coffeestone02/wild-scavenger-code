using UnityEngine;

[CreateAssetMenu(fileName = "RecipeDatabaseSO", menuName = "Scriptable Objects/RecipeDatabaseSO")]
public class RecipeDatabaseSO : ScriptableObject
{
    [SerializeField] CraftingRecipeSO[] _recipes;

    public CraftingRecipeSO[] Recipes => _recipes;
}
