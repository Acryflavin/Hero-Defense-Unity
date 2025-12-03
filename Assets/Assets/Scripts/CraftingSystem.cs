using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public void SetCategory(string category)
    {
        currentCategory = category;
        //BuildRecipeUI();   // refreshes the recipe list
    }


    private string currentCategory = "All";


    [Header("UI References")]
    public GameObject craftingScreenUI;   // Panel that holds the whole crafting window
    public Transform recipeContainer;     // Parent for recipe UI entries
    public GameObject recipeUIPrefab;     // Prefab with RecipeUI on it

    [Header("Blueprints")]
    public List<BlueprintSO> blueprints = new List<BlueprintSO>();

    private readonly Dictionary<BlueprintSO, RecipeUI> _recipeUIDict = new Dictionary<BlueprintSO, RecipeUI>();

    private bool _isOpen;

    private void Start()
    {
        if (craftingScreenUI != null)
            craftingScreenUI.SetActive(false);

        InitializeUI();
    }

    private void Update()
    {
        // Toggle crafting menu with K
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleCrafting();
        }

        if (_isOpen)
        {
            RefreshAllRecipes();
        }
    }

    private void ToggleCrafting()
    {
        _isOpen = !_isOpen;

        if (craftingScreenUI != null)
            craftingScreenUI.SetActive(_isOpen);
    }

    private void InitializeUI()
    {
        if (recipeUIPrefab == null)
        {
            Debug.LogError("CraftingSystem: recipeUIPrefab is not assigned.");
            return;
        }

        if (recipeContainer == null)
        {
            Debug.LogError("CraftingSystem: recipeContainer is not assigned.");
            return;
        }

        foreach (var bp in blueprints)
        {
            if (currentCategory != "All" && bp.category != currentCategory)
                continue;

            if (bp == null)
                continue;

            var uiGO = Instantiate(recipeUIPrefab, recipeContainer);
            uiGO.name = $"Recipe_{bp.itemName}";
            var ui = uiGO.GetComponent<RecipeUI>();

            if (ui == null)
            {
                Debug.LogError("CraftingSystem: recipeUIPrefab is missing RecipeUI component.");
                continue;
            }

            ui.Initialize(bp, () => Craft(bp));
            _recipeUIDict[bp] = ui;
        }
    }

    private void Craft(BlueprintSO bp)
    {
        if (bp == null)
            return;

        // Add crafted item
        InventorySystem.Instance.AddToInventory(bp.itemName);

        // Remove requirements
        if (!string.IsNullOrEmpty(bp.req1))
            InventorySystem.Instance.RemoveItem(bp.req1, bp.req1Amount);

        if (!string.IsNullOrEmpty(bp.req2))
            InventorySystem.Instance.RemoveItem(bp.req2, bp.req2Amount);

        InventorySystem.Instance.ReCalculateList();

        RefreshAllRecipes();
    }

    private void RefreshAllRecipes()
    {
        foreach (var pair in _recipeUIDict)
        {
            RefreshRecipe(pair.Key, pair.Value);
        }
    }

    private void RefreshRecipe(BlueprintSO bp, RecipeUI ui)
    {
        if (bp == null || ui == null)
            return;

        int req1Count = 0;
        int req2Count = 0;

        if (!string.IsNullOrEmpty(bp.req1))
            req1Count = InventorySystem.Instance.GetItemCount(bp.req1);

        if (!string.IsNullOrEmpty(bp.req2))
            req2Count = InventorySystem.Instance.GetItemCount(bp.req2);

        if (ui.req1Text != null && !string.IsNullOrEmpty(bp.req1))
            ui.req1Text.text = $"{bp.req1Amount} {bp.req1} [{req1Count}]";

        if (ui.req2Text != null)
        {
            if (!string.IsNullOrEmpty(bp.req2))
            {
                ui.req2Text.gameObject.SetActive(true);
                ui.req2Text.text = $"{bp.req2Amount} {bp.req2} [{req2Count}]";
            }
            else
            {
                ui.req2Text.gameObject.SetActive(false);
            }
        }

        bool canCraft = true;

        if (!string.IsNullOrEmpty(bp.req1))
            canCraft &= req1Count >= bp.req1Amount;

        if (!string.IsNullOrEmpty(bp.req2))
            canCraft &= req2Count >= bp.req2Amount;

        if (ui.craftButton != null)
            ui.craftButton.interactable = canCraft;
    }
}
