using UnityEngine;

[CreateAssetMenu(fileName = "StoreSO", menuName = "Scriptable Objects/StoreSO")]
public class StoreSO : ScriptableObject
{
    [SerializeField] Item[] _products;

    public Item[] Products => _products;
}
