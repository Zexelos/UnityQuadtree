using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] List<Rect> allObjects;

    QuadTree quadTree;

    void Start()
    {
        quadTree = new QuadTree(0, new Rect(Vector2.zero, new Vector2(20, 20)));
        quadTree.ShowBound(0, 0, 20, 20);
    }

    void Update()
    {
        quadTree.Clear();

        ClickToAddPoint();
    }

    void ClickToAddPoint()
    {
        if (Input.GetMouseButtonDown(0))
        {
            allObjects.Add(new Rect(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)), new Vector2(0.2f, 0.2f)));

            for (int i = 0; i < allObjects.Count; i++)
            {
                quadTree.Insert(allObjects[i]);
            }
        }
    }
    void OnDrawGizmos()
    {
        foreach (var item in allObjects)
        {
            Gizmos.DrawCube(new Vector3(item.x + item.width / 2, item.y + item.height / 2, 0), new Vector3(0.2f, 0.2f, 0f));
        }
    }
}

