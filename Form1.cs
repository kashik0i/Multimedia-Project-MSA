using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BigGame
{
    class WeaponIcon
    {
        public Bitmap img;
    }
    class Camera
    {
        public int x, y;
    }
    class Shape
    {
        public List<Bitmap> Walk;
        public List<Bitmap> Climb;
        public bool isWalking, isJumping, isClimbing, isHurt, isIdle, isFlip, isFalling;
        public int JumpTimer = 0;
        public int speed;
        public int dirx, diry;
        public int JumpSpeed;
        public int hp;
        public int shield;
        public int MaxShield;
        public int MaxHp;
        public int lvl;
        public int x, y, w, h;
        public string type;
        public Bitmap img;
        public int CurImg;
        public bool isOnLedder;
    }
    class Weapon : Shape
    {

    }
    class Player : Shape
    {

        public bool HasFlag1=false, HasFlag2 = false, HasFlag3 = false;
        public Weapon AK;
        public int CurWeapon = 0;

        public List<Bitmap> IdleImg;



        public void Jump(int GameTimer)
        {
            JumpTimer = GameTimer + 6;
            isJumping = true;

        }





    }
    class Bullet : Shape
    {
        public float dirx, diry, speed, x, y;
        public int bullettype, damage;

    }
    class AI : Shape
    {


        public bool seePlayer;
        public bool isStatic;


        public void Jump(int GameTimer)
        {
            JumpTimer = GameTimer + 6;
            isJumping = true;

        }
        public void FindPlayer(Player p)
        {
            if (type == "spinner")
            {

                int pw = p.img.Width;
                int ph = p.img.Height;
                int px = (p.x);
                int py = (p.y);


                bool fx1 = false, fx2 = false, fy1 = false, fy2 = false;
                int w = img.Width;
                int h = img.Height;




                if (x >= px && x <= px + pw)
                {
                    fx1 = true;
                }
                if (y >= py && y <= py + ph)
                {
                    fy1 = true;
                }
                if (px >= x && px <= x + w)
                {
                    fx2 = true;
                }
                if (py >= y && py <= y + h)
                {
                    fy2 = true;
                }

                if ((fx1 || fx2) && (fy1 || fy2))
                {
                    if (p.shield > 0)
                        p.shield -= 3;
                    else
                        p.hp -= 3;
                }


            }
        }
    }

    public partial class Form1 : Form
    {

        Random rr = new Random();
        List<WeaponIcon> WepIcons = new List<WeaponIcon>();
        float angle = 0;
        bool isMouseDown = false;
        Timer timer = new Timer();
        Player p;
        List<AI> ene = new List<AI>();
        List<Bullet> bullet = new List<Bullet>();
        int TimerTicks = 0;
        List<Shape> obj = new List<Shape>();
        Bitmap off;
        Camera cam = new Camera();
        public float mx = 0, my = 0;
        Point[] destinationPoints;
        List<int> hitlist;
        public Form1()
        {
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
            Load += Form1_Load;
            Paint += Form1_Paint;
            timer.Tick += Timer_Tick;
            MouseDown += Form1_MouseDown;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            MouseMove += Form1_MouseMove;
            MouseUp += Form1_MouseUp;

        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.A)
            {
                p.dirx = 0;
                p.isWalking = false;
                p.isIdle = true;


            }
            if (e.KeyCode == Keys.D)
            {
                p.dirx = 0;
                p.isWalking = false;
                p.isIdle = true;

            }
            if (e.KeyCode == Keys.W)
            {

                p.diry = 0;
            }
            if (e.KeyCode == Keys.S)
            {
                p.diry = 0;
            }
            if (e.KeyCode == Keys.Space)
            {

            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {


            //float opp = e.Y - p.y;
            //opp += opp != 0 ? 0 : 1;
            //float adj = e.X - p.x;
            //adj += adj != 0 ? 0 : 1;
            //angle = opp / adj;

            my = e.Y;


            mx = e.X;


            //  destinationPoints = new Point[]{
            //      new Point(p.x, p.y),
            //new Point(p.x + p.AK.img.Width + 20, p.y - a),
            //new Point(p.x + a, p.y + p.AK.img.Height + 20)};
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                if (true)
                {
                    p.isWalking = true;
                    p.dirx = -1;
                    //  Graphics.FromImage(img).DrawImage(AK.img, x + img.Width, (y + img.Height) / 2,400,400);
                    //  Graphics.FromImage(img).Clear(Color.Red);

                }


            }
            if (e.KeyCode == Keys.D)
            {

                p.isWalking = true;
                p.dirx = 1;

            }
            if (e.KeyCode == Keys.D1)//cur weapon is pistol
            {
                p.CurWeapon = 0;
            }
            if (e.KeyCode == Keys.D2)//cur weapon is rifle
            {
                p.CurWeapon = 1;
            }
            if (e.KeyCode == Keys.D3)//cur weapon is shotgun
            {
                p.CurWeapon = 2;
            }
            if (e.KeyCode == Keys.D4)//cur weapon is sniper
            {
                p.CurWeapon = 3;
            }
            if (e.KeyCode == Keys.W)
            {
                if (p.isClimbing)
                {

                    if (++p.CurImg >= p.Climb.Count)
                        p.CurImg = 0;
                    p.img = p.Climb[p.CurImg];
                    p.diry = -1;

                }
                //if (p.isOnLedder)
                //{
                //    p.isClimbing = true;
                //    p.diry = -1;
                //}
            }
            if (e.KeyCode == Keys.S)
            {

                if (p.isClimbing)
                {

                    if (++p.CurImg >= p.Climb.Count)
                        p.CurImg = 0;
                    p.img = p.Climb[p.CurImg];
                    p.diry = 1;

                }
            }
            if (e.KeyCode == Keys.Space)
            {
                Text += " " + p.JumpTimer;
                if (p.JumpTimer == 0)
                    p.Jump(TimerTicks);
                //if (!InAir())
                //{
                //    p.Jump(TimerTicks);
                //}
            }

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

            if (p.CurWeapon == 0) //pistol 1 shot
            {
                bullet.Add(new Bullet
                {
                    damage = rr.Next(2, 10),
                    x = p.x,
                    y = p.y,
                    w = 5,
                    h = 5,
                    img = new Bitmap(6, 6),
                    speed = 10,
                    diry = (float)Math.Sin(ang),
                    dirx = (float)Math.Cos(ang)
                });
            }
            if (p.CurWeapon == 1) //rifle multishot
            {
                isMouseDown = true;
            }
            float[] initbuck = { -0.1f, -0.2f, 0.1f, 0.2f, 0.3f };
            if (p.CurWeapon == 2) // Shotgun 1 buck
            {

                int BuckSize = rr.Next(1, 18);
                for (int i = 0; i < BuckSize; i++)
                {
                    bullet.Add(new Bullet
                    {
                        bullettype = 2,
                        x = p.x,
                        y = p.y,
                        w = 2,
                        h = 2,
                        img = new Bitmap(12, 12),
                        speed = 6,
                        diry = (float)Math.Sin(ang) + initbuck[rr.Next(0, 22) % initbuck.Length],
                        dirx = (float)Math.Cos(ang) + initbuck[rr.Next(0, 22) % initbuck.Length]
                    });
                }

            }
            if (p.CurWeapon == 3) // sniper
            {
                bullet.Add(new Bullet
                {

                    x = p.x,
                    y = p.y,
                    w = 5,
                    h = 5,
                    img = new Bitmap(6, 6),
                    speed = 17,
                    diry = (float)Math.Sin(ang),
                    dirx = (float)Math.Cos(ang)
                });
            }

        }


        bool Collision(Shape p)
        {
            hitlist = new List<int>();
            Text = " ";
            int ct = 0;
            bool fx1, fx2, fy1, fy2;

            int pw = p.img.Width;
            int ph = p.img.Height;
            int px = (p.x);
            int py = (p.y);
            for (int i = 1; i < obj.Count; i++)
            {

                fx1 = fx2 = fy1 = fy2 = false;
                int w = obj[i].img.Width;
                int h = obj[i].img.Height;
                int x = obj[i].x;
                int y = obj[i].y;



                if (x >= px && x <= px + pw)
                {
                    fx1 = true;
                }
                if (y >= py && y <= py + ph)
                {
                    fy1 = true;
                }
                if (px >= x && px <= x + w)
                {
                    fx2 = true;
                }
                if (py >= y && py <= y + h)
                {
                    fy2 = true;
                }

                if ((fx1 || fx2) && (fy1 || fy2))
                {

                    hitlist.Add(i);
                    Text += " " + i + " type: " + obj[i].type;
                    ct++;
                    if(obj[i].type=="flag1")
                    {
                        this.p.HasFlag1 = true;

                    }
                    if (obj[i].type == "flag2")
                    {
                        this.p.HasFlag2 = true;
                    }
                    if (obj[i].type == "flag3")
                    {
                        this.p.HasFlag3 = true;
                    }

                    if (obj[i].type == "ladder")
                    {

                        p.isOnLedder = true;
                        p.isClimbing = true;
                        return true;

                    }

                    p.isClimbing = false;
                    //if (obj[i].type == "Platform1")
                    //{
                    //    p.dirx = obj[lead1].dirx;
                    //}
                    if (p.type == "player")
                    {
                        if (x < px && y < (py) && p.dirx == -1)
                        {
                            p.dirx = 0;

                        }
                        if (p.dirx == 1 && x > px && y < (py))
                        {

                            p.dirx = 0;
                        }
                    }
                    else
                    {
                        if (x < px && y < (py) && p.dirx == -1)
                        {
                            p.dirx = 1;

                        }
                        if (p.dirx == 1 && x > px && y < (py))
                        {

                            p.dirx = -1;
                        }
                    }







                }
            }
            if (ct == 0)
            {
                p.diry = 1;
                // p.JumpTimer = -1;
            }
            else
            {
                if (p.isJumping != true)
                    p.JumpTimer = 0;
                p.diry = 0;
            }
            p.isOnLedder = false;
            Text += " ct: " + ct;

            return false;
        }

        bool ff = false;
        private void Timer_Tick(object sender, EventArgs e)
        {
            if(p.HasFlag1&& p.HasFlag2&& p.HasFlag3)
            {
                ff = true;
                MessageBox.Show("You Won");
            }
            if(EnemyTimer==0)
            {
                EnemyTimer = TimerTicks + 1250;
            }
            if(EnemyTimer<TimerTicks)
            {
                InitEnemies();
                EnemyTimer = 0;

            }
            if (ff)
            {
                timer.Stop();
                return;
            }
            if (p.hp < 0)
            {
                ff = true;
                MessageBox.Show("You are Dead");

            }
            Collision(p);
            //Text = "dirx " + p.dirx + " diry " + p.diry;
            //Text = "p.isJumping " + p.isJumping+" ticks: "+TimerTicks;
            TimerTicks++;
            for(int j=0;j<lead1.Count;j++)
            {
                if (obj[lead2[j]].dirx == 1)
                {
                    if (obj[lead2[j]].x >= ClientSize.Width - obj[lead2[j]].img.Width * 2)
                    {
                        obj[lead2[j]].dirx = -1;
                    }
                }
                else
                {
                    if (obj[lead1[j]].x <= obj[lead2[j]].img.Width)
                    {
                        obj[lead2[j]].dirx = 1;
                    }
                }
                for (int i = lead1[j]; i <= lead2[j]; i++)
                {

                    obj[i].x += obj[lead2[j]].dirx * 5;
                }
            }
            
            if (isMouseDown && p.CurWeapon == 1)
            {
                bullet.Add(new Bullet
                {
                    damage = rr.Next(2, 20),
                    x = p.x,
                    y = p.y,
                    w = 7,
                    h = 7,
                    img = new Bitmap(6, 6),
                    speed = 18,
                    diry = 0,
                    dirx = (mx < p.x) ? -1 : 1
                });
            }
            if (p.JumpTimer > TimerTicks)
            {

                p.isJumping = true;
                p.diry = -2;



            }
            if (p.JumpTimer == TimerTicks)
            {
                p.diry = 0;
                p.isJumping = false;

            }

            for (int i = 0; i < ene.Count; i++)
            {
                //  ene[i].x += 5;

                if (ene[i].type == "ai" && TimerTicks%20==0)
                {

                    if (ene[i].x + 50 >= p.x)
                    {
                        if (ene[i].y + ene[i].img.Height > p.y)
                        {
                            bullet.Add(new Bullet
                            {
                                type = "ene",
                                damage = rr.Next(2, 10),
                                x = ene[i].x,
                                y = ene[i].y+ ene[i].img.Height/2,
                                w = 5,
                                h = 5,
                                img = new Bitmap(12, 12),
                                speed = 4,
                                diry = 0,
                                dirx = -1
                            });
                        }

                    }
                    else if (ene[i].y + ene[i].img.Height > p.y)
                    {
                        bullet.Add(new Bullet
                        {
                            type = "ene",
                            damage = rr.Next(2, 10),
                            x = ene[i].x+ene[i].img.Width,
                            y = ene[i].y + ene[i].img.Height / 2,
                            w = 5,
                            h = 5,
                            img = new Bitmap(12, 12),
                            speed = 4,
                            diry = 0,
                            dirx = 1
                        });
                    }

                }


                Collision(ene[i]);

                ene[i].FindPlayer(p);

                ene[i].x += ene[i].dirx * ene[i].speed;
                ene[i].y += ene[i].diry * ene[i].speed;
                //HitLeft(ene[i]);

                //HitUp(ene[i]);
            }



            p.x += p.dirx * p.speed;
            p.y += p.diry * p.speed;
            if (p.isWalking)
            {
                if (++p.CurImg == p.Walk.Count)
                    p.CurImg = 0;
                p.img = p.Walk[p.CurImg];

            }

            if (!p.isClimbing && !p.isWalking)
            {


                p.img = p.IdleImg[0];

            }

            for (int i = 0; i < bullet.Count; i++)
            {
                if (!BulletHit(i))
                {
                    bullet[i].x += bullet[i].dirx * bullet[i].speed;
                    bullet[i].y += bullet[i].diry * bullet[i].speed;
                }
                else
                {
                    bullet.RemoveAt(i);
                }

            }
            if (p.y > ClientSize.Height)
            {
                cam.y = p.y - ClientSize.Height / 2;
            }
            else
            {
                cam.y = 0;
            }
            DrawDubb(CreateGraphics());
        }
        bool BulletHit(int i)
        {
            bool fx1, fx2, fy1, fy2;
            if (bullet[i].type=="ene")
            {
                fx1 = fx2 = fy1 = fy2 = false;
                int w = p.img.Width;
                int h = p.img.Height;
                int x = p.x;
                int y = p.y;


                if (bullet[i].x > x && bullet[i].x < x + w)
                {
                    fx1 = true;
                }
                if (bullet[i].y > y && bullet[i].y < y + h)
                {
                    fy1 = true;
                }
                if (x > bullet[i].x && x < p.x + bullet[i].img.Width)
                {
                    fx2 = true;
                }
                if (y > bullet[i].y && y < bullet[i].y + bullet[i].img.Height)
                {
                    fy2 = true;
                }

                if ((fx1 || fx2) && (fy1 || fy2))
                {
                    if (p.shield > 0)
                        p.shield -= 5;
                    else
                        p.hp -= 5;
                    if (p.x > x)
                    {
                        p.x -= 5;
                    }
                    else
                    {
                        p.x += 5;
                    }
                    return true;
                }
                
            }
            
            for (int j = 1; j < obj.Count; j++)
            {
                fx1 = fx2 = fy1 = fy2 = false;
                int w = obj[j].img.Width;
                int h = obj[j].img.Height;
                int x = obj[j].x;
                int y = obj[j].y;


                if (bullet[i].x > x && bullet[i].x < x + w)
                {
                    fx1 = true;
                }
                if (bullet[i].y > y && bullet[i].y < y + h)
                {
                    fy1 = true;
                }
                if (x > bullet[i].x && x < p.x + bullet[i].img.Width)
                {
                    fx2 = true;
                }
                if (y > bullet[i].y && y < bullet[i].y + bullet[i].img.Height)
                {
                    fy2 = true;
                }

                if ((fx1 || fx2) && (fy1 || fy2))
                {
                    Text = "j: " + j + " fx1 " + fx1 + " fy1 " + fy1 + " fx2 " + fx2 + " fy2 " + fy2 + " ticks " + TimerTicks + " jumpticks " + p.JumpTimer;

                    return true;
                }
            }
            for (int j = 0; j < ene.Count; j++)
            {
                if (bullet[i].type == "ene")
                    break;
                fx1 = fx2 = fy1 = fy2 = false;
                int w = ene[j].img.Width;
                int h = ene[j].img.Height;
                int x = ene[j].x;
                int y = ene[j].y;


                if (bullet[i].x > x && bullet[i].x < x + w)
                {
                    fx1 = true;
                }
                if (bullet[i].y > y && bullet[i].y < y + h)
                {
                    fy1 = true;
                }
                if (x > bullet[i].x && x < p.x + bullet[i].img.Width)
                {
                    fx2 = true;
                }
                if (y > bullet[i].y && y < bullet[i].y + bullet[i].img.Height)
                {
                    fy2 = true;
                }

                if ((fx1 || fx2) && (fy1 || fy2))
                {
                    Text = "j: " + j + " fx1 " + fx1 + " fy1 " + fy1 + " fx2 " + fx2 + " fy2 " + fy2 + " ticks " + TimerTicks + " jumpticks " + p.JumpTimer;
                    if (ene[j].shield > 0)
                        ene[j].shield -= 5;
                    else
                        ene[j].hp -= 5;
                    if (p.x > x)
                    {
                        ene[j].x -= 5;
                    }
                    else
                    {
                        ene[j].x += 5;
                    }
                    return true;
                }
            }
            return false;
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(CreateGraphics());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width * 3, ClientSize.Height * 3);
            InitPlayer();
            InitMap();
            InitEnemies();


            timer.Interval = 10;
            timer.Start();


        }
        void InitEnemies()
        {
            string loc = "enemies/ai/stand1.png";
            for (int i = 1; i <= 3; i++)
            {

                ene.Add(new AI
                {
                    type = "ai",
                    img = new Bitmap(loc),
                    x = 200 * i,
                    y = 100,
                    hp = 100,
                    shield = 50,
                    speed = 15,
                    MaxHp = 100,
                    MaxShield = 50,
                    lvl = 3
                });
            }
            for (int i = 1; i <= 3; i++)
            {

                ene.Add(new AI
                {
                    type = "ai",
                    img = new Bitmap(loc),
                    x = 200 * i,
                    y = 1100,
                    hp = 100,
                    shield = 50,
                    speed = 15,
                    MaxHp = 100,
                    MaxShield = 50,
                    lvl = 3
                });
            }
            //######biting enemy##########
            loc = "enemies/ai/spinner.png"; ;
            for (int i = 1; i <= 3; i++)
            {

                ene.Add(new AI
                {
                    type = "spinner",
                    img = new Bitmap(loc),
                    dirx = -1,
                    x = 400 * i,
                    y = 600,
                    hp = 100,
                    shield = 50,
                    speed = 15,
                    MaxHp = 100,
                    MaxShield = 50,
                    lvl = 3
                });
            }
        }
        void InitPlayer()
        {
            p = new Player
            {
                type = "player",
                x = 200,
                y = 600,
                speed = 15,
                JumpSpeed = 8,
                hp = 100,
                shield = 150,
                lvl = 1,
                MaxShield = 150,
                MaxHp = 100,
                img = new Bitmap(200, 200),
                IdleImg = new List<Bitmap>(),
                Walk = new List<Bitmap>(),
                Climb = new List<Bitmap>()

            };
            //######Walk########
            string loc;
            for (int i = 1; i <= 11; i++)
            {
                loc = "player/" + i + ".png";
                p.Walk.Add(new Bitmap(loc));
            }
            for (int i = 1; i <= 2; i++)
            {
                loc = "player/idle" + i + ".png";
                p.IdleImg.Add(new Bitmap(loc));
            }
            for (int i = 1; i <= 2; i++)
            {
                loc = "player/climb" + i + ".png";
                p.Climb.Add(new Bitmap(loc));
            }
            p.img = p.IdleImg[0];
            //######IDLE########
            for (int i = 0; i < 2; i++)
            {

            }
            //########Weapons##########
            loc = "weapons/1.png";
            p.AK = new Weapon
            {
                img = new Bitmap(loc)

            };

        }
        
        List<int> lead1=new List<int>(), lead2= new List<int>();
        void InitMap()
        {
            string loc;
            //######Weapon Icons###############
            loc = "icons/1.png";
            WepIcons.Add(new WeaponIcon { img = new Bitmap(loc) });
            loc = "icons/2.png";
            WepIcons.Add(new WeaponIcon { img = new Bitmap(loc) });
            loc = "icons/3.png";
            WepIcons.Add(new WeaponIcon { img = new Bitmap(loc) });
            loc = "icons/4.png";
            WepIcons.Add(new WeaponIcon { img = new Bitmap(loc) });



            //#######Background################
            for (int i = 1; i < 2; i++)
            {
                loc = "background/" + i + ".png";
                obj.Add(new Shape
                {
                    img = new Bitmap(new Bitmap(loc), ClientSize.Width, ClientSize.Height),
                    x = 0,
                    y = 0
                });
            }
            //#######Ground################
            Bitmap pnn;
            loc = "objects/box.png";
            for (int j = 0; j < 3; j++)
                for (int i = 0; i < 20; i++)
                {


                    pnn = new Bitmap(loc);
                    obj.Add(new Shape
                    {
                        img = pnn,
                        x = pnn.Width * i,
                        y = (ClientSize.Height - pnn.Height) + j * (ClientSize.Height * 2 - 1000),
                        type = "box"
                    });
                }

            for (int i = 0; i < 10; i++)
            {


                pnn = new Bitmap(loc);
                obj.Add(new Shape
                {
                    img = pnn,
                    x = pnn.Width * i,
                    y = ClientSize.Height / 2 - pnn.Height
                    ,
                    type = "box"
                });
            }
            //##############moving platform#####################


            pnn = new Bitmap("objects/platform.png");
            for(int j=0;j<3;j++)
            {
                Shape pn = new Shape
                {
                    dirx = 0,
                    diry = 0,
                    img = pnn,
                    x = 0,
                    y = ClientSize.Height * 2 - pnn.Height + 300*j
                                  ,
                    type = "platform-1"
                };
                obj.Add(pn);
                lead1.Add( obj.Count);
          
           
           
            for (int i = 3; i < 8; i++)
            {
                obj.Add(new Shape
                {
                    dirx = 1,
                    diry = 0,
                    img = pnn,
                    x = pnn.Width * i+ 100*j,
                    y = ClientSize.Height * 2 - pnn.Height + 300*j
                    ,
                    type = "platform1"
                });

            }
            lead2.Add(obj.Count - 1);
            pn = new Shape
            {
                dirx = 0,
                diry = 0,
                img = pnn,
                x = ClientSize.Width - pnn.Width,
                y = ClientSize.Height * 2 - pnn.Height + 300*j
                  ,
                type = "platform-1"
            };
            obj.Add(pn);

            }

            //for (int i = 3; i < 8; i++)
            //{



            //    obj.Add(new Shape
            //    {
            //        dirx = 1,
            //        diry = 0,
            //        img = pnn,
            //        x = pnn.Width * i,
            //        y = ClientSize.Height * 2 + pnn.Height + 300
            //        ,
            //        type = "platform1"
            //    });

            //}
            //############Flags###########
            pnn = new Bitmap("objects/1.png");
            obj.Add(new Shape
            {
                type = "flag1",
                img = pnn,
                y = 280,
                x = 70
               
            }) ;
            pnn = new Bitmap("objects/2.png");
            obj.Add(new Shape
            {
                type = "flag2",
                img = pnn,
                y = ClientSize.Height+500,
                x = 150
                
            });
            pnn = new Bitmap("objects/3.png");
            obj.Add(new Shape
            {
                type = "flag3",
                img = pnn,
                y = ClientSize.Height*2,
                x = 0
               
            });

            //#######Wall################
            loc = "objects/box.png";

            for (int i = 0; i < 20; i++)
            {


                pnn = new Bitmap(loc);
                obj.Add(new Shape
                {
                    img = pnn,
                    y = pnn.Height * i,
                    x = 0
                    ,
                    type = "box"
                });
            }
            for (int i = 0; i < 6; i++)
            {


                pnn = new Bitmap(loc);
                obj.Add(new Shape
                {
                    img = pnn,
                    y = pnn.Height * 5 + pnn.Height * i,
                    x = ClientSize.Width - pnn.Width * 3
                    ,
                    type = "box"
                });
            }
            for (int i = 0; i < 5; i++)
            {


                pnn = new Bitmap(loc);
                obj.Add(new Shape
                {
                    img = pnn,
                    y = pnn.Height * 16 + pnn.Height * i - 16,
                    x = pnn.Width * 19
                    ,
                    type = "box"
                });
            }
            //#######Ledder################
            loc = "objects/Tiles/ladder0.png";
            for (int i = 0; i < 6; i++)
            {


                pnn = new Bitmap(loc);
                obj.Add(new Shape
                {
                    img = pnn,
                    y = pnn.Height * 5 + pnn.Height * i,
                    x = 10 * obj[2].img.Width,
                    type = "ladder"
                });
            }

            //#######Boxes################

            //pnn = new Bitmap(loc);
            //obj.Add(new Shape
            //{
            //    img = pnn,
            //    y = ClientSize.Height - pnn.Height * 2,
            //    x = 6 * pnn.Width,
            //    type = "box"
            //});
        }
        int EnemyTimer = 0;
        void DrawDubb(Graphics g)
        {
            DrawScene(Graphics.FromImage(off));
            g.DrawImage(off, new Rectangle(0, 0, ClientSize.Width, ClientSize.Height), new Rectangle(0, cam.y, ClientSize.Width, ClientSize.Height), GraphicsUnit.Pixel);
            //g.DrawImage(off, new Rectangle(0, 0,ClientSize.Width,ClientSize.Height),new Rectangle(p.x-300,p.y-200,1000,1000),GraphicsUnit.Pixel);
        }


        float ang;
        Pen Black3 = new Pen(Color.Black, 3);
        Pen Black = new Pen(Color.Black);
        SolidBrush Red = new SolidBrush(Color.Red);
        SolidBrush Blue = new SolidBrush(Color.Blue);
        SolidBrush BlackBrush = new SolidBrush(Color.Black);
        void DrawScene(Graphics g)
        {
            //  Text = " mx " + mx + " my " + (my + p.y) + " p.x " + p.x + " p.y " + p.y + " w " + ClientSize.Width + " h " + ClientSize.Height;
            //  Text += " ang " + ang;
            ang = (float)Math.Atan2(my - p.y, mx - p.x);

            g.Clear(Color.White);





            //####################WeaponICON###############
            //size is 250*250

            g.DrawRectangle(Black3, ClientSize.Width - 150, 0, 150, 150);
            g.DrawImage(WepIcons[p.CurWeapon].img, new Rectangle(ClientSize.Width - 150, 0, 150, 150), new Rectangle(0, 0, WepIcons[p.CurWeapon].img.Width, WepIcons[p.CurWeapon].img.Height), GraphicsUnit.Pixel);

            //#############################
            g.DrawString("" +(EnemyTimer- TimerTicks), new Font("Arial", 10), new SolidBrush(Color.Black), (p.x+25), p.y-30);

            for (int i = 1; i < obj.Count; i++)
            {
                if(obj[i].type=="flag1")
                {
                    if (p.HasFlag1)
                        continue;
                }
                if (obj[i].type == "flag2")
                {
                    if (p.HasFlag1)
                        continue;
                }
                if (obj[i].type == "flag3")
                {
                    if (p.HasFlag1)
                        continue;
                }
                g.DrawImage(obj[i].img, obj[i].x, obj[i].y);
                g.DrawString("" + i, new Font("Arial", 16), BlackBrush, obj[i].x, obj[i].y);
                if (hitlist != null)
                    if (hitlist.Contains(i))
                    {
                        g.DrawRectangle(new Pen(Color.Brown, 7), obj[i].x, obj[i].y, obj[i].img.Width, obj[i].img.Height);
                    }
            }
            for (int i = 0; i < bullet.Count; i++)
            {
                g.DrawRectangle(Black, bullet[i].x, bullet[i].y, bullet[i].w, bullet[i].h);
            }

            for (int i = 0; i < ene.Count; i++)
            {
                g.DrawImage(ene[i].img, ene[i].x, ene[i].y);
                g.FillRectangle(Red, ene[i].x, ene[i].y - 10, p.img.Width * (ene[i].hp) / 100, 6);
                g.FillRectangle(Blue, ene[i].x, ene[i].y - 16, p.img.Width * (ene[i].shield) / ene[i].MaxShield, 6 * (ene[i].shield > 0 ? 1 : 0));

            }
            g.FillRectangle(Red, p.x, p.y - 10, p.img.Width * (p.hp) / 100, 6);
            g.FillRectangle(Blue, p.x, p.y - 16, p.img.Width * (p.shield) / p.MaxShield, 6 * (p.shield > 0 ? 1 : 0));
            //if(p.shield>0)
            // p.shield -= 5;
            g.DrawImage(p.img, p.x, p.y);
            g.DrawImage(p.AK.img, p.x, p.y);
            if(p.CurWeapon==3)
            g.DrawLine(Black, new Point(p.x, p.y % ClientSize.Height), new PointF(mx, my));
            //  g.DrawLine(new Pen(Color.Black), new Point(p.x, p.y), a);

            //   Text =  " isWalking: " + p.isWalking;

            // Text = "Ticks: " + TimerTicks + " x: " + p.x + " y: " + p.y + " dirx: " + p.dirx + " flip: " + p.isFlip
            //    + " diry: " + p.diry + " cos " + Math.Cos(ang) + " sin " + +Math.Cos(ang) + " other " + (my - p.y) / (mx - p.x);

        }
    }
}
