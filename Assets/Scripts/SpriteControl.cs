using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class SpriteControl : MonoBehaviour
    {
        [SerializeField]
        private JugadoresEnEspera playerTemplate;

        private List<JugadoresEnEspera> buttons;

        private string currentScene;

        // Start is called before the first frame update
        private void Start()
        {
            buttons = new List<JugadoresEnEspera>();

            currentScene = GameManager.instance.GetCurrentScene().name;
            if (currentScene == "05 Espera")
            {
                GameManager.instance.AlEntrarJugador += Instance_AlEntrarJugador;
                InstanciarBotones();
            }

            if (currentScene == "10 Resultados Finales")
            {
                GameManager.instance.AlEntrarJugador -= Instance_AlEntrarJugador;
                ListarJugadores();
            }
        }

        private void Update()
        {
            if (currentScene == "05 Espera")
                ListarJugadoresEspera();
            if (currentScene == "10 Resultados Finales")
                ListarJugadores();
        }

        private void Instance_AlEntrarJugador(Player obj)
        {
            //throw new System.NotImplementedException();

            foreach (JugadoresEnEspera button in buttons)
            {
                if (!button.asignado)
                {
                    button.ConstruirAvatar(obj.CustomProperties["Imagen"] as string);

                    break;
                }
            }
        }

        public void InstanciarBotones()
        {
            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers - 1; i++)
            {
                JugadoresEnEspera button = Instantiate(playerTemplate) as JugadoresEnEspera;
                button.gameObject.SetActive(true);
                button.transform.SetParent(playerTemplate.transform.parent, false);
                buttons.Add(button);
            }
        }

        public void ListarJugadores()
        {
            if (buttons.Count > 0)
            {
                foreach (JugadoresEnEspera button in buttons)
                    Destroy(button.gameObject);
            }
            buttons.Clear();

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                JugadoresEnEspera button = Instantiate(playerTemplate) as JugadoresEnEspera;
                button.gameObject.SetActive(true);

                button.ConstruirAvatar(p.CustomProperties["Imagen"] as string);

                button.transform.SetParent(playerTemplate.transform.parent, false);
                buttons.Add(button);
            }

            foreach (JugadoresEnEspera button in buttons)
            {
                //foreach (Player p in PlayerList)
                //{
                //    Sprite sprite = GameManager.instance.Avatars.FirstOrDefault(s => s.name == (p.CustomProperties["Imagen"] as string));
                //}
            }
        }

        public void ListarJugadoresEspera()
        {
            Player[] playerNoHost = PhotonNetwork.PlayerList;
            List<Player> PlayerList = new List<Player>();
            PlayerList.AddRange(playerNoHost);
            PlayerList.Remove(PhotonNetwork.MasterClient);

            if (buttons.Count > 0)
            {
                foreach (JugadoresEnEspera button in buttons)
                    Destroy(button.gameObject);
            }
            buttons.Clear();

            foreach (Player p in PlayerList)
            {
                JugadoresEnEspera button = Instantiate(playerTemplate) as JugadoresEnEspera;
                button.gameObject.SetActive(true);

                button.ConstruirAvatar(p.CustomProperties["Imagen"] as string);

                button.transform.SetParent(playerTemplate.transform.parent, false);
                buttons.Add(button);
            }

            foreach (JugadoresEnEspera button in buttons)
            {
                //foreach (Player p in PlayerList)
                //{
                //    Sprite sprite = GameManager.instance.Avatars.FirstOrDefault(s => s.name == (p.CustomProperties["Imagen"] as string));
                //}
            }
        }
    }
}