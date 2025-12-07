using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public void SetCategory(string category)
    {
        currentCategory = category;
        BuildRecipeUI();
    }

    private string currentCategory = "";

    [Header("UI References")]
    public GameObject craftingScreenUI;
    public Transform recipeContainer;
    public GameObject recipeUIPrefab;

    [Header("Blueprints")]
    public List<BlueprintSO> blueprints = new List<BlueprintSO>();

    [Header("Crafting Feedback")]
    public AudioClip craftSound;
    public AudioClip craftingSound;
    public ParticleSystem craftParticles;

    private readonly Dictionary<BlueprintSO, RecipeUI> _recipeUIDict = new Dictionary<BlueprintSO, RecipeUI>();
    private bool _isOpen;
    private bool _isCrafting;

    private void Start()
    {
        if (craftingScreenUI != null)
            craftingScreenUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleCrafting();
        }

        if (_isOpen && !_isCrafting)
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

    private void BuildRecipeUI()
    {
        if (recipeUIPrefab == null || recipeContainer == null)
            return;

        foreach (Transform child in recipeContainer)
        {
            Destroy(child.gameObject);
        }
        _recipeUIDict.Clear();

        if (string.IsNullOrEmpty(currentCategory))
            return;

        List<BlueprintSO> filteredBlueprints = new List<BlueprintSO>();

        foreach (var bp in blueprints)
        {
            if (bp == null)
                continue;

            if (currentCategory == "All" || bp.category == currentCategory)
            {
                filteredBlueprints.Add(bp);
            }
        }

        foreach (var bp in filteredBlueprints)
        {
            var uiGO = Instantiate(recipeUIPrefab, recipeContainer);
            uiGO.name = $"Recipe_{bp.itemName}";
            var ui = uiGO.GetComponent<RecipeUI>();

            if (ui == null)
                continue;

            ui.Initialize(bp, () => StartCraft(bp));
            _recipeUIDict[bp] = ui;
        }
    }

    private void StartCraft(BlueprintSO bp)
    {
        if (bp == null || _isCrafting)
            return;

        bool canCraft = CanCraft(bp);

        if (!canCraft)
        {
            Debug.Log("Cannot craft - missing requirements!");
            return;
        }

        StartCoroutine(CraftCoroutine(bp));
    }

    private IEnumerator CraftCoroutine(BlueprintSO bp)
    {
        _isCrafting = true;

        RecipeUI ui = _recipeUIDict[bp];
        if (ui != null)
        {
            ui.StartCraftingAnimation(bp.craftTime);
        }

        if (craftingSound != null)
        {
            AudioSource.PlayClipAtPoint(craftingSound, Camera.main.transform.position, 0.5f);
        }

        yield return new WaitForSeconds(bp.craftTime);

        CompleteCraft(bp);

        _isCrafting = false;
    }

    private void CompleteCraft(BlueprintSO bp)
    {
        InventorySystem.Instance.AddToInventory(bp.itemName);

        if (!string.IsNullOrEmpty(bp.req1))
            InventorySystem.Instance.RemoveItem(bp.req1, bp.req1Amount);

        if (!string.IsNullOrEmpty(bp.req2))
            InventorySystem.Instance.RemoveItem(bp.req2, bp.req2Amount);

        InventorySystem.Instance.ReCalculateList();

        PlayCraftFeedback();
        RefreshAllRecipes();
    }

    private bool CanCraft(BlueprintSO bp)
    {
        bool canCraft = true;

        if (!string.IsNullOrEmpty(bp.req1))
            canCraft &= InventorySystem.Instance.GetItemCount(bp.req1) >= bp.req1Amount;

        if (!string.IsNullOrEmpty(bp.req2))
            canCraft &= InventorySystem.Instance.GetItemCount(bp.req2) >= bp.req2Amount;

        return canCraft;
    }

    private void PlayCraftFeedback()
    {
        if (craftSound != null)
        {
            AudioSource.PlayClipAtPoint(craftSound, Camera.main.transform.position);
        }

        if (craftParticles != null)
        {
            craftParticles.Play();
        }
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

        bool canCraft = CanCraft(bp);

        ui.Refresh(req1Count, req2Count, canCraft);
    }
}
