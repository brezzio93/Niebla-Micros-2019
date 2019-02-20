﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Window_Graph : MonoBehaviour {

    [SerializeField] private Sprite circleSpriteBlue;
    [SerializeField] private Sprite circleSpriteGreen;
    private Sprite circleSprite;
    private RectTransform graphContainer;

    public void GraficosLineas(List<double> valueList, int color) {
        
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        int color1 = color;
        for (int i=0; i<=9; i++)
        {
            valueList[i] = (double)(valueList[i] * 240) / 100; //se transforma el porcentaje segun alto de container (240)
        }
        ShowGraph(valueList, color1);
    }

    private GameObject CreateCircle(Vector2 anchoredPosition, int color) {
        int color1 = color;
        if(color1==1)//1 es el gráfico azul, 0 es el verde
        {
            circleSprite = circleSpriteBlue;   
        }else if(color1==0)
        {
            circleSprite = circleSpriteGreen;
        }

        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<double> valueList, int color) {
        int color1 = color;
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 240f; //alto máximo de container eje y
        float xSize = 45f; //separación entre cada punto en eje x

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++) {
            float xPosition = xSize + i * xSize;
            float yPosition = ((float)valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), color1);
            if (lastCircleGameObject != null) {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition, color1);
            }
            lastCircleGameObject = circleGameObject;
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB, int color) { //crea la linea de conexion entre 2 puntos
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        if(color==1)
        { 
        gameObject.GetComponent<Image>().color = new Color((float)0.4509804, (float)0.4, 1, .5f); //si color es 1(azul),las conexiones son azules
        }else if(color==0)
        {
            gameObject.GetComponent<Image>().color = new Color(0, (float)0.8431373, (float)0.4196079, .5f);//si color es 0(verde),las conexiones son verdes
        }

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
