public abstract class BaseSkill : BaseAction
{
    public Data.SkillData SkillData { get; protected set; }
    
    public int CoinNum { get; protected set; }
    public int ReducedStat { get; protected set; }

    public void SetInfo(int templateId)
    {
        DataId = templateId;
        
        SkillData = Managers.DataMng.SkillDataDict[templateId];
        
        CoinNum = SkillData.CoinNum;
        ReducedStat = SkillData.ReducedStat;
    }
}
