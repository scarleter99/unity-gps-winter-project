using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static Define;

public class AreaSystem : MonoBehaviour
{   
    public AreaName AreaName { get; set; }

    private AreaState _areaState;

    public AreaState AreaState
    {
        get => _areaState;
        set
        {
            _areaState = value;
            if (value == AreaState.Idle)
            {
                _grid.ChangeNeighborTilesColor(_currentPlayerPosition, TileColorChangeType.Highlight);
            }
        }
    }

    private HexGrid _grid;
    private List<GameObject> _players = new();

    private Vector3 _currentPlayerPosition;
    private Vector3 _recentMouseWorldPosition;

    private Camera _mainCamera;
    private GameObject _cameraController;
    private GameObject _light;

    private Action OnMouseLeftClick;

    private Vector2[] PLAYER_SPAWN_POSITION_OFFSET = new[]
        { new Vector2(0, 0.75f), new Vector2(-0.75f, -0.75f), new Vector2(0.75f, -0.75f) };


    void Update()
    {
        if (AreaState == AreaState.Idle)
        {
            if (TryGetGridInformation())
            {
                _grid.HandleMouseHover(_recentMouseWorldPosition);
            }
            else
            {
                _grid.ResetMouseHover();
            }
        }
    }

    public void Init()
    {
        GenerateMap();
        SpawnPlayers();
        InitCamera();
        OnMouseLeftClick -= SelectDestinationTile;
        OnMouseLeftClick += SelectDestinationTile;
        Managers.InputMng.MouseAction -= HandleMouseInput;
        Managers.InputMng.MouseAction += HandleMouseInput;
        AreaState = AreaState.Idle;
        _light = GameObject.FindGameObjectWithTag("AreaLight");
    }

    private void HandleMouseInput(MouseEvent mouseEvent)
    {   
        if (AreaState != AreaState.Idle || !TryGetGridInformation())
        {
            return;
        }
        switch (mouseEvent)
        {
            case MouseEvent.PointerUp:
                OnMouseLeftClick.Invoke();
                break;
        }
    }

    private void SelectDestinationTile()
    {
        if (_grid.IsNeighbor(_currentPlayerPosition, _recentMouseWorldPosition))
        {   
            _grid.GetGridPosition(_recentMouseWorldPosition, out int x, out int z);
            AreaState = AreaState.Moving;
            _grid.ChangeNeighborTilesColor(_currentPlayerPosition, TileColorChangeType.Reset);
            MovePlayers( _grid.GetWorldPosition(x, z) + new Vector3(0, 1.02f,0));
        }
    }

    private void GenerateMap()
    {
        AreaGenerator _areaGenerator = new AreaGenerator(AreaName, new Vector3(100, 0, 100));
        _areaGenerator.GenerateMap();
        _grid = _areaGenerator.Grid;
    }

    private void SpawnPlayers()
    {
        Vector3 spawnOriginPos = _grid.GetWorldPosition(_grid.Width / 2, 0, 1.02f);
        _currentPlayerPosition = spawnOriginPos;
        for (int i = 0; i < 3; i++)
        {
            GameObject player = Managers.GameMng.Spawn(WorldObject.Player, "Players/Player");
            player.transform.position = spawnOriginPos + new Vector3(PLAYER_SPAWN_POSITION_OFFSET[i].x, 0, PLAYER_SPAWN_POSITION_OFFSET[i].y);
            _players.Add(player);
        }

    }

    public bool TryGetGridInformation()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit rayHit, maxDistance: 100f, layerMask: LayerMask.GetMask("AreaGrid")))
        {
            _recentMouseWorldPosition = rayHit.point;
            Debug.DrawLine(_mainCamera.transform.position, rayHit.point, Color.red);
            return true;
        }

        return false;
    }

    private void MovePlayers(Vector3 destination)
    {
        Sequence moveSequence = DOTween.Sequence();

        for (int i = 0; i < 3; i++)
        {
            Vector3 destWithOffset = destination +
                                     new Vector3(PLAYER_SPAWN_POSITION_OFFSET[i].x, 0,
                                         PLAYER_SPAWN_POSITION_OFFSET[i].y);
            _players[i].transform.LookAt(destWithOffset);
            moveSequence.Join(_players[i].transform.DOMove(destWithOffset, 0.7f));
        }
        moveSequence.Play().OnComplete(() => {OnMoveFinish(destination);});
    }

    private void OnMoveFinish(Vector3 currentPosition)
    {
        _currentPlayerPosition = currentPosition;
        _grid.OnTileEnter(currentPosition);
    }

    private void InitCamera()
    {
        _cameraController = Managers.ResourceMng.Instantiate("Area/@AreaCameraController");
        _cameraController.transform.position = new Vector3(_currentPlayerPosition.x, 50, _currentPlayerPosition.z - 40);
        _mainCamera = _cameraController.transform.GetComponentInChildren<Camera>();
    }

    public void FreezeCamera()
    {
        _cameraController.GetComponent<AreaCameraController>().Freeze = true;
    }

    public void OnBattleSceneLoadStart()
    {
        _light.SetActive(false);
        _cameraController.SetActive(false);
    }

    public void OnBattleSceneUnloadFinish()
    {   
        Destroy(FindObjectOfType<UI_BattleScene>().gameObject);
        _cameraController.GetComponent<AreaCameraController>().Freeze = false;
        _light.SetActive(true);
        _cameraController.SetActive(true);
        
        foreach (var player in _players)
        {
            player.transform.position = _currentPlayerPosition + new Vector3(PLAYER_SPAWN_POSITION_OFFSET[_players.IndexOf(player)].x, 0, PLAYER_SPAWN_POSITION_OFFSET[_players.IndexOf(player)].y);
        }

        _grid.OnTileEventFinish(_currentPlayerPosition);
    }
}
