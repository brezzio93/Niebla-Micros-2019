using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class linegraph : MonoBehaviour
{

    [SerializeField] private Sprite circleSprite; //puntos de grafico
    private RectTransform GraphContainer;

    private void Awake()
    {
        GraphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();

        List<int> valueList = new List<int>() { 150, 69, 124, 102, 90, 114, 74, 55, 95, 108 }; //valores de prueba
        ShowGraph(valueList);
    }

    private GameObject CreateCircle(Vector2 anchoredPosition) //posicion de puntos
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(GraphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        gameObject.GetComponent<Image>().color = new Color((float)0.4509804, (float)0.4, (float)1); //color de circulo
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<int> valueList)//recibe lista de valores a graficar
    {
        float graphHeight = GraphContainer.sizeDelta.y;
        float yMaximum = 250f; //valor máximo de y
        float xSize = 50f; //distancia entre cada punto del eje x

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) //lineas de coexion entre puntos. Para llamar a esta funcion se necesita la posicion del primer y segundo punto
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(GraphContainer, false);
        gameObject.GetComponent<Image>().color = new Color((float)0.4509804, (float)0.4, (float)1, (float).5f); //color de linea
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

}

