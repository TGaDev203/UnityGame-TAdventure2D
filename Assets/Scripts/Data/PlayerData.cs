public class PlayerData
{
    public float positionX;
    public float positionY;
    public int currentHealth;
    public int coin;

    public PlayerData(float x, float y, int health, int coinAmount)
    {
        positionX = x;
        positionY = y;
        currentHealth = health;
        coin = coinAmount;
    }
}