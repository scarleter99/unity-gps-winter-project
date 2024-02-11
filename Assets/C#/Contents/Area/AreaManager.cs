using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static Define;

public class AreaManager
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

    private AreaGrid _grid;
    private List<GameObject> _players = new();

    private Vector3 _currentPlayerPosition;
    private Vector3 _currentMouseoverPosition;

    private Camera _areaCamera;
    private GameObject _cameraController;
    private GameObject _light;

    private Action OnMouseLeftClick;

    private int _turnCount;
    public int TurnCount
    {
        get => _turnCount;
        set
        {
            _turnCount = value;
            if (TurnCount != 0 && TurnCount % _suddendeathTimer == 0) HandleSuddendeath();
        }
    }
    private int _suddendeathTimer; // timer번의 이동마다 맨 밑 타일 파괴됨.
    private int _suddendeathCount;

    private Vector2[] HERO_SPAWN_POSITION_OFFSET = new[]
        { new Vector2(0, 0.75f), new Vector2(-0.75f, -0.75f), new Vector2(0.75f, -0.75f) };


    public void Init()
    {
        // TODO: 게임 시작 씬에서 이 Init을 실행한다면, 밑의 코드는 지우고 GenerateMap() 등을 다른 곳에서 호출해야함 
        if (Managers.SceneMng.CurrentScene.SceneType != SceneType.AreaScene) return;
        GenerateMap();
        InitCamera();
        SpawnPlayers();
        OnMouseLeftClick -= SelectDestinationTile;
        OnMouseLeftClick += SelectDestinationTile;
        Managers.InputMng.MouseAction -= HandleMouseInput;
        Managers.InputMng.MouseAction += HandleMouseInput;

        AreaState = AreaState.Idle;
        _turnCount = 0;
        _suddendeathCount = 0;
        _suddendeathTimer = 4; // Area마다 타이머를 다르게 한다면 Areadata json 사용
        _light = GameObject.FindGameObjectWithTag("AreaLight");
    }

    private void HandleMouseInput(MouseEvent mouseEvent)
    {   
        if (AreaState != AreaState.Idle || !GetMouseoverCell())
        {
            return;
        }
        switch (mouseEvent)
        {
            case MouseEvent.PointerUp:
                OnMouseLeftClick.Invoke();
                break;
            case MouseEvent.Hover:
                _grid.HandleMouseHover(_currentMouseoverPosition);
                break;
        }
    }

    private void SelectDestinationTile()
    {
        if (_grid.IsNeighbor(_currentPlayerPosition, _currentMouseoverPosition))
        {   
            _grid.GetGridPosition(_currentMouseoverPosition, out int x, out int z);
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

            GameObject player = Managers.ObjectMng.SpawnHero<Knight>(HERO_KNIGHT_ID).gameObject;
            player.transform.position = spawnOriginPos + new Vector3(HERO_SPAWN_POSITION_OFFSET[i].x, 0, HERO_SPAWN_POSITION_OFFSET[i].y);
            _players.Add(player);

        }

    }

    public bool GetMouseoverCell()
    {
        Ray ray = _areaCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit rayHit, maxDistance: 100f, layerMask: LayerMask.GetMask("AreaGrid")))
        {
            _currentMouseoverPosition = rayHit.point;
            Debug.DrawLine(_areaCamera.transform.position, rayHit.point, Color.red);
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
                                     new Vector3(HERO_SPAWN_POSITION_OFFSET[i].x, 0,
                                         HERO_SPAWN_POSITION_OFFSET[i].y);
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
        _areaCamera = _cameraController.transform.GetComponentInChildren<Camera>();
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
        // TODO - 배틀 씬에서 에어리어 씬으로 넘어올 시 배틀 씬 UI 삭제하는 코드. 구조적으로 더 좋은 코드가 가능해 보임.
        GameObject.Destroy(GameObject.FindObjectOfType<UI_BattleScene>().gameObject);

        _cameraController.GetComponent<AreaCameraController>().Freeze = false;
        _light.SetActive(true);
        _cameraController.SetActive(true);
        
        foreach (var player in _players)
        {
            player.transform.position = _currentPlayerPosition + new Vector3(HERO_SPAWN_POSITION_OFFSET[_players.IndexOf(player)].x, 0, HERO_SPAWN_POSITION_OFFSET[_players.IndexOf(player)].y);
        }

        _grid.OnTileEventFinish(_currentPlayerPosition);
        TurnCount++;
    }

    private void HandleSuddendeath()
    {
        
    }
}
