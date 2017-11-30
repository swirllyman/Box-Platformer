using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialVictoryChest : VictoryChest
{
    public enum SpecialChestContainer { DoubleJump, AirDash, FloatJump }

    public SpecialChestContainer myContainer;
    public string powerupName = "DoubleJump";
    public GameObject specialVFX;

    protected override void EndLevel(Player p)
    {
        base.EndLevel(p);
        GameObject g = Instantiate(specialVFX, transform.position, Quaternion.identity);
        g.transform.parent = transform;
        p.SetupDoubleJump();
        StartCoroutine(RegisterPowerup(p.username));
    }

    IEnumerator RegisterPowerup(string username)
    {
        print("Registering powerup");
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("powerUpNamePost", powerupName);
        WWW www = new WWW("https://shipment.000webhostapp.com/InsertPlayerPowerup.php", form);

        yield return www;
        print(www.text);
    }
}
