using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject ResetButton;
    public GameObject NextButton;

    private void Awake()
    {
        Instance = this;
    }

    public void RestartButtonPressed()
    {
        StageController.Instance.ReloadStage();
    }

    public void NextStageButtonPressed()
    {
        StageController.Instance.NextStage();
    }

}
