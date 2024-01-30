public interface IStat
{
    public string Name { get; }
    public int Hp { get; set; }
    public int MaxHp { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public void OnDamage(int attackerAttack, int attackCount = 1);
    public void RecoverHp(int amount);
}