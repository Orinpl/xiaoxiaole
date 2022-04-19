using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Manager
{
    public class Mouse_Controller : MonoBehaviour
    {

        public static Mouse_Controller Instance;

        private void Awake()
        {
            if(Instance!=null)
            {
                Destroy(Instance);
            }

            Instance = this;
        }

        private void Start()
        {
            

        }



        public GameObject Mouse_On_Click()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            return hit.collider.gameObject;
        }

    }
}
