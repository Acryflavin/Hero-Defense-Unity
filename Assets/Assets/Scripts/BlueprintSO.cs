using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/Blueprint")]
public class BlueprintSO : ScriptableObject
{
    public Sprite icon;  // this is the picture for the item

    public string category = "Tools";

    public string itemName;

    [Header("Requirement 1")]
    public string req1;
    public int req1Amount;

    [Header("Requirement 2 (optional)")]
    public string req2;
    public int req2Amount;
}
