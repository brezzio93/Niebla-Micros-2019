using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class SpriteControl : MonoBehaviour
    {
        [SerializeField]
        private AvatarCreator playerTemplate;      

        private List<AvatarCreator> buttons;

        private string currentScene;

        // Start is called before the first frame update
        private void Start()
        {
            buttons = new List<AvatarCreator>();

            currentScene = GameManager.instance.GetCurrentScene().name;
            if (currentScene == "05 Espera")
            {
                GameManager.instance.AlEntrarJugador += Instance_AlEntrarJugador;
                InstanciarBotones();
            }

            if (currentScene == "10 Resultados Finales")
            {
                ListarJugadores();
            }
        }

        private void Update()
        {
            if (currentScene == "05 Espera")
                if (!PhotonNetwork.IsMasterClient) ListarJugadoresEspera();
            if (currentScene == "10 Resultados Finales")
                ListarJugadores();
        }

        private void Instance_AlEntrarJugador(Player obj)
        {
            //throw new System.NotImplementedException();
            Debug.Log(obj.CustomProperties["Imagen"] as string);
            foreach (AvatarCreator button in buttons)
            {
                if (!button.asignado)
                {
                    button.ConstruirAvatar(obj.CustomProperties["Imagen"] as string);
                    button.ObtenerNombre(obj.NickName);
                    break;
                }
            }
        }

        public void InstanciarBotones()
        {
            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers - 1; i++)
            {
                AvatarCreator button = Instantiate(playerTemplate) as AvatarCreator;
                button.gameObject.SetActive(true);
                button.transform.SetParent(playerTemplate.transform.parent, false);
                buttons.Add(button);
            }
        }

        public void ListarJugadores()
        {
            if (buttons.Count > 0)
            {
                foreach (AvatarCreator button in buttons)
                    Destroy(button.gameObject);
            }
            buttons.Clear();

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                AvatarCreator button = Instantiate(playerTemplate) as AvatarCreator;
                button.gameObject.SetActive(true);
                button.ConstruirAvatar(p.CustomProperties["Imagen"] as string);
                button.ObtenerNombre(p.NickName);
                button.transform.SetParent(playerTemplate.transform.parent, false);
                buttons.Add(button);
            }
        }

        /// <summary>
        /// Actualiza lista de jugadores en la sala de espera
        /// </summary>
        public void ListarJugadoresEspera()
        {
            Player[] playerNoHost = PhotonNetwork.PlayerList;
            List<Player> PlayerList = new List<Player>();//Copia de PhotonNetwork.PlayerList sin el host de la sala
            PlayerList.AddRange(playerNoHost);
            PlayerList.Remove(PhotonNetwork.MasterClient);

            if (buttons.Count > 0)
            {
                foreach (AvatarCreator button in buttons)
                    Destroy(button.gameObject);
            }
            buttons.Clear();

            foreach (Player p in PlayerList)
            {
                AvatarCreator button = Instantiate(playerTemplate) as AvatarCreator;
                button.gameObject.SetActive(true);
                button.ConstruirAvatar(p.CustomProperties["Imagen"] as string);
                button.transform.SetParent(playerTemplate.transform.parent, false);
                buttons.Add(button);
            }
        }
    }
}