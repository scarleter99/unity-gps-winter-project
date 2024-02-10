public class Knight : Hero
{
    protected override void Init()
    {
        base.Init();
        
        // TODO - TEST CODE, 나중에 Skill 별로 ApproachType 관리...?
        _approachType = Define.ApproachType.Move;
    }
}
