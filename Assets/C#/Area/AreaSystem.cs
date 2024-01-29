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
    private Action OnMouseLeftClick;

    private Vector2[] PLAYER_SPAWN_POSITION_OFFSET = new[]
        { new Vector2(0, 0.75f), new Vector2(-0.75f, -0.75f), new Vector2(0.75f, -0.75f) };

    void Start()
    {
        _mainCamera = Camera.main;
    }

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
        OnMouseLeftClick -= SelectDestinationTile;
        OnMouseLeftClick += SelectDestinationTile;
        Managers.InputMng.MouseAction += HandleMouseInput;
        AreaState = AreaState.Idle;
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
        AreaGenerator _areaGenerator = new AreaGenerator(AreaName, Vector3.zero);
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
        // for test /////
        AreaState = AreaState.Idle;
        ////////////////
    }
}
