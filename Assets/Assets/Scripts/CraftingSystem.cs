using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public void SetCategory(string category)
    {
        currentCategory = category;
        BuildRecipeUI();   // refreshes the recipe list
    }


    private string currentCategory = "";


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

        //if (_isOpen)
        //{
        //    BuildRecipeUI();  // Build UI when opening
        //}
    }


    private void BuildRecipeUI()
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

        // Clear existing recipe UI
        foreach (Transform child in recipeContainer)
        {
            Destroy(child.gameObject);
        }
        _recipeUIDict.Clear();

        // If no category selected, don't show anything
        if (string.IsNullOrEmpty(currentCategory))
        {
            Debug.Log("No category selected yet.");
            return;
        }

        Debug.Log($"Building UI for category: {currentCategory}");
        Debug.Log($"Total blueprints in list: {blueprints.Count}");

        // Create UI for blueprints matching the current category
        int matchCount = 0;
        foreach (var bp in blueprints)
        {
            if (bp == null)
            {
                Debug.LogWarning("Null blueprint in list!");
                continue;
            }

            Debug.Log($"Blueprint: {bp.itemName}, Category: {bp.category}");

            if (currentCategory != "All" && bp.category != currentCategory)
            {
                Debug.Log($"  Skipping {bp.itemName} - doesn't match {currentCategory}");
                continue;
            }

            matchCount++;
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

        Debug.Log($"Created {matchCount} recipe UI elements for category {currentCategory}");
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
