using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageController : MonoBehaviour
{
    public static StageController Instance;

    public GameObject[] stages;
    private GameObject player;
    private int currentStage = 1;

    public static GameObject CurrentMap;

    // Start is called before the first frame update

    void Awake()
    {
        Instance = this;
        HideStages();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void HideStages()
    {
        int stageCount = 0;
        foreach (GameObject child in stages)
        {
            if (stageCount>0)
            {
                child.SetActive(false);
            }
            stageCount++;
        }
    }

    void Start()
    {
        ChangeStage(currentStage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReloadStage()
    {
        ChangeStage(currentStage);
    }

    public void NextStage()
    {
        currentStage++;
        ChangeStage(currentStage);
    }

    public void ChangeStage(int stage)
    {
        UIManager.Instance.NextButton.SetActive(false);
        PlayerController.Instance.ControlsDisabled = false;
        print("stage to: " + stage.ToString());
        GameObject newStage = Instantiate(stages[stage].gameObject, new Vector3(0.5f,0.5f,0), Quaternion.identity, gameObject.transform);
        newStage.SetActive(true);
        GameObject oldStage = CurrentMap;
        if (oldStage) { Destroy(oldStage); }
        CurrentMap = newStage;
        PlayerController.movePoint.position = TileManager.Instance.FindStartTile(CurrentMap) + new Vector3Int(1,1,0);
    }
}
