using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Prop
{
    public class Prop_Number:MonoBehaviour
    {
        int num { get=> Prop_Use.Get_Prop_Count(); }
        Text Text;

        private void Start()
        {
            Text = GetComponent<Text>();   
        }

        private void Update()
        {
            Text.text = num.ToString();
        }

    }
}
