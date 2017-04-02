using UnityEngine;
using UnityEngine.UI;

namespace BeeGame.test
{
    public class FPSCounter : MonoBehaviour
    {
        public Text text;
#if DEBUG
        private int delay = 0;

        void Update()
        {
            if (delay > 50)
            {
                text.text = (1f / Time.unscaledDeltaTime).ToString();
                delay = 0;
            }

            delay++;
        }
#else
        private void Start()
        {
            Destroy(text.transform.parent.gameObject);
            Destroy(this);
        }
#endif
    }
}
