using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/Blueprint")]
public class BlueprintSO : ScriptableObject
{
    [Header("Result Item")]
    public string itemName;
    public Sprite icon;

    [Header("Requirement 1")]
    public string req1;
    public int req1Amount = 1;

    [Header("Requirement 2")]
    public string req2;
    public int req2Amount = 0;

    [Header("Category")]
    public string category = "Tools";

    public bool HasSecondRequirement =>
        !string.IsNullOrEmpty(req2) && req2Amount > 0;
}
