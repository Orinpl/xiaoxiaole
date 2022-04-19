using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Background;

namespace Ball_Controller
{
    public class Ball : MonoBehaviour
    {
        public ColorType ColorType;

        private Point Point;

        Renderer renderer;
        Material material;
        float smoothness;
        float sn;
        float spakle_time;
        float left_time;
        public bool Is_Sparkling = false;

        public bool IsPoint
        {
            get
            {
                if (Point != null)
                    return true;
                else
                    return false;
            }
        }

        // Start is called before the first frame update
        private void Awake()
        {
            Point = new Point();
            renderer = GetComponent<Renderer>();
            material = renderer.material; 
            smoothness = material.GetFloat("_Glossiness");
            sn = smoothness;
            spakle_time = 0;
        }
        // Update is called once per frame
        void Update()
        {
            if (Is_Sparkling)
            {
                if (sn >= 1)
                {
                    sn = 0;
                }
                else
                {
                    sn += Time.deltaTime;
                }

                material.SetFloat("_Glossiness", sn);
                left_time -= Time.deltaTime;
                if(left_time<=0)
                {
                    Is_Sparkling = false;
                    material.SetFloat("_Glossiness", smoothness);
                }
            }




        }

        public void Disactive()
        {
            transform.gameObject.SetActive(false);
        }

        public void Active()
        {
            transform.gameObject.SetActive(true);
        }


        public Point Get_Point()
        {
            Point p = new Point();
            p.x = Point.x;
            p.y = Point.y;
            return p;
        }

        public void Set_Point(Point point)
        {
            Point.x = point.x;
            Point.y = point.y;


        }

        public void Set_Position(Vector3 vector3)
        {
            transform.position = vector3;
        }

        public void Sparkle(float time)
        {
            spakle_time = time;
            left_time = time;

            Is_Sparkling = true;
        }

    }
}