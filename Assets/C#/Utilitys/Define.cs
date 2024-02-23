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
        Destroyed
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
    
    public enum ActionAttribute
    {
        None,
        TauntSkill,
        AttackSkill,
        BuffSkill,
        DebuffSkill,
        HealSkill,
        HealItem,
        BuffItem,
        DebuffItem,
        AttackItem,
        Move,
        Flee,
        SelectBag
    }
    
    public enum GridSide
    {
        HeroSide,
        MonsterSide,
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
        Hover,
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
    
    #region Path
    public const string HERO_PATH = "Heroes";
    public const string MONSTER_PATH = "Monsters";
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

    public const int ITEM_HEALPOTION_ID = 301000;
    
    public const int SKILL_STRIKE_ID = 401000;
    #endregion

    public const float MOVE_SPEED = 5f;
}