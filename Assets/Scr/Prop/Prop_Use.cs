using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Background;

namespace Prop
{
    public class Prop_Use : MonoBehaviour
    {
        Button Button;
        private static int Prop_Count;
        public PropType propType;

        //public static Prop_Use Instance;

        //private void Awake()
        //{

        //    if (Instance != null)
        //    {
        //        Destroy(Instance);
        //    }
        //    Instance = this;
        //}

        private void Start()
        {

            Button = GetComponent<Button>();
            Button.onClick.AddListener(OnUsing);
            Prop_Count = 0;
        }

        public void OnUsing()
        {
            Table table = Table.Instance;
            if(Prop_Count>0)
            {
                table.Use_Prop(propType);
                Prop_Count--;
            }




        }


        public static void Add_Prop(int i)
        {
            Prop_Count += i;
        }

        public static int Get_Prop_Count()
        {
            return Prop_Count;

        }
    }
}
