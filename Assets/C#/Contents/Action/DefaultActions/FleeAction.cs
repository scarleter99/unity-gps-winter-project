using UnityEngine;

public class FleeAction : BaseAction
{
    public int CoinNum { get; set; }
    
    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);
            
        ActionTargetType = Define.ActionTargetType.Single;
    }

    public override void DoAction()
    {
        // TODO
    }
    
    public override void OnHandleAction()
    {
        // TODO
    }
}