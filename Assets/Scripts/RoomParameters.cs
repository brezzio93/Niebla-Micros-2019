﻿using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    [RequireComponent(typeof(InputField))]
    public class RoomParameters : MonoBehaviour
    {
        [SerializeField]
        private InputField monto, precio, ganancia, cantidad;

        [SerializeField]
        private Text Warning;

        private ExitGames.Client.Photon.Hashtable CustomProps = new ExitGames.Client.Photon.Hashtable();

        /// <summary>
        /// Estructura de datos que contiene los parametros de la sala
        /// </summary>
        public struct Params 
        {
            public string nombre, monto, precio, ganancia, cantidad;

            public Params(string p1, string p2, string p3, string p4, string p5)
            {
                nombre = p1;
                monto = p2;
                precio = p3;
                ganancia = p4;
                cantidad = p5;
            }
        }

        static public Params param;

        private void Start()
        {
            //Se cambia el teclado mostrado en móviles como teclado numérico 
            monto.keyboardType = TouchScreenKeyboardType.NumberPad;
            precio.keyboardType = TouchScreenKeyboardType.NumberPad;
            ganancia.keyboardType = TouchScreenKeyboardType.NumberPad;
            cantidad.keyboardType = TouchScreenKeyboardType.NumberPad;
        }

        /// <summary>
        /// Se almacenan los parametros de la sala en el objeto param
        /// </summary>
        public void setGetInputs()
        {
            param.nombre = PhotonNetwork.LocalPlayer.NickName;
            param.monto = monto.text;
            param.precio = precio.text;
            param.ganancia = ganancia.text;
            param.cantidad = cantidad.text;

            int cant = System.Convert.ToInt32(param.cantidad);
            if (cant > 20) Warning.text = "Advertencia: La Sala no puede tener más de 20 Jugadores";
        }

        /// <summary>
        /// Se añaden los parametros de la sala al objeto Room
        /// </summary>
        public void SetRoomProperties()
        {
            CustomProps["monto"] = param.monto;
            CustomProps["precio"] = param.precio;
            CustomProps["ganancia"] = param.ganancia;
            CustomProps["Imagen"] = PhotonNetwork.MasterClient.CustomProperties["Imagen"];
            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomProps);
        }
    }
}