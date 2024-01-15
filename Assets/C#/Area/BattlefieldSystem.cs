using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using static Define;
using UnityEngine;

public class BattlefieldSystem : MonoBehaviour
{   
    public static BattlefieldSystem Instance { get; private set; }

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

    private BattleGridSystem _gridSystem;

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
        _gridSystem.HandleMouseHover();
    }

    private void Init()
    {
        _gridSystem = new BattleGridSystem(_playergridOriginPos, _enemygridOriginPos);
        BattleState = BattleState.Idle;
        Managers.InputMng.MouseAction += HandleMouseInput;
        GeneratePrefabs();
    }

    private void GeneratePrefabs()
    {   
        // 현재는 테스트를 위해 그리드의 모든 셀에 프리팹을 생성하지만 추후 수정을 통해 특정 위치만 생성하도록 해야함.
        for (int z = 0; z < _gridSystem.PlayerGrid.Height; z++)
        {
            for (int x = 0; x < _gridSystem.PlayerGrid.Width; x++)
            {
                _gridSystem.PlayerGrid.InstantiatePrefab(_playerPrefabPath, x, z);
            }
        }
        for (int z = 0; z < _gridSystem.EnemyGrid.Height; z++)
        {
            for (int x = 0; x < _gridSystem.EnemyGrid.Width; x++)
            {
                _gridSystem.EnemyGrid.InstantiatePrefab(_monsterPrefabPath, x, z, 180);
            }
        }
    }

    private void HandleMouseInput(MouseEvent mouseEvent)
    {
        if (!_gridSystem.TryGetGridInformation())
        {
            return;
        }

        switch (mouseEvent)
        {
            case MouseEvent.Click:
                _gridSystem.OnMouseLeftClick.Invoke();
                break;
        }
    }

    private void OnBattleStateChange(BattleState from, BattleState to)
    {   
        _gridSystem.OnBattleStateChange(from, to);
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
