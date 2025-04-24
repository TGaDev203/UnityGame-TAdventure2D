public class PlayerData
{
    public string[] collectedCoinIDs;
    public string[] collectedHealthPickupIDs;
    public float currentHealth;
    public int coin;
    public float positionX;
    public float positionY;

    public PlayerData(float x, float y, float health, int coinAmount, string[] collectedCoins, string[] collectedHealths)
    {
        positionX = x;
        positionY = y;
        currentHealth = health;
        coin = coinAmount;
        collectedCoinIDs = collectedCoins;
        collectedHealthPickupIDs = collectedHealths;
    }
}