using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum FruitType
{
    apple,
    bananas,
    cherry,
    kiwi,
    melon,
    orange,
    pineapple,
    strawberry
}

public class Fruit_Item : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected SpriteRenderer sr;
    public FruitType myFruitType;
    [SerializeField] private Sprite[] fruitImage;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            PlayerManager.instance.fruits++;
            AudioManager.instance.PlaySFX(1);
            Destroy(gameObject);
        }
    }

    

    public void FruitSetup(int fruitIndex)
    {
        for (int i = 0; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(fruitIndex, 1);
    }

    //private void OnValidate()
    //{
    //    sr.sprite = fruitImage[((int)myFruitType)];
    //}
}
