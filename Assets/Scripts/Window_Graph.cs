using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using Com.MyCompany.MyGame;

public class Window_Graph : MonoBehaviour {

    [SerializeField] private Sprite circleSpriteBlue;
    [SerializeField] private Sprite circleSpriteBlueHalf;
    [SerializeField] private Sprite circleSpriteGreen;
    private Sprite circleSprite;
    private RectTransform graphContainer;
    private List<double> pagoList, llegoList, valueList, valueList1;
    private FinalResults contar;

    private void Start()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        Debug.Log("Crea graphContainer");
        int pagoVerde = 0; //para el gráfico de porcentaje de pagados
        int llegoAzul = 1; //para el gráfico de porcentaje de llegados
        pagoList = new List<double>();
        llegoList = new List<double>();
        Debug.Log("Crea listas de pagados, llegadas y grafico");
        contar = new FinalResults();
        pagoList.AddRange(contar.ContarPorcentajes("pago"));
        llegoList.AddRange(contar.ContarPorcentajes("llega"));
        Debug.Log("Se llenan las de porcentaje");
        Debug.Log("Llama a verde");
        GraficosLineas(pagoList, llegoList, pagoVerde);
        Debug.Log("Llama a azul");
        GraficosLineas(pagoList, llegoList, llegoAzul);
    }

    public void GraficosLineas(List<double> pago, List<double> llego, int color) {
        Debug.Log("entra a GraficosLineas");
        valueList = new List<double>();
        valueList1 = new List<double>();
        int color1 = color;
        if (color1 == 0)
        {
            valueList = pago;
            valueList1 = llego;
        }
        else if (color1 == 1)
        {
            valueList = llego;
            valueList1 = pago;
        }

        for (int i = 0; i < valueList.Count; i++)
        {
            Debug.Log("Porcentaje sin convertir pago ");
            Debug.Log(pago[i]);
            Debug.Log("Porcentaje sin convertir llego ");
            Debug.Log(llego[i]);

            valueList[i] = ((double)valueList[i] * 240); //se transforma el porcentaje segun alto de container (240)
            Debug.Log("Porcentaje convertido pago ");
            Debug.Log(pago[i]);
            Debug.Log("Porcentaje convertido llego ");
            Debug.Log(llego[i]);
        }

        ShowGraph(valueList, valueList1, color1);
    }

    private GameObject CreateCircle(Vector2 anchoredPosition, int color, double valueList, double valueList1) {
        int color1 = color;
        if(color1==1)//1 es el gráfico azul, 0 es el verde
        {
            circleSprite = circleSpriteBlue;
            if (valueList == valueList1)
            {
                Debug.Log("son ambos iguales?");
                Debug.Log(valueList);
                Debug.Log(valueList1);
                circleSprite = circleSpriteBlueHalf;
            }
        }
        else if(color1==0)
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

    private void ShowGraph(List<double> valueList, List<double> valueList1, int color) {
        int color1 = color;
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 240f; //alto máximo de container eje y
        float xSize = 45f; //separación entre cada punto en eje x

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++) {
            float xPosition = xSize + i * xSize;
            float yPosition = ((float)valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), color1, valueList[i], valueList1[i]);
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
