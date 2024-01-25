public class SampleBody2 : Armor
{
    public SampleBody2(PlayerController equipper): base(equipper)
    {
        LoadDataFromJson(this.GetType().Name);

        _armorType = Define.ArmorType.Body; 
    }
}