using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ball_Controller;
using UnityEngine;
using UnityEngine.UI;
using Prop;

namespace Background
{
    public class Table : MonoBehaviour
    {
        public int Col_Length = 8;
        public int Row_Length = 8;

        public Ball[] Balls = new Ball[64];

        public Ball[] Backup_Balls = new Ball[8];

        public float Cell_Width { get => 1; }

        public Vector3 Left_Top { get => new Vector3(-3.5f, 3.5f, 0); }

        private BallPool BallPool;

        public static Table Instance;

        public bool Is_Using_Prop = false;
        public PropType PropType;

        public Button Button_Refresh;
        public Text Is_Can_Clear;

        private void Awake()
        {

            if (Instance != null)
            {
                Destroy(Instance);
            }
            Instance = this;
        }

        private void Start()
        {
            Button_Refresh.onClick.AddListener(Refresh_Table);

            BallPool = new BallPool();
            BallPool.Build_Pool();
            Refresh_Table();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Refresh_Table();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                Delet_All();
            }

            if (Input.GetMouseButtonDown(0))
            {
                GameObject gameObject = Mouse_On_Click();

                if (gameObject != null)
                {
                    if (gameObject.CompareTag("Ball"))
                    {
                        List<Point> clear_list = new List<Point>();
                        Ball ball = gameObject.GetComponent<Ball>();
                        Point p = ball.Get_Point();
                        Debug.Log(string.Format("点击小球[{0},{1}]", p.x, p.y));

                        Ball b = Balls[Get_Index_By_Point(p.x, p.y)];
                        Debug.Log(string.Format("点击桌面[{0},{1}]", b.Get_Point().x, b.Get_Point().y));

                        //Delet_Ball(p);


                        if (Is_Using_Prop)
                        {
                            if (PropType == PropType.Col)
                            {
                                Delet_Col(p.x);
                            }
                            else if (PropType == PropType.Row)
                            {
                                Delet_Row(p.y);
                            }
                            Is_Using_Prop = false;
                        }


                        if (Check_Ball(ball, out clear_list))
                        {
                            foreach (Point point in clear_list)
                            {
                                Debug.Log(string.Format("[{0},{1}]", point.x, point.y));

                            }
                            Clear_Same_Color_Balls(clear_list);


                        }


                    }

                }
            }

            if(Scan_Table())
            {
                Is_Can_Clear.text = "是";
            }
            else
            {
                Is_Can_Clear.text = "否";
            }
        }


        public GameObject Mouse_On_Click()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if(Physics.Raycast(ray, out hit))
            {
                return hit.collider.gameObject;

            }
            return null;
        }












        public Vector3 Get_Position_By_Point(int x, int y)
        {

            float _x = Left_Top.x + x * Cell_Width;
            float _y = Left_Top.y - y * Cell_Width;


            Vector3 result = new Vector3(_x, _y, 0);

            return result;
        }

        public Point Get_Point_By_Position(Vector3 position)
        {
            Point point = new Point();

            point.x = (int)Math.Abs((position.x - Left_Top.x) / Cell_Width);
            point.y = (int)Math.Abs((position.y - Left_Top.y) / Cell_Width);

            return point;
        }


        public int Get_Index_By_Point(int x, int y)
        {
            int index = 64;

            index = y * Row_Length + x;

            return index;

        }

        public Point Get_Point_By_Index(int index)
        {
            Point point = new Point();

            point.x = index % Row_Length;
            point.y = (index - point.x) / Row_Length;


            return point;
        }

        public Ball Get_Ball_By_Point(Point point)
        {
            int index = Get_Index_By_Point(point.x, point.y);

            return Balls[index];
        }


        public void Set_Position(Ball ball, int x, int y)
        {
            Vector3 position = Get_Position_By_Point(x, y);
            Point point = new Point(x, y);
            ball.Set_Point(point);
            //Debug.Log(position);
            ball.transform.position = position;

        }

        //检测是否满足条件
        //横排或纵排满足3个以及以上同颜色的球
        public bool Check_Ball(Ball ball, out List<Point> clear_balls)
        {
            bool flag = false;
            clear_balls = new List<Point>();
            int left_count = 0;
            int right_count = 0;
            int up_count = 0;
            int down_count = 0;

            List<Point> row_ball = new List<Point>();
            List<Point> col_ball = new List<Point>();

            Point p = ball.Get_Point();
            //检测右边
            while (true)
            {
                p.x += 1;
                if (p.x >= Row_Length)
                {
                    break;
                }
                Ball b = Get_Ball_By_Point(p);
                if (b.ColorType == ball.ColorType)
                {
                    right_count++;
                    Point _p = new Point(p.x, p.y);
                    row_ball.Add(_p);
                }
                else
                {
                    break;
                }

            }

            p = ball.Get_Point();
            //检测左边
            while (true)
            {
                p.x -= 1;
                if (p.x < 0)
                {
                    break;
                }
                Ball b = Get_Ball_By_Point(p);
                if (b.ColorType == ball.ColorType)
                {
                    left_count++;
                    Point _p = new Point(p.x, p.y);
                    row_ball.Add(_p);
                }
                else
                {
                    break;
                }

            }

            p = ball.Get_Point();
            //检测上边
            while (true)
            {
                p.y -= 1;
                if (p.y < 0)
                {
                    break;
                }
                Ball b = Get_Ball_By_Point(p);
                if (b.ColorType == ball.ColorType)
                {
                    up_count++;
                    Point _p = new Point(p.x, p.y);
                    col_ball.Add(_p);
                }
                else
                {
                    break;
                }

            }

            p = ball.Get_Point();
            //检测下边
            while (true)
            {
                p.y += 1;
                if (p.y >= Col_Length)
                {
                    break;
                }
                Ball b = Get_Ball_By_Point(p);
                if (b.ColorType == ball.ColorType)
                {
                    down_count++;
                    Point _p = new Point(p.x, p.y);
                    col_ball.Add(_p);
                }
                else
                {
                    break;
                }

            }

            if ((left_count + right_count + 1) >= 3 || (up_count + down_count + 1) >= 3)
            {
                flag = true;
                p = ball.Get_Point();
                clear_balls.Add(p);
                if ((left_count + right_count + 1) >= 3)
                {
                    foreach (Point point in row_ball)
                    {
                        clear_balls.Add(point);
                    }
                }
                if ((up_count + down_count + 1) >= 3)
                {
                    foreach (Point point in col_ball)
                    {
                        clear_balls.Add(point);
                    }
                }


            }
            else
            {
                flag = false;
            }



            return flag;
        }


        public void Clear_Same_Color_Balls(List<Point> Balls)
        {
            foreach (Point p in Balls)
            {
                Debug.Log(string.Format("清除[{0}][{1}]", p.x, p.y));
                Delet_Ball(p);
            }
            if(Balls.Count>=4)
            {
                Prop_Use.Add_Prop(1);
            }

            Refill();

        }

        //在某列上面随机生成小球
        public Ball Create_Random_Ball()
        {
            ColorType colorType = (ColorType)UnityEngine.Random.Range(0, 6);


            return BallPool.Get_Ball(colorType).GetComponent<Ball>();

        }

        public Vector3 Get_Backup_Position_By_Index(int x)
        {
            Vector3 vector3 = new Vector3(Left_Top.x + x * Cell_Width, Left_Top.y + Cell_Width, 0);

            return vector3;
        }


        public void Replace_Ball(Point point, Ball ball)
        {
            int index = Get_Index_By_Point(point.x, point.y);

            Set_Position(ball, point.x, point.y);

            Balls[index] = ball;

        }


        //全部重新刷新生成
        public void Refresh_Table()
        {
            Delet_All();
            System.Random random = new System.Random();
            for (int i = 0; i < 64; i++)
            {
                ColorType colorType = (ColorType)random.Next(0, 6);
                Balls[i] = BallPool.Get_Ball(colorType).GetComponent<Ball>();
                Balls[i].Active();
                Point p = Get_Point_By_Index(i);

                //Balls[i].Set_Point(p);
                //Balls[i].Set_Position(Get_Position_By_Point(p.x, p.y));
                Set_Position(Balls[i], p.x, p.y);
            }
        }

        //清空所有小球
        public void Delet_All()
        {
            for (int i = 0; i < Balls.Length; i++)
            {
                if (Balls[i] != null)
                {
                    BallPool.Recovery(Balls[i]);
                    Balls[i] = null;
                }
            }

        }

        public void Delet_Ball(Point point)
        {
            int index = Get_Index_By_Point(point.x, point.y);

            if (Balls[index] != null)
            {
                BallPool.Recovery(Balls[index]);
                Balls[index] = null;
            }

        }

        //下落
        public int[] Ball_Fall()
        {
            int[] points_y;
            int[] empty_length;
            Check_Button(out points_y, out empty_length);

            Point point = new Point();


            int[] move_ball_number = new int[8];
            for (int j = 0; j < 8; j++)
            {
                move_ball_number[j] = 0;
            }

            for (int i = 0; i < 8; i++)
            {
                int count = empty_length[i];

                if (count == 0)
                {
                    continue;
                }

                point.x = i;
                //最顶上的那个
                point.y = points_y[i] - count;


                while (Get_Index_By_Point(point.x, point.y) > 0)
                {
                    move_ball_number[i]++;
                    Ball ball = Get_Ball_By_Point(point);
                    Balls[Get_Index_By_Point(point.x, point.y)] = null;
                    Point rp = new Point(point.x, point.y + count);
                    Replace_Ball(rp, ball);
                    point.y--;

                }


            }

            return move_ball_number;
        }

        //重新填充
        public void Refill()
        {
            int[] points_y;
            int[] empty_length;
            Check_Button(out points_y, out empty_length);

            Point point = new Point();

            int[] move_ball_number = Ball_Fall();

            for (int i = 0; i < 8; i++)
            {
                int count = empty_length[i];
                if (count <= 0)
                {
                    continue;
                }

                point.x = i;
                point.y = points_y[i] - move_ball_number[i];


                while (point.y >= 0)
                {
                    Ball ball = Create_Random_Ball();
                    ball.Active();
                    ball.Sparkle(3f);
                    Replace_Ball(point, ball);
                    point.y--;
                }

            }




        }

        public void Check_Button(out int[] points_y, out int[] empty_length)
        {
            int[] temp = new int[8];
            int[] length = new int[8];

            for (int i = 0; i < 8; i++)
            {
                temp[i] = -1;
                length[i] = 0;
            }

            List<Point> empty = Scan_Empty();

            foreach (Point point in empty)
            {
                length[point.x]++;
                if (point.y > temp[point.x])
                {
                    temp[point.x] = point.y;
                }
            }

            points_y = temp;
            empty_length = length;
        }

        public List<Point> Scan_Empty()
        {
            List<Point> points = new List<Point>();

            for (int i = 0; i < 64; i++)
            {
                if (Balls[i] == null)
                {
                    Point point = Get_Point_By_Index(i);
                    points.Add(point);

                }
            }
            return points;
        }


        //扫描看是否有能消除的
        public bool Scan_Table()
        {
            List<Point> points = new List<Point>();
            foreach (Ball ball in Balls)
            {

                if (Check_Ball(ball, out points))
                {
                    return true;
                }

            }

            return false;
        }


        //删除指定行
        public void Delet_Row(int i)
        {
            Point point = new Point(0, i);
            while (point.x < 8)
            {
                Delet_Ball(point);
                point.x++;
            }
            Refill();
        }

        //删除指定列
        public void Delet_Col(int i)
        {
            Point point = new Point(i, 0);
            while (point.y < 8)
            {
                Delet_Ball(point);
                point.y++;
            }
            Refill();
        }

        public void Use_Prop(PropType propType)
        {
            Is_Using_Prop = true;
            PropType = propType;

        }


    }
}
