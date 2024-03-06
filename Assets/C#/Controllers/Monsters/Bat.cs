public class Bat : Monster
{
    public override void SetInfo(int templateId)
    {
        ApproachType = Define.ApproachType.Move;
        base.SetInfo(templateId);
    }
}
