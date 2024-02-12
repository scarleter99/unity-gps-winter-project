public class Define
{
    #region Type
    public enum CreatureType
    {
        None,
        Hero,
        Monster,
    }
    
    public enum EquipmentType
    {
        None,
        Weapon,
        Armor,
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
        Wand,
    }
    
    public enum ArmorType
    {
        None,
        Accessory,
        Body,
        Cloak,
        HeadAccessory,
        Helmet,
    }
    
    public enum ItemType
    {
        None,
        Attack,
        Buff,
        Debuff,
        Recover,
    }
    
    public enum ActionType
    {
        None,
        MeleeAttack,
        RangedAttack,
        Buff,
        Heal,
        Move
    }
    
    public enum ActionTargetType
    {
        Single,
        Cross,
        Horizontal,
        Vertical,
    }
    
    public enum ApproachType
    {
        InPlace,
        Jump,
        Move,
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
    
    public enum QuestRewardType
    {
        Money,
    }
    
    // public enum SceneType
    public enum SceneType
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
    public enum SoundType
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
        Strength,
        Intelligence,
        Vitality,
        Dexterity,
    }
    
    public enum GridSide
    {
        HeroSide,
        MonsterSide,
    }

    public enum WeaponSide
    {
        Left,
        Right,
    }
    #endregion
    
    #region State
    public enum CreatureBattleState
    {
        Wait,
        Action,
        Dead
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
        Move,
        Skill,
        Victory
    }
    
    public enum BattleState
    {
        Init,
        MonsterTurn,
        SelectAction,
        SelectTarget,
        ActionProceed,
    }
    
    public enum AreaState
    {
        Idle,
        Moving,
        Battle,
        Encounter,
        Boss,
    }
    #endregion
    
    #region Event
    public enum UIEvent
    {
        Click,
        DoubleClick,
        Drag,
        Enter,
        Exit,
        Stay,
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }
    #endregion
    
    // Name은 나중에 Json으로 관리
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

    // DataId는 나중에 Json으로 관리
    #region DataId
    public const int HERO_KNIGHT_ID = 101000;

    public const int MONSTER_BAT_ID = 102000;
    
    public const int WEAPON_SAMPLESINGLESWORD_ID = 201000;
    public const int WEAPON_SAMPLESWORDANDSHIELD_ID = 201001;
    
    public const int ARMOR_SAMPLEBODY1_ID = 202000;
    public const int ARMOR_SAMPLEBODY2_ID = 202001;
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