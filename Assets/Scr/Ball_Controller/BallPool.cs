using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Background;

namespace Ball_Controller
{
    public class BallPool
    {
        Dictionary<ColorType, List<Ball>> Ball_Pool_Stay = new Dictionary<ColorType, List<Ball>>();

        Dictionary<ColorType, List<Ball>> Ball_Pool_Using = new Dictionary<ColorType, List<Ball>>();


        public Ball Get_Ball(ColorType colorType)
        {
            List<Ball> bps = Ball_Pool_Stay[colorType];
            List<Ball> bpu = Ball_Pool_Using[colorType];
            Ball ball;
            if (bps.Count == 0)
            {
                ball = Ball_Creater.Creat(colorType).GetComponent<Ball>();
            }
            else
            {
                ball = bps[0];
                bps.RemoveAt(0);
            }
            bpu.Add(ball);

            ball.transform.gameObject.SetActive(false);

            return ball;
        }

        public void Recovery(Ball ball)
        {
            ball.Is_Sparkling = false;
            ColorType colorType = ball.ColorType;
            List<Ball> bps = Ball_Pool_Stay[colorType];
            List<Ball> bpu = Ball_Pool_Using[colorType];
            if(bpu.Contains(ball))
            {
                bpu.Remove(ball);
            }
            bps.Add(ball);
            ball.Disactive();

        }

        public void Build_Pool()
        {
            for(int i=0;i<6;i++)
            {
                List<Ball> bps = new List<Ball>();
                List<Ball> bpu = new List<Ball>();

                Ball_Pool_Stay.Add((ColorType)i, bps);
                Ball_Pool_Using.Add((ColorType)i, bpu);

            }

        }




    }
}
