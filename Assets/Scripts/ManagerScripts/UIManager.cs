using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    [SerializeField] private GameObject[] panels;
    [SerializeField] private TextMeshProUGUI scoreTxt , finishScoreTxt;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void Awake()
    {

    }


    private void OnEnable()
    {
        GameManager.OnGameStateChanged += PanelActivator;
        GameManager.OnScoreStateChanged += UpdateUI;
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//
    private void Start()
    {
        GameManager.Instance.UpdateGameState(GameState.mainmenu);
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void UpdateUI(AddScore state)
    {
        if(state == AddScore.addScore)
        {
            scoreTxt.text = GameManager.Instance.totalScore.ToString();
        }

        if(state == AddScore.multiplierScore)
        {
            finishScoreTxt.text = GameManager.Instance.totalScore.ToString();
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void PanelActivator(GameState state) 
    {
        panels[0].SetActive(state == GameState.mainmenu);
        panels[1].SetActive(state == GameState.playing || state == GameState.bonus);
        panels[2].SetActive(state == GameState.restart);
        panels[3].SetActive(state == GameState.finish);

        switch (state)
        {
            case GameState.mainmenu:
                scoreTxt.text = GameManager.Instance.totalScore.ToString();
                break;
            case GameState.playing:
                break;
            case GameState.restart:
                break;
            case GameState.bonus:
                break;
            case GameState.finish:
                finishScoreTxt.text = GameManager.Instance.totalScore.ToString();
                break;
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= PanelActivator;
        GameManager.OnScoreStateChanged -= UpdateUI;
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

}
