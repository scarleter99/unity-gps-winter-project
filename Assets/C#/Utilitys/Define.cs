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
        TestTitleScene,
        TestGameScene,
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
}