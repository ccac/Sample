using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using YYArena.Controls;
using YYArena.Core;

using FarseerGames.FarseerPhysics.Mathematics;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;

namespace YYArena
{
    public partial class Page : UserControl
    {
        private PlayerSprite _playerSprite;

        public Page()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Main_Loaded);
        }

        void Main_Loaded(object sender, RoutedEventArgs e)
        {
            InitData();

            interactive.Start += new EventHandler<EventArgs>(interactive_Start);
            interactive.LevelUp += new EventHandler<EventArgs>(interactive_LevelUp);
            interactive.Over += new EventHandler<EventArgs>(interactive_Over);
        }

        void interactive_Start(object sender, EventArgs e)
        {
            Start();
        }

        void interactive_LevelUp(object sender, EventArgs e)
        {
            Start();
        }

        void interactive_Over(object sender, EventArgs e)
        {
            Clear();
            board.Initialize();
            interactive.Load(InteractiveType.Start, board.Level);
        }

        private void InitData()
        {
            // ���ó����Լ��������Ĵ�С
            scene.Width = App.SceneWidth;
            scene.Height = App.SceneHeight;
            root.Width = scroll.Width = App.AreaWidth;
            root.Height = scroll.Height = App.AreaHeight;
            scene.Loaded += new RoutedEventHandler(delegate
            {
                scroll.ScrollToHorizontalOffset(App.SceneWidth / 2 - App.AreaWidth / 2);
                scroll.ScrollToVerticalOffset(App.SceneHeight / 2 - App.AreaHeight / 2);
            });

            // ��ͼλ��
            map.SetValue(Canvas.LeftProperty, App.AreaWidth - map.Width - 5);
            map.SetValue(Canvas.TopProperty, App.AreaHeight - map.Height - 5);
            map.Panel = canvas;
            map.Visibility = Visibility.Collapsed;

            // ���ɱ߽�
            RectangleBorder rb = new RectangleBorder((float)App.SceneWidth, (float)App.SceneHeight, new Vector2(0, 0));
            rb.CollisionGroup = (int)SpriteType.Edge;
            rb.Load(App.PhysicsSimulator);

            // ���ؽ�������
            interactive.Load(InteractiveType.Start, board.Level);
        }

        /// <summary>
        /// ������ײ����ָ���ķ���
        /// </summary>
        private bool OnCollision(Geom geom1, Geom geom2, ContactList contactList)
        {
            if (geom2.CollisionGroup == (int)SpriteType.Edge)
            {
                if (geom1.Tag is UniformVelocitySprite)
                {
                    // ��߽���ײ���ӵ�Ҫ��ը���ɴ����ñ�ը
                    ExplodeGeom(geom1);
                }
            }
            else // �Ѿ��͵о�����ײ
            {
                if (((Sprite)geom1.Tag).Enabled == false || ((Sprite)geom2.Tag).Enabled == false)
                    return false;

                if (geom1.Tag is AISprite || geom2.Tag is AISprite) // �о���٣��Ʒּ� 1
                    board.Score += 1;
                if (geom1.Tag is PlayerSprite || geom2.Tag is PlayerSprite) // �Լ����ˣ�HP �� 1
                    board.HP -= 0.1;

                // ���������Լ�
                if (geom1.Tag is PlayerSprite)
                {
                    ExplodeGeom(geom2);

                    // �Լ�û HP �ˣ�game over
                    if (board.HP < 0.1)
                    {
                        ExplodeGeom(geom1);
                        interactive.Load(InteractiveType.Over, board.Level);
                        Clear();
                    }
                }
                // �����������Լ�
                else if (geom2.Tag is PlayerSprite)
                {
                    ExplodeGeom(geom1);

                    // �Լ�û HP �ˣ�game over
                    if (board.HP < 0.1)
                    {
                        ExplodeGeom(geom2);
                        interactive.Load(InteractiveType.Over, board.Level);
                        Clear();
                    }
                }
                // �Լ����ӵ��͵о����ӵ�����ײ
                else
                {
                    ExplodeGeom(geom1);
                    ExplodeGeom(geom2);
                }
            }

            if (interactive.InteractiveType == InteractiveType.Running
                &&
                (geom1.CollisionGroup == (int)SpriteType.Enemy || geom2.CollisionGroup == (int)SpriteType.Enemy))
                DetectLevelUp();

            return true;
        }

        /// <summary>
        /// ����Ƿ��Ѿ������ȫ�����ˣ�������
        /// </summary>
        private void DetectLevelUp()
        {
            bool existsEnemy = false;
            foreach (var element in canvas.Children)
            {
                var enemy = element as PhysicsBox;
                if (enemy != null && enemy.Geom.CollisionGroup == (int)SpriteType.Enemy)
                {
                    existsEnemy = true;
                    break;
                }
            }

            if (!existsEnemy)
            {
                board.Level += 1;
                interactive.Load(InteractiveType.LevelUp, board.Level);
                Clear();
            }
        }

        /// <summary>
        /// ��ָ���� Geom �Ϸ�����ը�����Ƴ��� Geom ����������
        /// </summary>
        private void ExplodeGeom(Geom geom)
        {
            explosion.GenerateExplosion(geom.Position.X, geom.Position.Y, 20, 1, 10, 20, 0.75);
            soundEffect.PlayBomb();

            var sprite = geom.Tag as Sprite;
            geom.OnCollision -= OnCollision;
            sprite.Enabled = false;
            canvas.Children.Remove(sprite.PhysicsBox);
        }

        /// <summary>
        /// �����û������ķɴ�
        /// </summary>
        private void LoadShip()
        {
            Ship ship = new Ship();
            ship.ShipType = SpriteType.Friend;

            _playerSprite = new PlayerSprite(
                 App.PhysicsSimulator,
                 ship, new Vector2((float)App.SceneWidth / 2, (float)App.SceneHeight / 2), 0, 0,
                 App.KeyboardHandler,
                 new List<Key> { Key.Up, Key.W },
                 new List<Key> { Key.Down, Key.S },
                 new List<Key> { Key.Left, Key.A },
                 new List<Key> { Key.Right, Key.D },
                 new List<Key> { Key.J, Key.Ctrl });
            _playerSprite.Enabled = true;
            _playerSprite.PhysicsBox.Geom.CollisionGroup = (int)SpriteType.Friend;
            _playerSprite.PhysicsBox.Geom.OnCollision += OnCollision;
            _playerSprite.Fire += new EventHandler<EventArgs>(_myShip_Fire);

            _playerSprite.PhysicsBox.SetValue(Canvas.ZIndexProperty, 1000);
            canvas.Children.Add(_playerSprite.PhysicsBox);
        }

        /// <summary>
        /// ���������ɵо�
        /// </summary>
        private void LoadAIShip(int level)
        {
            for (int i = 0; i < level * 3; i++)
            {
                Ship ship = new Ship();
                ship.ShipType = SpriteType.Enemy;

                var halfWidth = (int)App.SceneWidth / 2;
                var halfHeight = (int)App.SceneHeight / 2;
                var offsetX = Helper.GenerateRandom(-halfWidth, halfWidth);
                var offsetY = Helper.GenerateRandom(-halfHeight, halfHeight);
                // ����о����Լ������ķɴ�����λ���ص�
                while (Math.Abs(offsetX) < 50)
                    offsetX = Helper.GenerateRandom(-halfWidth + 20, halfWidth - 20);
                while (Math.Abs(offsetY) < 50)
                    offsetY = Helper.GenerateRandom(-halfHeight + 20, halfHeight - 20);

                AISprite shipSprite = new AISprite(
                                App.PhysicsSimulator,
                                ship,
                                new Vector2(halfWidth + offsetX, halfHeight + offsetY),
                                Helper.GenerateRandom(0, 360),
                                0,
                                _playerSprite,
                                level);
                shipSprite.Enabled = true;
                shipSprite.PhysicsBox.Geom.CollisionGroup = (int)SpriteType.Enemy;
                shipSprite.PhysicsBox.Geom.OnCollision += OnCollision;
                shipSprite.Fire += new EventHandler<EventArgs>(aiShip_Fire);

                shipSprite.PhysicsBox.SetValue(Canvas.ZIndexProperty, 100);
                canvas.Children.Add(shipSprite.PhysicsBox);
            }
        }

        void _myShip_Fire(object sender, EventArgs e)
        {
            LoadBullet(sender as Sprite, SpriteType.Friend);
        }

        void aiShip_Fire(object sender, EventArgs e)
        {
            LoadBullet(sender as Sprite, SpriteType.Enemy);
        }

        /// <summary>
        /// �����ӵ�
        /// </summary>
        void LoadBullet(Sprite sprite, SpriteType spriteType)
        {
            //����
            Bullet bullet = new Bullet();
            bullet.BulletType = spriteType;

            UniformVelocitySprite bulletSprite = new UniformVelocitySprite(
                App.PhysicsSimulator,
                bullet, sprite.PhysicsBox.Body.Position, (float)Helper.Radian2Angle(sprite.PhysicsBox.Body.Rotation));
            bulletSprite.Enabled = true;
            bulletSprite.PhysicsBox.Geom.CollisionGroup = (int)spriteType;
            bulletSprite.PhysicsBox.Geom.OnCollision += OnCollision;

            bulletSprite.PhysicsBox.SetValue(Canvas.ZIndexProperty, 10);
            canvas.Children.Add(bulletSprite.PhysicsBox);

            //ɢ��
            float angle = (float)Helper.Radian2Angle(sprite.PhysicsBox.Body.Rotation);
            for (int i = -10; i < 10; i++)
            {
                CreateBullet(sprite.PhysicsBox.Body.Position, angle + i * 5, spriteType);
            }

            //�ŵ�
            //float angle = (float)Helper.Radian2Angle(sprite.PhysicsBox.Body.Rotation);
            //Vector2 position = sprite.PhysicsBox.Body.Position;
            //for (int i = 0; i < 8; i++)
            //{
            //    CreateBullet(position + Helper.Convert2Vector(38f * i, angle), angle, spriteType);
            //}

            soundEffect.PlayFire();
        }

        void CreateBullet(Vector2 position, float angle, SpriteType spriteType)
        {
            Bullet bullet = new Bullet();
            bullet.BulletType = spriteType;

            UniformVelocitySprite bulletSprite = new UniformVelocitySprite(
                App.PhysicsSimulator,
                bullet, position, angle);
            bulletSprite.Enabled = true;
            bulletSprite.PhysicsBox.Geom.CollisionGroup = (int)spriteType;
            bulletSprite.PhysicsBox.Geom.OnCollision += OnCollision;

            bulletSprite.PhysicsBox.SetValue(Canvas.ZIndexProperty, 10);
            canvas.Children.Add(bulletSprite.PhysicsBox);
        }

        /// <summary>
        /// ��ʱ���� Scroll �Ĺ���λ�ú͵�ͼ
        /// </summary>
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (App.AreaWidth - _playerSprite.PhysicsBox.Body.Position.X + scroll.HorizontalOffset < App.RightOffset) // ��
                scroll.ScrollToHorizontalOffset(_playerSprite.PhysicsBox.Body.Position.X - App.AreaWidth + App.RightOffset);
            else if (_playerSprite.PhysicsBox.Body.Position.X - scroll.HorizontalOffset < App.LeftOffset) // ��
                scroll.ScrollToHorizontalOffset(_playerSprite.PhysicsBox.Body.Position.X - App.LeftOffset);

            if (App.AreaHeight - _playerSprite.PhysicsBox.Body.Position.Y + scroll.VerticalOffset < App.DownOffset) // ��
                scroll.ScrollToVerticalOffset(_playerSprite.PhysicsBox.Body.Position.Y - App.AreaHeight + App.DownOffset);
            else if (_playerSprite.PhysicsBox.Body.Position.Y - scroll.VerticalOffset < App.UpOffset) // ��
                scroll.ScrollToVerticalOffset(_playerSprite.PhysicsBox.Body.Position.Y - App.UpOffset);

            map.Refresh();
        }

        /// <summary>
        /// �����������ʾ��������
        /// </summary>
        void Clear()
        {
            interactive.Visibility = Visibility.Visible;
            map.Visibility = Visibility.Collapsed;

            foreach (var element in canvas.Children)
            {
                var physicsBox = element as PhysicsBox;
                if (physicsBox != null)
                {
                    var sprite = physicsBox.Geom.Tag as Sprite;
                    sprite.Enabled = false;
                }
            }

            canvas.Children.Clear();

            CompositionTarget.Rendering -= new EventHandler(CompositionTarget_Rendering);
        }

        /// <summary>
        /// ���ؽ������棬�����û������ķɴ��͵о�
        /// </summary>
        void Start()
        {
            interactive.Visibility = Visibility.Collapsed;
            map.Visibility = Visibility.Visible;

            scroll.ScrollToHorizontalOffset(App.SceneWidth / 2 - App.AreaWidth / 2);
            scroll.ScrollToVerticalOffset(App.SceneHeight / 2 - App.AreaHeight / 2);

            LoadShip();
            LoadAIShip(board.Level);

            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }
    }
}
