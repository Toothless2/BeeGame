using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace BeeGame.test
{
    public class FPSCounter : MonoBehaviour
    {
        public Text text;
#if DEBUG

        float frameCount = 0;
        float dt = 0;
        float fps = 0;
        float updateRate = 4;  // 4 updates per sec.

        void Update()
        {
            frameCount++;
            dt += Time.deltaTime;
            if (dt > 1.0 / updateRate)
            {
                fps = frameCount / dt;
                frameCount = 0;
                dt -= 1.0f / updateRate;

                text.text = fps.ToString();
            }
        }
#else
        private void Start()
        {
            Destroy(text.transform.parent.gameObject);
        }
#endif
    }
}
