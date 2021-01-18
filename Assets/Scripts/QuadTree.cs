using System.Collections.Generic;
using UnityEngine;

public class QuadTree
{
    List<Rect> objectList;
    Rect bounds;
    int maxLevel = 5;
    int maxObjects = 1;

    QuadTree[] children;
    float width;
    float height;
    int level;
    bool divided;

    public QuadTree(int level, Rect bounds)
    {
        this.level = level;
        this.bounds = bounds;
        this.objectList = new List<Rect>();
        children = new QuadTree[4];
        width = bounds.max.x - bounds.max.x;
        height = bounds.max.y - bounds.max.y;
    }

    public void Clear()
    {
        objectList.Clear();

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i] != null)
            {
                children[i].Clear();
                children[i] = null;
            }
        }
    }

    void Split()
    {
        int subWidth = (int)(bounds.width / 2);
        int subHeight = (int)(bounds.height / 2);
        int x = (int)bounds.x;
        int y = (int)bounds.y;

        children[0] = new QuadTree(level + 1, new Rect(x + subWidth, y, subWidth, subHeight));
        children[1] = new QuadTree(level + 1, new Rect(x, y, subWidth, subHeight));
        children[2] = new QuadTree(level + 1, new Rect(x, y + subHeight, subWidth, subHeight));
        children[3] = new QuadTree(level + 1, new Rect(x + subWidth, y + subHeight, subWidth, subHeight));

        divided = true;
        ShowBoundries();
    }

    int GetIndex(Rect pRect)
    {
        int index = -1;

        float verticalMidpoint = bounds.x + bounds.width / 2;
        float horizontalMidpoint = bounds.y + bounds.height / 2;

        // Top quadrant
        bool topQuadrant = (pRect.y < horizontalMidpoint && pRect.y + pRect.height < horizontalMidpoint);

        // Bottom quadrant
        bool bottomQuadrant = (pRect.y > horizontalMidpoint);

        // Left quadrant
        if (pRect.x < verticalMidpoint && pRect.x + pRect.width < verticalMidpoint)
        {
            // Top-left
            if (topQuadrant)
                index = 1;
            // Bottom-left
            else if (bottomQuadrant)
                index = 2;
        }
        // Right quadrant
        else if (pRect.x > verticalMidpoint)
        {
            // Top-right
            if (topQuadrant)
                index = 0;
            // Bottom-right
            else if (bottomQuadrant)
                index = 3;
        }

        return index;
    }

    public void Insert(Rect pRect)
    {
        if (children[0] != null)
        {
            int index = GetIndex(pRect);

            if (index != -1)
            {
                children[index].Insert(pRect);

                return;
            }
        }

        objectList.Add(pRect);

        if (objectList.Count > maxObjects && level < maxLevel)
        {
            if (children[0] == null)
                Split();

            int i = 0;
            while (i < objectList.Count)
            {
                int index = GetIndex(objectList[i]);
                if (index != -1)
                {
                    children[index].Insert(objectList[i]);
                    objectList.RemoveAt(i);
                }
                else
                    i++;
            }
        }
    }

    public void ShowBoundries()
    {
        float x = bounds.x;
        float y = bounds.y;
        float w = bounds.width;
        float h = bounds.height;

        Vector2 bottomLeftPoint = new Vector2(x, y);
        Vector2 bottomRightPoint = new Vector2(x + w, y);
        Vector2 topRightPoint = new Vector2(x + w, y + h);
        Vector2 topLeftPoint = new Vector2(x, y + h);

        Debug.DrawLine(bottomLeftPoint, bottomRightPoint, Color.red, 100f);   //bottomLine
        Debug.DrawLine(bottomLeftPoint, topLeftPoint, Color.red, 100f);       //leftLine
        Debug.DrawLine(bottomRightPoint, topRightPoint, Color.red, 100f);     //rightLine
        Debug.DrawLine(topLeftPoint, topRightPoint, Color.red, 100f);         //topLine

        if (divided)
        {
            children[0].ShowBoundries();
            children[1].ShowBoundries();
            children[2].ShowBoundries();
            children[3].ShowBoundries();
        }
    }

    public void ShowBound(float x, float y, float h, float w)
    {
        Vector2 bottomLeftPoint = new Vector2(x - w, y - h);
        Vector2 bottomRightPoint = new Vector2(x + w, y - h);
        Vector2 topRightPoint = new Vector2(x + w, y + h);
        Vector2 topLeftPoint = new Vector2(x - w, y + h);

        Debug.DrawLine(bottomLeftPoint, bottomRightPoint, Color.green, 50f);   //bottomLine
        Debug.DrawLine(bottomLeftPoint, topLeftPoint, Color.green, 50f);       //leftLine
        Debug.DrawLine(bottomRightPoint, topRightPoint, Color.green, 50f); //rightLine
        Debug.DrawLine(topLeftPoint, topRightPoint, Color.green, 50f);     //topLine
    }
}
