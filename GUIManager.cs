using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUI_Manager
{

    public enum Screens
    {
        NONE,
        MAINMENU,
        SETTINGS,
        STORE,
        GAME
    }

    private static Dictionary<Screens, string> _requiredScreens = new Dictionary<Screens, string>() {
        // format: (Screens) screen type, (string) prefab location
        {Screens.MAINMENU, "screens/mainmenu_screen"},
        {Screens.SETTINGS, "screens/settings_screen"},
        {Screens.STORE, "screens/store_screen"},
        {Screens.GAME, "screens/game_screen"}
    };


    private static Dictionary<Screens, ScreenPanel> _availableScreens
            = new Dictionary<Screens, ScreenPanel>();
    private static bool _initialised = false;

    private static Stack<Screens> _uiStack;
    private static GameObject _canvasRootObject;
    private static Screens _currentScreen = Screens.NONE;
    private static float _defaultDuration = 0.5f;


    public static void InitialiseGUI()
    {
        if (_initialised == true)
        {
            Debug.LogError("Error: Cannot initialize GUI again, it is already setup.");
            return;
        }

        _canvasRootObject = GameObject.Instantiate(Resources.Load("Canvas") as GameObject);
        _canvasRootObject.name = "ui_root_canvas";

        if (_canvasRootObject == null)
        {
            Debug.LogError("Error: Could not find 'uicanvas' root");
            return;
        }

        foreach (KeyValuePair<Screens, string> entry in _requiredScreens)
        {
            CreateAndCatalogueScreen(entry.Key, entry.Value);
        }

        _initialised = true;
    }

    private static void CreateAndCatalogueScreen(Screens screenType, string prefabLocation)
    {
        GameObject screenPrefab = Resources.Load<GameObject>(prefabLocation) as GameObject;
        GameObject instantiatedScreen = GameObject.Instantiate(screenPrefab);
        instantiatedScreen.transform.SetParent(_canvasRootObject.transform, false);
        ScreenPanel screenPanel = instantiatedScreen.GetComponent<ScreenPanel>();
        _availableScreens.Add(screenType, screenPanel);
    }



}
