using System.Collections;
using System.Collections.Generic;
using Data;
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
                    break;
                case Define.BattleState.SelectTarget:
                    // BattleGridCell로 Target 선택 활성화
                    break;
                case Define.BattleState.MonsterTurn:
                    CurrentTurnCreature.CreatureBattleState = Define.CreatureBattleState.Action;
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
    }
    
    #region InitBattle
    public void InitBattle(int monsterSquadDataId)
    {
        Managers.InputMng.MouseAction += HandleMouseInput;
        
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
        
        PlaceAllCreatures(monsterSquadDataId);
        
        SetBattleTurns();
        NextTurn(true);
    }
    
    private void PlaceAllCreatures(int monsterSquadDataId)
    {
        // TODO - TEST CODE
        PlaceHero(10000, HeroGrid[0, 0]);
        PlaceHero(10001, HeroGrid[0, 1]);
        PlaceHero(10002, HeroGrid[1, 2]);
        
        MonsterSquadData monsterSquadData = Managers.DataMng.MonsterSquadDataDict[monsterSquadDataId];
        int line1Col = 0;
        int line2Col = 0;
        foreach (int monsterId in monsterSquadData.Line1)
        {
            SpawnAndPlaceMonster(monsterId, MonsterGrid[0, line1Col++]);
        }
        foreach (int monsterId in monsterSquadData.Line2)
        {
            SpawnAndPlaceMonster(monsterId, MonsterGrid[1, line2Col++]);
        }
    }
    
    public Hero PlaceHero(ulong heroId, BattleGridCell targetCell)
    {
        Hero hero = Managers.ObjectMng.Heroes[heroId];
        
        MoveCreature(hero, targetCell, true);

        return hero;
    }
    
    public Monster SpawnAndPlaceMonster(int monsterDataId, BattleGridCell targetCell)
    {
        Monster monster = Managers.ObjectMng.SpawnMonster(monsterDataId);

        Vector3 currentRotation = monster.transform.rotation.eulerAngles;
        currentRotation.y += 180f;
        monster.transform.rotation = Quaternion.Euler(currentRotation);
        
        MoveCreature(monster, targetCell, true);
        
        return monster;
    }
    
    private void SetBattleTurns()
    {
        // TODO - 속도에 따른 코드로 수정 예정
        ulong[] turns = new ulong[100];
        int turnNum = 0;
        foreach (ulong id in Managers.ObjectMng.Heroes.Keys)
            turns[turnNum++] = id;
        foreach (ulong id in Managers.ObjectMng.Monsters.Keys)
            turns[turnNum++] = id;
        
        TurnSystem.Init(turns);
        // TODO - 디버깅 코드
        Debug.Log(turns);
    }
    #endregion

    #region Input
    private void HandleMouseInput(Define.MouseEvent mouseEvent)
    {
        switch (mouseEvent)
        {
            case Define.MouseEvent.Hover:
                OnMouseOverCell();
                break;
            case Define.MouseEvent.Click:
                if (BattleState == Define.BattleState.SelectTarget && OnMouseOverCell())
                    OnClickGridCell();
                break;
        }
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
            currentTurnHero.CreatureBattleState = Define.CreatureBattleState.Action;
        else
            Debug.Log("No currentTurnHero!");
    }

    #endregion
    
    #region Battle
    public void MoveCreature(Creature creature, BattleGridCell targetCell, bool isPlace = false)
    {
        if (creature.Cell != null)
            creature.Cell.CellCreature = null;
        
        targetCell.CellCreature = creature;
        creature.Cell = targetCell;

        if (isPlace)
            creature.transform.position = targetCell.transform.position;
    }
    
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
    #endregion

    public void EndBattle()
    {
        Managers.InputMng.MouseAction -= HandleMouseInput;
    }
}
