using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] List<Rect> allObjects;

    QuadTree quadTree;

    void Start()
    {
        quadTree = new QuadTree(0, new Rect(0, 0, 20, 20));
        quadTree.ShowBound(0, 0, 20, 20);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            allObjects.Add(new Rect(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)), new Vector2(0.2f, 0.2f)));
        }

        quadTree.Clear();

        for (int i = 0; i < allObjects.Count; i++)
        {
            quadTree.Insert(allObjects[i]);
        }
    }
}
