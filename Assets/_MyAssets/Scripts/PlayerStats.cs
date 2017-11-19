
[System.Serializable]
public class PlayerStats
{
    string accountName;
    float totalScore;

    public PlayerStats (string newAccountName, float newTotalScore)
    {
        accountName = newAccountName;
        totalScore = newTotalScore;
    }
}
