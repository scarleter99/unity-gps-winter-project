using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ActionButton : UI_Base
{
    public Button Button { get; protected set; }
    
    public UI_BattleOrder UIBattleOrder { get; set; }
    
    public BaseAction Action { get; set; }
    
    public override void Init()
    {
        Button = gameObject.GetOrAddComponent<Button>();
        
        Button.onClick.AddListener(OnClickActionButton);
        gameObject.BindEvent(OnEnterActionButton, Define.UIEvent.Enter);
    }

    protected void OnClickActionButton()
    {
        UIBattleOrder.CurrentTurnHero.CurrentAction = Action;
        UIBattleOrder.CurrentTurnHero.CreatureBattleState = Define.CreatureBattleState.SelectTarget;
        
        UIBattleOrder.gameObject.SetActive(false);

        Managers.BattleMng.BattleState = Define.BattleState.SelectTarget;
    }
    
    protected void OnEnterActionButton(PointerEventData data)
    {
        bool isNoDamage = false;
        bool isNoPercentage = false;
        
        switch (Action.ActionAttribute)
        {
            case Define.ActionAttribute.Move:
                isNoDamage = true;
                isNoPercentage = true;
                break;
            case Define.ActionAttribute.Flee:
                isNoDamage = true;
                break;
        }
        
        UIBattleOrder.ShowActionInfo(Action, isNoDamage, isNoPercentage);
    }
}