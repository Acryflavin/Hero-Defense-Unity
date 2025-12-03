This is a placeholder file representing the CraftingDemoScene.

Unity scenes cannot be reliably generated outside of the Unity editor, 
but here is EXACTLY how to build the working scene:

1. Create a new Scene in Unity and save it as: Assets/Scenes/CraftingDemoScene.unity

2. In the Hierarchy, create:
   - Empty GameObject: "InventorySystem"
       * Add Component: InventorySystem (from Scripts folder)
   - Empty GameObject: "CraftingSystem"
       * Add Component: CraftingSystem (from Scripts folder)

3. Create a Canvas:
   - GameObject -> UI -> Canvas
   - Inside Canvas, create a Panel and rename it to: "CraftingScreenUI"
   - Set CraftingScreenUI inactive (uncheck the checkbox at the top of the Inspector)

4. Inside CraftingScreenUI, create:
   - Empty GameObject: "RecipeContainer"
     (optionally add a Vertical Layout Group to it)

5. Create the Recipe UI prefab:
   - Right-click in Hierarchy -> UI -> Panel -> rename to "UI_CraftItem"
   - Inside UI_CraftItem create:
       * UI -> Text: "NameText"
       * UI -> Text: "req1"
       * UI -> Text: "req2"
       * UI -> Button: "Button"
   - Add the RecipeUI component to UI_CraftItem
   - Drag NameText, req1, req2, Button into the respective fields on RecipeUI
   - Drag UI_CraftItem from Hierarchy into Assets/Prefabs to make it a prefab.

6. Create at least one Blueprint:
   - Right-click in Project window -> Create -> Crafting -> Blueprint
   - Name it "AxeBlueprint"
   - Fill it like:
       itemName = "Axe"
       req1 = "Stone", req1Amount = 3
       req2 = "Stick", req2Amount = 3
   - Save it in Assets/Blueprints.

7. Wire up the CraftingSystem in the scene:
   - Select the CraftingSystem GameObject
   - In the Inspector:
       * craftingScreenUI: drag the CraftingScreenUI panel
       * recipeContainer: drag the RecipeContainer object
       * recipeUIPrefab: drag the UI_CraftItem prefab
       * blueprints: set size to 1 and drag in AxeBlueprint

8. Press Play.
   - Press K to open/close the crafting menu.
   - Add items to Inventory at runtime (e.g., via script or temporary debug buttons).
   - When you have enough Stones and Sticks, the Craft button for the Axe becomes interactable.

Once you follow these steps, you will have a fully working crafting UI driven by this code.
