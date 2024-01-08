public class Define
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
    }
    
    public enum State
    {
        Die,
        Idle,
        Moving,
        Skill
    }
    public enum Layer
    {
        Ground = 6,
        Block = 7,
        Monster = 8,
    }
    
    public enum Scene
    {
        UnknownScene,
        TestTitleScene,
        TestGameScene,
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
}