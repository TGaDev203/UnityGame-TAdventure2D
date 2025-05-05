public class PlayerData
{
    public float currentHealth;
    public float positionX;
    public float positionY;
    public string[] collectedCoinIDs;
    public string[] collectedHealthPickupIDs;
    public int coin;

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