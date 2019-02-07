using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class SpritesLoader : MonoBehaviour
    {
        [SerializeField]
        private Image playerAvatar;

        [SerializeField]
        private Texture2D texture;

        private SpriteRenderer renderer;

        // Start is called before the first frame update
        private void Start()
        {
            //Debug.Log(texture.name);
            Sprite[] sprites = Resources.LoadAll<Sprite>("Resources/" + texture.name);

            playerAvatar.sprite = Resources.Load(PlayerParameters.ChosenName) as Sprite;

            LoadPlayerSprite();
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void LoadPlayerSprite()
        {
        }
    }
}