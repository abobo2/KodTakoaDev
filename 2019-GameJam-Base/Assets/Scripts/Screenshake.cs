using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Assets.Scripts
{
    public static class Screenshake
    {
        private static Queue<Vector2> screenSheks = new Queue<Vector2>();

        private static bool isRunning;

        public static void RequestScreenshake(float magnitude, float frameDuration)
        {
            float dir = Random.Range(0, 1f) * Mathf.PI * 2;
            for (int i = 0; i < frameDuration - screenSheks.Count; i++)
            {
                Vector2 v = new Vector2(Mathf.Sin(dir), Mathf.Cos(dir)) * magnitude;
                screenSheks.Enqueue(v);
            }
            screenSheks.Enqueue(Vector2.zero);
            if (!isRunning)
            {
                isRunning = true;
                ChangeScreenshakeVector();
            }
        }

        private static void ChangeScreenshakeVector()
        {
            ScreenshakeVector = screenSheks.Dequeue();

            if (screenSheks.Count > 0)
            {
                Observable.Timer(TimeSpan.FromSeconds(0.03)).Subscribe(_ =>
                {
                    ChangeScreenshakeVector();
                });
            }
            else
            {
                isRunning = false;
            }
        }

        public static Vector2 ScreenshakeVector;
    }
}
