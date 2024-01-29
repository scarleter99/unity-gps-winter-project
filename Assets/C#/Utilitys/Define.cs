public class Define
{
    #region Type
    
    //public enum CreatureType
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
    
    public enum ArmorType
    {
        Accessory,
        Body,
        Cloak,
        HeadAccessory,
        Helmet
    }
    
    public enum ItemType
    {
        Attack,
        Buff,
        Debuff,
        Recover,
        None,
    }
    
    public enum ActionType
    {
        Attack,
        Defend,
        ItemUse,
        SkillUse
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
    
    // public enum QuestRewardType
    public enum QuestReward
    {
        Money,
    }
    
    // public enum SceneType
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
    
    // public enum SoundType
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }
    #endregion
    
    #region Attribute
    
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
    
    public enum AnimState
    {
        Attack,
        Defend,
        DefendHit,
        Die,
        Dizzy,
        Hit,
        Idle,
        Skill,
        Victory
    }
    
    public enum BattleState
    {
        Idle,
        SelectingTargetPlayer,
        SelectingTargetMonster,
        SelectingPlayerSideEmptyCell,
        ActionProceeding,
    }
    
    public enum TurnState
    {
        Wait,
        Action,
        End,
        Dead
    }
    
    public enum BattleManagerTurnState
    {

    }
    
    public enum CreatureTurnState
    {

    }
    

    #endregion
    
    // Name은 Json으로 관리
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
    
    #region Event
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
    

}