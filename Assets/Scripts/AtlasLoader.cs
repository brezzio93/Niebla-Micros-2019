using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class AtlasLoader : MonoBehaviour
    {
        [SerializeField]
        private Image img;

        public static Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();

        
        void Start()
        {
            Sprite[] allSprites = Resources.LoadAll<Sprite>("Atlas_1");

            if (allSprites == null || allSprites.Length <= 0)
            {
                Debug.LogError("The Provided Base-Atlas Sprite `Atlas_1` does not exist!");
                return;
            }

            foreach (var sprite in allSprites)
            {
                spriteDic.Add(sprite.name, sprite);
            }
        }

        public void test()
        {
            Sprite tempSprite=img.sprite;
            string atlasName = "atlas_1_8";
            if (!spriteDic.TryGetValue(atlasName, out tempSprite))
            {
                Debug.LogError("The Provided atlas `" + atlasName + "` does not exist!");
                return;
            }
            img.sprite = tempSprite;
            
        }


    }
}