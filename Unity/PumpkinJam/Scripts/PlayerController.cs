using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private float moveSpeed = 7;
    private Grid grid;
    public static Transform movePoint;
    private bool canMove = true;
    public Tilemap Map;
    Tilemap tilemap;
    Vector3Int[] StartPoints;
    public bool ControlsDisabled = false;

    private void Awake()
    {
        Instance = this;
        movePoint = GameObject.FindGameObjectWithTag("Movepoint").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject currentMap = StageController.CurrentMap;
        if (currentMap) { tilemap = currentMap.GetComponent<Tilemap>(); }
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        Vector3Int GetCurrentTilePos()
        {
            return new Vector3Int((int)(movePoint.position.x - 1), (int)(movePoint.position.y - 1), (int)movePoint.position.z);
        }

        bool CheckTile(Vector3Int moveAmount)
        {
            if (tilemap)
            {
                moveAmount = GetCurrentTilePos() + moveAmount;
                if (TileManager.Instance.CheckTile(moveAmount, "End") == true)
                {
                    if (TileManager.Instance.IsMapCleared())
                    {
                        ControlsDisabled = true;
                        print("won");
                        GameManager.Instance.Sound(3);
                        TileManager.Instance.SetTile(moveAmount, "End2");
                        UIManager.Instance.NextButton.SetActive(true);
                        return false;
                    }
                }
                TileBase tile = tilemap.GetTile(moveAmount);
                if (tile && !TileManager.Instance.CheckTile(moveAmount, "Used") && !TileManager.Instance.CheckTile(moveAmount, "End"))
                { return true; }
            }
            GameManager.Instance.Sound(2);
            return false;
        }
        
        if (!ControlsDisabled && Vector3.Distance(transform.position, movePoint.position) <= .1f)
        {
            canMove = true;
            float axisV = Input.GetAxisRaw("Vertical");
            float axisH = Input.GetAxisRaw("Horizontal");

            if (Mathf.Abs(axisH) == 1f && canMove)
            {
                if (Input.GetButtonDown("Horizontal"))
                {
                    canMove = false;
                    if (CheckTile(new Vector3Int((int)axisH, 0, 0)))
                    {
                        movePoint.position += new Vector3(axisH, 0f, 0f);
                        TileManager.Instance.SetTile(GetCurrentTilePos(), "Used");
                        GameManager.Instance.Sound(1);
                        //tilemap.GetTile(GetCurrentTilePos()); //kesken
                    }
                }
            }
            if (Mathf.Abs(axisV) == 1f && canMove)
            {
                if (Input.GetButtonDown("Vertical"))
                {
                    canMove = false;
                    if (CheckTile(new Vector3Int(0, (int)axisV, 0)))
                    {
                        movePoint.position += new Vector3(0f, axisV, 0f);
                        TileManager.Instance.SetTile(GetCurrentTilePos(), "Used");
                        GameManager.Instance.Sound(1);
                    }
                }
            }
        }
    }
}
