using System;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    [SerializeField] private Transform[] fruitPosition;
    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private bool randomFruits;

    private int fruitIndex;
    // Start is called before the first frame update
    void Start()
    {
        fruitPosition = GetComponentsInChildren<Transform>();

        for (int i = 1; i < fruitPosition.Length; i++)
        {
            GameObject newFruit = Instantiate(fruitPrefab, fruitPosition[i]);

            if (randomFruits)
            {
                fruitIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(FruitType)).Length);
                newFruit.GetComponent<Fruit_Item>().FruitSetup(fruitIndex);

            }
            else
            {
                newFruit.GetComponent<Fruit_Item>().FruitSetup(fruitIndex);
                fruitIndex++;

                if (fruitIndex > Enum.GetNames(typeof(FruitType)).Length)
                    fruitIndex = 0;
            }

            fruitPosition[i].GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}
