using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum MenuSelector
{
    main,
    game,
    end
}
public enum ValueType
{
    score,
    combo,
    health
}

public class UIManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject mainMenuObject;
    public GameObject gameMenuObject;
    public GameObject endMenuObject;

    [Header("Scoring")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI comboText;
    public Transform healthParent;
    public GameObject healthPrefab;

    void Awake()
    {
        SelectMenu(MenuSelector.main);    
    }

    public void ShowAndResetGameMenu()
    {
        SelectMenu(MenuSelector.game);
        scoreText.text = "0";
        comboText.text = "x1";
        AddHeartsBack();
    }

    public void SelectMenu(MenuSelector selector)
    {
        mainMenuObject.SetActive(false);
        gameMenuObject.SetActive(false);
        endMenuObject.SetActive(false);

        switch(selector)
        {
            case MenuSelector.main: mainMenuObject.SetActive(true); break;
            case MenuSelector.game: gameMenuObject.SetActive(true); break;
            case MenuSelector.end: endMenuObject.SetActive(true); break;
            default: break;
        }
    }

    public void AddHeartsBack()
    {
        while(healthParent.childCount < 3)
        {
            Instantiate(healthPrefab, healthParent);
        }
    }

    public void UpdateGUI(ValueType valueType, float value)
    {
        switch(valueType)
        {
            case ValueType.score: scoreText.text = value.ToString(); break;
            case ValueType.combo: comboText.text = "x"+value; break;
            case ValueType.health: Destroy(healthParent.GetChild(0).gameObject); break;
        }
    }
}
