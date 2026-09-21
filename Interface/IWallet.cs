
public interface IWallet
{
    public int Gold { get; }
    public void AddGold(int amount);
    public bool TrySpendGold(int amount);
}
