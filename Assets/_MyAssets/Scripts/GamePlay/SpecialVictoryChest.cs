using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialVictoryChest : VictoryChest
{
    public enum SpecialChestContainer { DoubleJump, AirDash, FloatJump }

    public SpecialChestContainer myContainer;
    public string powerupName = "DoubleJump";
    public GameObject specialVFX;
    public ParticleSystem confettiParticles2;
    public float initialOffset = 1.0f;
    public float confettiBurstTime2 = 2.0f;
    float confettiTimer2;

    protected override void Start()
    {
        base.Start();
        confettiTimer2 = confettiBurstTime2;
    }

    protected override void Update()
    {
        base.Update();

        if(initialOffset > 0.0f)
        {
            initialOffset -= Time.deltaTime;
        }
        else
        {
            if (confettiTimer2 > 0.0f)
            {
                confettiTimer2 -= Time.deltaTime;
            }
            else
            {
                confettiTimer2 = confettiBurstTime2;
                confettiParticles2.Play();
                confettiAudio.Play();
            }
        }
    }

    protected override void EndLevel(Player p)
    {
        base.EndLevel(p);
        GameObject g = Instantiate(specialVFX, transform.position, Quaternion.identity);
        g.transform.parent = transform;
        p.SetupDoubleJump();
        StartCoroutine(RegisterPowerup(p.username));
    }

    #region DB Calls
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

    #endregion
}
