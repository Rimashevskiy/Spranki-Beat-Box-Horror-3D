using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCube : MonoBehaviour
{
    [SerializeField] private int cubeSize = 10; // Размер большого куба
    [SerializeField] private int minBlockSize = 2; // Минимальный размер блока
    [SerializeField] private GameObject blockPrefab; // Префаб блока

    void Start()
    {
        // Создаём начальное пространство
        BoundsInt initialSpace = new BoundsInt(0, 0, 0, cubeSize, cubeSize, cubeSize);

        // Рекурсивно делим пространство
        List<BoundsInt> blocks = SubdivideSpace(initialSpace);

        // Создаём визуальные блоки
        foreach (BoundsInt block in blocks)
        {
            SpawnBlock(block);
        }
    }

    private List<BoundsInt> SubdivideSpace(BoundsInt space)
    {
        List<BoundsInt> result = new List<BoundsInt>();

        // Проверяем, можно ли дальше делить это пространство
        if (space.size.x <= minBlockSize && space.size.y <= minBlockSize && space.size.z <= minBlockSize)
        {
            result.Add(space); // Если пространство достаточно маленькое, добавляем его в результат
            return result;
        }

        // Выбираем случайную ось для деления
        int axis = Random.Range(0, 3); // 0 = X, 1 = Y, 2 = Z

        // Определяем точку деления
        int splitPoint;
        if (axis == 0 && space.size.x > minBlockSize * 2) // Делаем разрез по X
        {
            splitPoint = Random.Range(minBlockSize, space.size.x - minBlockSize);
            BoundsInt left = new BoundsInt(space.xMin, space.yMin, space.zMin, splitPoint, space.size.y, space.size.z);
            BoundsInt right = new BoundsInt(space.xMin + splitPoint, space.yMin, space.zMin, space.size.x - splitPoint, space.size.y, space.size.z);

            result.AddRange(SubdivideSpace(left));
            result.AddRange(SubdivideSpace(right));
        }
        else if (axis == 1 && space.size.y > minBlockSize * 2) // Делаем разрез по Y
        {
            splitPoint = Random.Range(minBlockSize, space.size.y - minBlockSize);
            BoundsInt bottom = new BoundsInt(space.xMin, space.yMin, space.zMin, space.size.x, splitPoint, space.size.z);
            BoundsInt top = new BoundsInt(space.xMin, space.yMin + splitPoint, space.zMin, space.size.x, space.size.y - splitPoint, space.size.z);

            result.AddRange(SubdivideSpace(bottom));
            result.AddRange(SubdivideSpace(top));
        }
        else if (axis == 2 && space.size.z > minBlockSize * 2) // Делаем разрез по Z
        {
            splitPoint = Random.Range(minBlockSize, space.size.z - minBlockSize);
            BoundsInt front = new BoundsInt(space.xMin, space.yMin, space.zMin, space.size.x, space.size.y, splitPoint);
            BoundsInt back = new BoundsInt(space.xMin, space.yMin, space.zMin + splitPoint, space.size.x, space.size.y, space.size.z - splitPoint);

            result.AddRange(SubdivideSpace(front));
            result.AddRange(SubdivideSpace(back));
        }
        else
        {
            // Если разрез невозможен, добавляем это пространство в результат
            result.Add(space);
        }

        return result;
    }

    private void SpawnBlock(BoundsInt bounds)
    {
        if (blockPrefab != null)
        {
            // Создаём блок в центре заданного пространства
            Vector3 center = bounds.center;
            Vector3 size = bounds.size;
            GameObject block = Instantiate(blockPrefab, center, Quaternion.identity, transform);
            block.transform.localScale = size;
        }
    }
}
