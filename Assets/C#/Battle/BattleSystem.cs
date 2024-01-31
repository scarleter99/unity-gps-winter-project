using System.Collections.Generic;
using static Define;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{   
    [ReadOnly(false), SerializeField]
    private BattleState _battleState;
    private ulong[] _turns;
    private int _currentTurn;
    
    private Define.ActionType _actionType;
    private BaseController _actingEntity;
    
    public BattleState BattleState
    {
        get => _battleState;
        set
        {
            //Managers.RpcMng.BattleStateChangeServerRpc(value);
            var tmp = _battleState;
            _battleState = value;
            OnBattleStateChange(tmp, _battleState);
        }
    }
    public Define.ActionType ActionType { get => _actionType; set => _actionType = value; }
    public BaseController ActingEntity { get => _actingEntity; set => _actingEntity = value; }

    private BattleGridSystem _gridSystem;

    // 그리드 기준 위치 (그리드 좌표 0,0)
    [SerializeField]
    private Vector3 _playergridOriginPos; // 기본: (-3, 0.1, -4.75)
    [SerializeField]
    private Vector3 _enemygridOriginPos; // 기본: (-3, 0.1, 2.25)

    private const string _playerPrefabPath = "Players/Player";
    private const string _monsterPrefabPath = "Monsters/Bat";

    void Start()
    {
        Init();
    }

    private void Init()
    {
        // temp - for test
        _playergridOriginPos = new Vector3(-3f, 0.1f, -4.75f);
        _enemygridOriginPos = new Vector3(-3f, 0.1f, 2.25f);
        
        
        _gridSystem = new BattleGridSystem(_playergridOriginPos, _enemygridOriginPos);
        //BattleState = BattleState.Idle; // TODO - 아래 test code 없애면 주석 풀기
        Managers.InputMng.MouseAction -= HandleMouseInput;
        Managers.InputMng.MouseAction += HandleMouseInput;
        
        GeneratePrefabs();
        SetupPlayer();
        
        ////////////////////////////////////////////////
        // temp - for test
        BattleState = BattleState.SelectingTargetMonster;
        ActionType = Define.ActionType.Attack;
        ActingEntity = GameObject.Find("@Players").transform.GetChild(0).GetComponent<BaseController>();
        ////////////////////////////////////////////////
    }

    void Update()
    {
        _gridSystem.OnUpdate();
        //----------------------    
        // TODO - Test Code
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EndBattle();
        }
        //----------------------


    }

    private void GeneratePrefabs()
    {   
        // 현재는 테스트를 위해 그리드의 모든 셀에 프리팹을 생성하지만 추후 수정을 통해 특정 위치만 생성하도록 해야함.
        //for (int z = 0; z < _gridSystem.PlayerGrid.Height; z += 2) // temp - for test
        //{
        //    for (int x = 0; x < _gridSystem.PlayerGrid.Width; x++)
        //    {
        //        _gridSystem.PlayerGrid.SetupObject(Managers.GameMng.Spawn(WorldObject.Player, _playerPrefabPath), x, z);
        //    }
        //}
        for (int z = 0; z < _gridSystem.EnemyGrid.Height; z += 2) // temp - for test
        {
            for (int x = 0; x < _gridSystem.EnemyGrid.Width; x++)
            {
                _gridSystem.EnemyGrid.SetupObject(Managers.GameMng.Spawn(WorldObject.Monster, _monsterPrefabPath), x, z, 180);
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
                TriggerAction(_gridSystem.SelectedCell);
                break;
            case BattleState.SelectingTargetPlayer:
                break;
            case BattleState.SelectingTargetMonster:
                break;
            case BattleState.ActionProceeding:
                TriggerAction(_gridSystem.SelectedCell);
                break;
        }
    }
    
    // Attack, Heal, Skill etc. 핸들링 : XXXController로 형변환 후 각 함수 호출
    private void TriggerAction(SquareGridCell selectedCell)
    {
        switch (ActionType)
        {
            case Define.ActionType.Attack:
                ActingEntity.LockAndAttack(selectedCell.OnCellObject);
                // 만약 각 클래스에 구현된 함수라면
                // (ActingEntity as PlayerController)?.Attack();
                // (ActingEntity as MonsterController)?.Attack();
                break;
            case Define.ActionType.Defend:
                break;
            case Define.ActionType.ItemUse:
                break;
            case Define.ActionType.SkillUse:
                break;
        }
    }
    
    
    
    private void MakeTurn()
    {
        // TODO: 배틀 그리드 진입을 시도한 유저가 실행
    }

    private void NextTurn()
    {
        _currentTurn++;

        if (_currentTurn >= _turns.Length)
            _currentTurn = 0;
    }

    
    private void EndBattle()
    {
        Managers.InputMng.MouseAction -= HandleMouseInput;
        GameObject.FindObjectOfType<AreaScene>().UnloadBattleScene();
    }

    public void SetupPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int count = 0;

        for (int z = 0; z < _gridSystem.PlayerGrid.Height; z ++)
        {
            for (int x = 0; x < _gridSystem.PlayerGrid.Width; x++)
            {
                if (count == players.Length) break;
                _gridSystem.PlayerGrid.SetupObject(players[count], x, z);
                count++;
            }
        }

        //_gridSystem.PlayerGrid.SetupObject();
    }
}
