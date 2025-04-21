public class PlayerData
{
    public string[] collectedCoinIDs;
    public float positionX;
    public float positionY;
    public int currentHealth;
    public int coin;

    public PlayerData(float x, float y, int health, int coinAmount, string[] collectedCoins)
    {
        positionX = x;
        positionY = y;
        currentHealth = health;
        coin = coinAmount;
        collectedCoinIDs = collectedCoins;
    }
}