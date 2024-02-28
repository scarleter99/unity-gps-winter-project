public abstract class BaseSkill : BaseAction
{
    public int DataId { get; protected set; }
    public Data.SkillData SkillData { get; protected set; }
    
    public int CoinNum { get; protected set; }
    public int ReducedStat { get; protected set; }

    public virtual void SetInfo(int templateId, Creature owner)
    {
        DataId = templateId;
        SkillData = Managers.DataMng.SkillDataDict[templateId];
        
        Owner = owner;
        CoinNum = SkillData.CoinNum;
        ReducedStat = SkillData.ReducedStat;
    }
}
