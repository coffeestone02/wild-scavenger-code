using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipeSO", menuName = "Scriptable Objects/CraftingRecipeSO")]
public class CraftingRecipeSO : ScriptableObject
{
    [Header("제작에 필요한 아이템들")]
    [SerializeField] Item[] _ingredients;

    [Header("제작 결과 아이템")]
    [SerializeField] Item _resultItem;

    public Item[] Ingredients => _ingredients;
    public Item ResultItem => _resultItem;
}
