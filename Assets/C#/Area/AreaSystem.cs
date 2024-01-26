using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class AreaSystem : MonoBehaviour
{   
    public AreaName AreaName { get; set; }
    public AreaState AreaState { get; set; }

    private HexGrid _grid;

    private Vector2 _currentPlayerPosition;
    private Vector3 _recentMousePosition;

    private Camera _mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        AreaState = AreaState.Idle;
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (AreaState == AreaState.Idle)
        {
            TryGetGridInformation();
        }
    }

    public void Init()
    {
        GenerateMap();
        SpawnPlayers();
    }

    private void GenerateMap()
    {
        AreaGenerator _areaGenerator = new AreaGenerator(AreaName, Vector3.zero);
        _areaGenerator.GenerateMap();
        _grid = _areaGenerator.Grid;
    }

    private void SpawnPlayers()
    {
        Vector3 spawnpoint = _grid.GetWorldPosition(_grid.Width / 2, 0, 1.02f);
        GameObject player = Managers.GameMng.Spawn(WorldObject.Player, "Players/Player");
        player.transform.position = spawnpoint;
    }

    public bool TryGetGridInformation()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit rayHit, maxDistance: 100f, layerMask: LayerMask.GetMask("BattleGrid")))
        {
            _recentMousePosition = rayHit.point;
            Debug.DrawLine(_mainCamera.transform.position, rayHit.point);
            return true;
        }

        return false;
    }
}
