public class PlayerData
{
    public string[] collectedCoinIDs;
    public float currentHealth;
    public int coin;
    public float positionX;
    public float positionY;

    public PlayerData(float x, float y, float health, int coinAmount, string[] collectedCoins)
    {
        positionX = x;
        positionY = y;
        currentHealth = health;
        coin = coinAmount;
        collectedCoinIDs = collectedCoins;
    }
}