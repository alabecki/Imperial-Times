using EasyUIAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
   public  GameObject imperialTimesLogo;
    public Image logoBackground;
    public Image logoOrniment;
    public TextMeshProUGUI logoText;
    private Graphic logoRect;
    private RectTransform logoTransform;

    private UIAnimation logoFadeIn;

    public Button newGameButton;
    public Button loadGameButton;

    App app;

    // Start is called before the first frame update
    void Start()
    {
        app = UnityEngine.Object.FindObjectOfType<App>();
        Music musicPlayer = app.GetComponentInChildren<Music>();
        musicPlayer.musicOn = true;

        newGameButton.onClick.AddListener(delegate { startNewGame(); });
        loadGameButton.onClick.AddListener(delegate { loadSavedGame(); });

        logoTransform = imperialTimesLogo.GetComponent<RectTransform>();
        logoFadeIn = UIAnimator.Scale(logoTransform, new Vector3(0, 0, 0), new Vector3(2, 2, 2), 2f).SetModifier(Modifier.QuadIn);
        Quaternion rotation = logoTransform.localRotation;
        logoFadeIn.SetEffect(Effect.Spring(0.5f, 1), rotation);

        /* logoFadeIn = new UIGroupAnimation(
          UIAnimator.FadeIn(logoTransform.GetComponent<Image>(), 2),
          UIAnimator.FadeIn(logoTransform.GetComponentInChildren<Image>(), 2),
          UIAnimator.FadeIn(logoTransform.GetComponentInChildren<TextMeshProUGUI>(), 2)
           ); */


        //  logoFadeIn.SetDelay(1f);
        logoFadeIn.Play();
       // imperialTimesLogo.SetActive(false);
      //  logoRect = imperialTimesLogo.GetComponentInChildren<Graphic>();
        //imperialTimesLogo.SetActive(true);
       // logoFadeIn = new UIGroupAnimation(
         //   UIAnimator.FadeIn(logoBackground, 1.5f),
          //  UIAnimator.FadeIn(logoOrniment, 1.5f)
       // diplomacyIn = UIAnimator.ChangeColor(logoBackground, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 3f),
      //  diplomacyIn = UIAnimator.ChangeColor(logoOrniment, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 3f)
      //  );
        // diplomacyIn = UIAnimator.ChangeColor(logoBackground, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1.2f);
        // diplomacyIn = UIAnimator.ChangeColor(logoOrniment, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1.2f);
        //  diplomacyIn = UIAnimator.ChangeColor(logoBackground, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1.2f);
       // logoFadeIn.Play();
      //  Debug.Log("Are you doing anything?");

    }

    private void startNewGame()
    {
        app.newGame = true;
        SceneManager.LoadScene(3);

    }

    private void loadSavedGame()
    {
        app.newGame = false;
        SceneManager.LoadScene(4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
