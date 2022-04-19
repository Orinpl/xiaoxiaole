using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Ball_Controller;

namespace Background
{
    public class Ball_Creater : MonoBehaviour
    {
        public GameObject Red;
        public GameObject Yellow;
        public GameObject Bule;
        public GameObject Green;
        public GameObject Pink;
        public GameObject Orange;

        public static Dictionary<ColorType, GameObject> Dic_Ball = new Dictionary<ColorType, GameObject>();

        public bool IsBuild = false;

        private void Awake()
        {

            Build_Dic();
        }



        //实例化小球
        public static GameObject Creat(ColorType colorType)
        {
            GameObject go = Dic_Ball[colorType];

            GameObject gameObject = Instantiate(go);

            return gameObject;
        }

        public void Build_Dic()
        {
            Dic_Ball.Add(ColorType.Red, Red);
            Dic_Ball.Add(ColorType.Bule, Bule);
            Dic_Ball.Add(ColorType.Green, Green);
            Dic_Ball.Add(ColorType.Orange, Orange);
            Dic_Ball.Add(ColorType.Pink, Pink);
            Dic_Ball.Add(ColorType.Yellow, Yellow);
            IsBuild = true;
        }






    }
}
