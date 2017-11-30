using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{

    public delegate void JustDestroyed(BreakablePlatform platform);
    public event JustDestroyed justDestroyed;
    public Sprite[] spriteStrengthImgs;
    public SpriteRenderer[] breakBlocks;
    public ParticleSystem breakParticles;
    public GameObject platform;

    public int currentHealth = 3;
	
    void Start()
    {
        foreach (SpriteRenderer s in breakBlocks)
        {
            s.sprite = spriteStrengthImgs[currentHealth - 1];
        }
    }
    public void TakeDamage()
    {
        currentHealth--;
        if(currentHealth <= 0)
        {
            BreakPlatform();
        }
        else
        {
            foreach(SpriteRenderer s in breakBlocks)
            {
                s.sprite = spriteStrengthImgs[currentHealth];
            }
        }
    }

    void BreakPlatform()
    {
        platform.SetActive(false);
        if (justDestroyed != null)
            justDestroyed(this);
        breakParticles.Play();
    }
}
