public class Define
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
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
        Skill,
        Victory
    }

    public enum TurnState
    {
        Wait,
        Action,
        End,
        Dead
    }

    public enum ItemType
    {
        Attack,
        Buff,
        Debuff,
        Recover,
        None,
    }

    public enum Status
    {
        
    }

    public enum Layer
    {
        Ground = 6,
        Block = 7,
        Monster = 8,
        Player = 9,
    }
    
    public enum Scene
    {
        UnknownScene,
        GameScene,
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

    public enum CameraMode
    {
        QuarterView,
    }
   
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

    public enum QuestReward
    {
        Money,
    }

    public enum GridOwner
    {
        Player,
        Enemy
    }
}