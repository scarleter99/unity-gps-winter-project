using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BattleManager
{
    public TurnSystem TurnSystem { get; protected set; }
    public Define.BattleState BattleState { get; protected set; }
    
    public BattleGridCell[,] HeroGrid = new BattleGridCell[3,2];
    public BattleGridCell[,] MonsterGrid = new BattleGridCell[3,2];

    public Creature CurrentTurnCreature => TurnSystem.CurrentTurnCreature();
    public Creature TargetCreature;

    public void Init()
    {
        Managers.InputMng.MouseAction -= HandleMouseInput;
        Managers.InputMng.MouseAction += HandleMouseInput;
    }

    public void InitBattle()
    {
        GameObject battleGrid = Managers.ResourceMng.Instantiate("Battle/BattleGrid", null, "@BattleGrid");
        GameObject heroSide = Util.FindChild(battleGrid, "HeroSide");
        GameObject monsterSide = Util.FindChild(battleGrid, "MonsterSide");

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                HeroGrid[row, col] = Util.FindChild<BattleGridCell>(heroSide, $"BattleGridCell ({row}, {col})");
                HeroGrid[row, col].SetRowCol(row, col);
                MonsterGrid[row, col] = Util.FindChild<BattleGridCell>(monsterSide, $"BattleGridCell ({row}, {col})");
                MonsterGrid[row, col].SetRowCol(row, col);
            }
        }


        SpawnCreatures();
        SetTurns();
    }

    private void SpawnCreatures()
    {
        // TODO - TEST CODE
        SpawnHero(1000, 0, 0);
        SpawnHero(1001, 1, 0);
        SpawnHero(1002, 2, 1);
        SpawnMonster(Define.MONSTER_BAT_ID, 0, 0);
        SpawnMonster(Define.MONSTER_BAT_ID, 1, 0);
        SpawnMonster(Define.MONSTER_BAT_ID, 2, 1);
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

    public void NextTurn()
    {
        TurnSystem.NextTurn();
        
        Creature currentTurnCreature = TurnSystem.CurrentTurnCreature();
        switch (currentTurnCreature.CreatureType)
        {
            case Define.CreatureType.Hero:
                BattleState = Define.BattleState.SelectAction;
                break;
            case Define.CreatureType.Monster:
                BattleState = Define.BattleState.MonsterTurn;
                break;
        }
    }

    public Hero SpawnHero(ulong heroId, int row, int col)
    {
        Hero hero = Managers.ObjectMng.Heroes[heroId];
        HeroGrid[row, col].CellCreature = hero;
        hero.transform.position = HeroGrid[row, col].transform.position;
        hero.Row = row;
        hero.Col = col;

        return hero;
    }
    
    public Monster SpawnMonster(int monsterDataId, int row, int col)
    {
        Monster monster = Managers.ObjectMng.Spawn<Monster>(monsterDataId);
        HeroGrid[row, col].CellCreature = monster;
        monster.transform.position = HeroGrid[row, col].transform.position;
        monster.Row = row;
        monster.Col = col;
        
        return monster;
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
            TargetCreature = gridCell.CellCreature;
            
            Hero currentHero = CurrentTurnCreature as Hero;
            currentHero.DoAction(TargetCreature.Id);
            
            // TODO - DEBUG CODE
            Debug.DrawLine(Camera.main.transform.position, rayHit.point);
            
            return true;
        }

        return false;
    }

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
