using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class SpriteControl : MonoBehaviour
    {
        [SerializeField]
        private GameObject playerTemplate;

        private List<GameObject> buttons;

        // Start is called before the first frame update
        private void Start()
        {
            buttons = new List<GameObject>();            
        }

        private void Update()
        {
            ListarJugadores();
        }

        public void ListarJugadores()
        {
            if (buttons.Count > 0)
            {
                foreach (GameObject button in buttons)
                    Destroy(button.gameObject);
            }
            buttons.Clear();

            foreach (Player p in PhotonNetwork.PlayerListOthers)
            {
                GameObject button = Instantiate(playerTemplate) as GameObject;
                button.SetActive(true);

                button.transform.SetParent(playerTemplate.transform.parent, false);
                buttons.Add(button.gameObject);
            }
        }
    }
}