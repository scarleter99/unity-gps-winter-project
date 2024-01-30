public class Define
{
    #region GameObjectType
    
    public enum WorldObject
    {
        Player,
        Monster,
        Unknown,
    }
    
    public enum WeaponType
    {
        NoWeapon,
        Bow,
        DoubleSword,
        SingleSword,
        Spear,
        SwordAndShield,
        TwoHandedSword,
        Wand
    }
    
    public enum ItemType
    {
        Attack,
        Buff,
        Debuff,
        Recover,
        None,
    }

    public enum ArmorType
    {
        Accessory,
        Body,
        Cloak,
        HeadAccessory,
        Helmet
    }
    
    public enum AreaTileType
    {   
        Invalid,
        Obstacle,
        Empty,
        Start,
        Normal,
        Battle,
        Encounter,
        Boss,
    }

    #endregion
    
    #region NonGameObjectType
    
    public enum ActionType
    {
        Attack,
        Defend,
        ItemUse,
        SkillUse
    }
    
    public enum GridSide
    {
        Player,
        Enemy
    }

    public enum WeaponSide
    {
        Left,
        Right
    }
    
    #endregion
    
    #region State
    
    public enum BattleState
    {
        Idle,
        SelectingTargetPlayer,
        SelectingTargetMonster,
        SelectingPlayerSideEmptyCell,
        ActionProceeding,
    }
    
    public enum AnimState
    {
        Attack,
        Defend,
        DefendHit,
        Die,
        Dizzy,
        Hit,
        Idle,
        JumpFront,
        JumpBack,
        Move,
        Skill,
        Victory
    }
    
    // 상태이상
    public enum Status
    {
        
    }
    
    public enum TurnState
    {
        Wait,
        Action,
        End,
        Dead
    }
    

    #endregion

    #region Event

    #region Name

    public enum AreaName
    {   
        Forest,
    }
   
    public enum MonsterName
    {
        Slime,
    }

    public enum ItemName
    {
        Sword,
    }

    #endregion

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    #endregion

    #region NonContent
    
    public enum Layer
    {
        Ground = 6,
        Block = 7,
        Monster = 8,
        Player = 9,
    }
    
    public enum CameraMode
    {
        QuarterView,
    }
    
    #endregion
    
    public enum Stat
    {
        Name,
        Hp,
        MaxHp,
        Attack,
        Defense,
        Speed,
        Gold,
        Dexterity,
        Strength,
        Vitality,
        Intelligence
    }
    
    public enum QuestReward
    {
        Money,
    }
    
    public enum Scene
    {
        UnknownScene,
        AreaScene,
        BattleScene,
        TestGameScene,
        TestTitleScene,
        TitleScene,
        TownScene,
    }
    
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }
}