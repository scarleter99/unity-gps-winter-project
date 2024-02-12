using System.Collections.Generic;
using UnityEngine;

public class BattleManager
{
    public TurnSystem TurnSystem { get; protected set; }

    private Define.BattleState _battleState;
    public Define.BattleState BattleState
    {
        get => _battleState;
        protected set
        {
            switch (value)
            {
                case Define.BattleState.SelectAction:
                    CurrentTurnCreature.CreatureBattleState = Define.CreatureBattleState.Action;
                    // TODO - Battle Ui 가시화
                    break;
                case Define.BattleState.SelectTarget:
                    // TODO - BattleGridCell로 Target 선택 활성화
                    break;
                case Define.BattleState.MonsterTurn:
                    CurrentTurnCreature.CreatureBattleState = Define.CreatureBattleState.Action;
                    CurrentTurnCreature.DoAction(GetRandomCreature(Managers.ObjectMng.Heroes));
                    break;
            }
        }
    }

    public BattleGridCell[,] HeroGrid { get; protected set; } = new BattleGridCell[2, 3];
    public BattleGridCell[,] MonsterGrid { get; protected set; } = new BattleGridCell[2, 3];

    public Creature CurrentTurnCreature => TurnSystem.CurrentTurnCreature();
    public Creature TargetCreature;

    public void Init()
    {
        TurnSystem = new TurnSystem();
        
        Managers.InputMng.MouseAction -= HandleMouseInput;
        Managers.InputMng.MouseAction += HandleMouseInput;
    }
    
    #region InitBattle
    public void InitBattle()
    {
        GameObject battleGrid = Managers.ResourceMng.Instantiate("Battle/BattleGrid", null, "@BattleGrid");
        battleGrid.transform.position = Vector3.zero;
        GameObject heroSide = Util.FindChild(battleGrid, "HeroSide");
        GameObject monsterSide = Util.FindChild(battleGrid, "MonsterSide");

        for (int row = 0; row < 2; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                HeroGrid[row, col] = Util.FindChild<BattleGridCell>(heroSide, $"BattleGridCell ({row}, {col})");
                HeroGrid[row, col].SetRowCol(row, col);
                MonsterGrid[row, col] = Util.FindChild<BattleGridCell>(monsterSide, $"BattleGridCell ({row}, {col})");
                MonsterGrid[row, col].SetRowCol(row, col);
            }
        }
        
        _battleState = Define.BattleState.Init;
        
        PlaceAllCreatures();
        
        SetTurns();
        NextTurn(true);
    }
    
    private void PlaceAllCreatures()
    {
        // TODO - TEST CODE
        PlaceHero(10000, 0, 0);
        PlaceHero(10001, 0, 1);
        PlaceHero(10002, 1, 2);
        SpawnAndPlaceMonster(Define.MONSTER_BAT_ID, 0, 0);
        SpawnAndPlaceMonster(Define.MONSTER_BAT_ID, 0, 1);
        SpawnAndPlaceMonster(Define.MONSTER_BAT_ID, 1, 2);
    }
    
    public Hero PlaceHero(ulong heroId, int row, int col)
    {
        Hero hero = Managers.ObjectMng.Heroes[heroId];
        HeroGrid[row, col].CellCreature = hero;
        hero.transform.position = HeroGrid[row, col].transform.position;
        hero.Row = row;
        hero.Col = col;

        return hero;
    }
    
    public Monster SpawnAndPlaceMonster(int monsterDataId, int row, int col)
    {
        Monster monster = Managers.ObjectMng.SpawnMonster(monsterDataId);
        HeroGrid[row, col].CellCreature = monster;
        monster.transform.position = MonsterGrid[row, col].transform.position;
        
        // 180도 회전
        Vector3 currentRotation = monster.transform.rotation.eulerAngles;
        currentRotation.y += 180f;
        monster.transform.rotation = Quaternion.Euler(currentRotation);
        
        monster.Row = row;
        monster.Col = col;
        
        return monster;
    }
    
    private void SetTurns()
    {
        // TODO - 속도에 따른 코드로 수정 예정
        ulong[] turns = new ulong[100];
        int turnNum = 0;
        foreach (ulong id in Managers.ObjectMng.Heroes.Keys)
            turns[turnNum++] = id;
        foreach (ulong id in Managers.ObjectMng.Monsters.Keys)
            turns[turnNum++] = id;
        
        TurnSystem.Init(turns);
    }
    #endregion

    #region Battle
    public void NextTurn(bool isInit = false)
    {
        if (isInit == false)
            TurnSystem.NextTurn();
        
        switch (CurrentTurnCreature.CreatureType)
        {
            case Define.CreatureType.Hero:
                BattleState = Define.BattleState.SelectAction;
                break;
            case Define.CreatureType.Monster:
                BattleState = Define.BattleState.MonsterTurn;
                break;
        }
    }
    
    private void HandleMouseInput(Define.MouseEvent mouseEvent)
    {
        switch (mouseEvent)
        {
            case Define.MouseEvent.Click:
                if (BattleState == Define.BattleState.SelectTarget)
                    ClickGridCell();
                break;
            // TODO - MouseHOver시 색바뀌기 구현
        }
    }
    
    public bool ClickGridCell()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit rayHit, maxDistance: 100f, layerMask: LayerMask.GetMask("BattleGridCell")))
        {
            BattleGridCell gridCell = rayHit.transform.gameObject.GetComponent<BattleGridCell>();
            
            Hero currentTurnHero = CurrentTurnCreature as Hero;
            TargetCreature = gridCell.CellCreature;

            if (currentTurnHero != null) 
                currentTurnHero.DoAction(TargetCreature.Id);
            else
                Debug.Log("Failed to ClickGirdCell");

            // TODO - DEBUG CODE
            Debug.DrawLine(Camera.main.transform.position, rayHit.point);
            
            return true;
        }

        return false;
    }
    
    ulong GetRandomCreature(Dictionary<ulong, Hero> dictionary)
    {
        List<ulong> keysList = new List<ulong>(dictionary.Keys);
        
        int randomIndex = Random.Range(0, keysList.Count);
        ulong randomKey = keysList[randomIndex];

        return randomKey;
    }
    #endregion
    
    #region Prev GridCell Color Code
    /*
    // 셀 나타내는 사각형 스프라이트
    // 그리드의 y좌표를 지면보다 약간 높게 설정해야 보임
    private SpriteRenderer _indicator;
    private Color _originalColor;
    
    // 마우스를 그리드 셀 위로 가져다 대면 해당 셀 색을 바꿈
    public void HandleMouseHover(Vector3 worldPosition)
    {
        GetGridPosition(worldPosition, out int x, out int z);
        //Debug.Log($"{z}, {x}");
        if (x >= 0 && x < _width && z >= 0 && z < _height)
        {
            _currentMouseoverCell?.OnMouseExit();
            _currentMouseoverCell = _gridArray[z, x];
            _currentMouseoverCell?.OnMouseEnter();
        }
        else
        {
            ResetMouseHover();
        }
    }
    
    public void ResetMouseHover()
    {
        _currentMouseoverCell?.OnMouseExit();
        _currentMouseoverCell = null;
    }
    
    public void OnMouseEnter()
    {
        if (_side == Define.GridSide.HeroSide)
        {
            ChangeColor(Color.green);
        }
        else if (_side == Define.GridSide.MonsterSide)
        {
            ChangeColor(Color.red);
        }
    }

    private void ChangeColor(Color color, float duration = 0.3f)
    {
        KillColorTween();
        _colorTween = _indicator.DOColor(color, duration).OnComplete(() => { _colorTween = null; });
    }

    public void OnMouseExit()
    {   
        ChangeColor(_originalColor);
    }

    // 기존 진행중인 colorTween을 중지, 삭제
    private void KillColorTween()
    {
        _colorTween?.Kill();
        _colorTween = null;
    }
    */
    #endregion
}
