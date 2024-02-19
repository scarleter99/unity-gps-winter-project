using System.Collections;
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
                    //CurrentTurnCreature.DoAction(GetRandomCreature(Managers.ObjectMng.Heroes));
                    break;
            }
        }
    }
    public Creature CurrentTurnCreature => TurnSystem.CurrentTurnCreature();

    public BattleGridCell[,] HeroGrid { get; protected set; } = new BattleGridCell[2, 3];
    public BattleGridCell[,] MonsterGrid { get; protected set; } = new BattleGridCell[2, 3];

    public BattleGridCell CurrentMouseOverCell { get; protected set; }

    public void Init()
    {
        TurnSystem = new TurnSystem();
        
        Managers.InputMng.MouseAction -= HandleMouseInput;
        Managers.InputMng.MouseAction += HandleMouseInput;
    }
    
    private void HandleMouseInput(Define.MouseEvent mouseEvent)
    {
        switch (mouseEvent)
        {
            case Define.MouseEvent.Click:
                if (BattleState == Define.BattleState.SelectTarget && OnMouseOverCell())
                    OnClickGridCell();
                break;
            case Define.MouseEvent.Hover:
                OnMouseOverCell();
                break;
        }
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
                HeroGrid[row, col].SetRowCol(row, col, Define.GridSide.HeroSide);
                MonsterGrid[row, col] = Util.FindChild<BattleGridCell>(monsterSide, $"BattleGridCell ({row}, {col})");
                MonsterGrid[row, col].SetRowCol(row, col, Define.GridSide.HeroSide);
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

    public void ReplaceCreature(Creature creature, BattleGridCell cell)
    {
        if (cell.GridSide == Define.GridSide.HeroSide) 
            HeroGrid[creature.Row, creature.Col] = null;
        else
            MonsterGrid[creature.Row, creature.Col] = null;
        cell.CellCreature = creature;
    }
    
    private bool OnMouseOverCell()
    {
        CurrentMouseOverCell?.MouseExit();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit rayHit, maxDistance: 100f, layerMask: LayerMask.GetMask("BattleGridCell")))
        {
            BattleGridCell gridCell = rayHit.transform.gameObject.GetComponent<BattleGridCell>();

            CurrentMouseOverCell = gridCell;

            if (gridCell != null)
            {
                CurrentMouseOverCell.MouseEnter();
                return true;
            }
        }

        CurrentMouseOverCell = null;
        
        return false;
    }
    
    public void OnClickGridCell()
    {
        if (CurrentMouseOverCell == null)
            return;
        
        Hero currentTurnHero = CurrentTurnCreature as Hero;
        if (currentTurnHero != null) 
            currentTurnHero.DoAction(CurrentMouseOverCell);
        else
            Debug.Log("No currentTurnHero!");
    }
    
    public Creature GetCreatureByRowCol(int row, int col, Define.GridSide gridSide)
    {
        if (gridSide == Define.GridSide.HeroSide)
        {
            return HeroGrid[row, col].CellCreature;
        }
        else
        {
            return MonsterGrid[row, col].CellCreature;
        }
    }
    
    public ulong GetRandomCreature(Dictionary<ulong, Hero> dictionary)
    {
        List<ulong> keysList = new List<ulong>(dictionary.Keys);
        
        int randomIndex = Random.Range(0, keysList.Count);
        ulong randomKey = keysList[randomIndex];

        return randomKey;
    }
  
    
    #endregion
}
