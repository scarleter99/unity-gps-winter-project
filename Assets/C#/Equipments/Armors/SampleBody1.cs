public class SampleBody1 : Armor
{
    public SampleBody1(PlayerController equipper): base(equipper)
    {
        LoadDataFromJson(this.GetType().Name);

        _armorType = Define.ArmorType.Body; 
    }
}