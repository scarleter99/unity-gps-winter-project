using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using static Define;
using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{   
    public static BattlefieldManager Instance { get; private set; }

    private BattleState _battleState;
    public BattleState BattleState
    {
        get => _battleState;
        set
        {
            var tmp = _battleState;
            _battleState = value;
            OnBattleStateChange(tmp, _battleState);
        }
    }

    private BattleGridManager _gridManager;

    // 그리드 기준 위치 (그리드 좌표 0,0)
    [SerializeField]
    private Vector3 _playergridOriginPos; // 기본: (-3, 0.1, -4.75)
    [SerializeField]
    private Vector3 _enemygridOriginPos; // 기본: (-3, 0.1, 2.25)

    private const string _playerPrefabPath = "Players/Player";
    private const string _monsterPrefabPath = "Monsters/FlyingDemon";

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        _gridManager.HandleMouseHover();
    }

    private void Init()
    {
        _gridManager = new BattleGridManager(_playergridOriginPos, _enemygridOriginPos);
        BattleState = BattleState.Idle;
        Managers.InputMng.MouseAction += HandleMouseInput;
        GeneratePrefabs();
    }

    private void GeneratePrefabs()
    {   
        // 현재는 테스트를 위해 그리드의 모든 셀에 프리팹을 생성하지만 추후 수정을 통해 특정 위치만 생성하도록 해야함.
        for (int z = 0; z < _gridManager.PlayerGrid.Height; z++)
        {
            for (int x = 0; x < _gridManager.PlayerGrid.Width; x++)
            {
                _gridManager.PlayerGrid.InstantiatePrefab(_playerPrefabPath, x, z);
            }
        }
        for (int z = 0; z < _gridManager.EnemyGrid.Height; z++)
        {
            for (int x = 0; x < _gridManager.EnemyGrid.Width; x++)
            {
                _gridManager.EnemyGrid.InstantiatePrefab(_monsterPrefabPath, x, z, 180);
            }
        }
    }

    private void HandleMouseInput(MouseEvent mouseEvent)
    {
        if (!_gridManager.TryGetGridInformation())
        {
            Debug.Log("Could not get grid information!");
            return;
        }

        switch (mouseEvent)
        {
            case MouseEvent.Click:
                _gridManager.onMouseLeftClick.Invoke();
                break;
        }
    }

    private void OnBattleStateChange(BattleState from, BattleState to)
    {   
        _gridManager.OnBattleStateChange(from, to);
        switch (from)
        {
            case BattleState.Idle:
                break;
            case BattleState.SelectingTargetPlayer:
                break;
            case BattleState.SelectingTargetMonster:
                break;
        }

        switch (to)
        {
            case BattleState.Idle:
                break;
            case BattleState.SelectingTargetPlayer:
                break;
            case BattleState.SelectingTargetMonster:
                break;
        }

        ///////////////////////////////////////////////
        // Or....?
        //
        //switch (from)
        //{
        //    case BattleState.Idle:
        //        switch (to)
        //        {
        //            case BattleState.SelectingTargetMonster:
        //                break;
        //        }
        //        break;
        //    case BattleState.SelectingTargetPlayer:
        //        switch (to)
        //        {
        //            case BattleState.Idle:
        //                break;
        //        }
        //        break;
        //    case BattleState.SelectingTargetMonster:
        //        switch (to)
        //        {
        //            case BattleState.SelectingTargetPlayer:
        //                break;
        //        }
        //        break;
        //}
        ////////////////////////////////////////////////
    }
}
