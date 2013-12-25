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
            // 设置场景以及可视区的大小
            scene.Width = App.SceneWidth;
            scene.Height = App.SceneHeight;
            root.Width = scroll.Width = App.AreaWidth;
            root.Height = scroll.Height = App.AreaHeight;
            scene.Loaded += new RoutedEventHandler(delegate
            {
                scroll.ScrollToHorizontalOffset(App.SceneWidth / 2 - App.AreaWidth / 2);
                scroll.ScrollToVerticalOffset(App.SceneHeight / 2 - App.AreaHeight / 2);
            });

            // 地图位置
            map.SetValue(Canvas.LeftProperty, App.AreaWidth - map.Width - 5);
            map.SetValue(Canvas.TopProperty, App.AreaHeight - map.Height - 5);
            map.Panel = canvas;
            map.Visibility = Visibility.Collapsed;

            // 生成边界
            RectangleBorder rb = new RectangleBorder((float)App.SceneWidth, (float)App.SceneHeight, new Vector2(0, 0));
            rb.CollisionGroup = (int)SpriteType.Edge;
            rb.Load(App.PhysicsSimulator);

            // 加载交互界面
            interactive.Load(InteractiveType.Start, board.Level);
        }

        /// <summary>
        /// 发生碰撞后所指定的方法
        /// </summary>
        private bool OnCollision(Geom geom1, Geom geom2, ContactList contactList)
        {
            if (geom2.CollisionGroup == (int)SpriteType.Edge)
            {
                if (geom1.Tag is UniformVelocitySprite)
                {
                    // 与边界相撞，子弹要爆炸，飞船不用爆炸
                    ExplodeGeom(geom1);
                }
            }
            else // 友军和敌军的碰撞
            {
                if (((Sprite)geom1.Tag).Enabled == false || ((Sprite)geom2.Tag).Enabled == false)
                    return false;

                if (geom1.Tag is AISprite || geom2.Tag is AISprite) // 敌军损毁，计分加 1
                    board.Score += 1;
                if (geom1.Tag is PlayerSprite || geom2.Tag is PlayerSprite) // 自己受伤，HP 减 1
                    board.HP -= 0.1;

                // 攻击方：自己
                if (geom1.Tag is PlayerSprite)
                {
                    ExplodeGeom(geom2);

                    // 自己没 HP 了，game over
                    if (board.HP < 0.1)
                    {
                        ExplodeGeom(geom1);
                        interactive.Load(InteractiveType.Over, board.Level);
                        Clear();
                    }
                }
                // 被攻击方：自己
                else if (geom2.Tag is PlayerSprite)
                {
                    ExplodeGeom(geom1);

                    // 自己没 HP 了，game over
                    if (board.HP < 0.1)
                    {
                        ExplodeGeom(geom2);
                        interactive.Load(InteractiveType.Over, board.Level);
                        Clear();
                    }
                }
                // 自己的子弹和敌军的子弹相碰撞
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
        /// 监测是否已经消灭掉全部敌人，并升级
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
        /// 在指定的 Geom 上发生爆炸，并移除该 Geom 的所属对象
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
        /// 生成用户操作的飞船
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
        /// 按级别生成敌军
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
                // 避免敌军和自己操作的飞船产生位置重叠
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
        /// 生成子弹
        /// </summary>
        void LoadBullet(Sprite sprite, SpriteType spriteType)
        {
            //单个
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

            //散弹
            float angle = (float)Helper.Radian2Angle(sprite.PhysicsBox.Body.Rotation);
            for (int i = -10; i < 10; i++)
            {
                CreateBullet(sprite.PhysicsBox.Body.Position, angle + i * 5, spriteType);
            }

            //排弹
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
        /// 即时更新 Scroll 的滚动位置和地图
        /// </summary>
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (App.AreaWidth - _playerSprite.PhysicsBox.Body.Position.X + scroll.HorizontalOffset < App.RightOffset) // 右
                scroll.ScrollToHorizontalOffset(_playerSprite.PhysicsBox.Body.Position.X - App.AreaWidth + App.RightOffset);
            else if (_playerSprite.PhysicsBox.Body.Position.X - scroll.HorizontalOffset < App.LeftOffset) // 左
                scroll.ScrollToHorizontalOffset(_playerSprite.PhysicsBox.Body.Position.X - App.LeftOffset);

            if (App.AreaHeight - _playerSprite.PhysicsBox.Body.Position.Y + scroll.VerticalOffset < App.DownOffset) // 下
                scroll.ScrollToVerticalOffset(_playerSprite.PhysicsBox.Body.Position.Y - App.AreaHeight + App.DownOffset);
            else if (_playerSprite.PhysicsBox.Body.Position.Y - scroll.VerticalOffset < App.UpOffset) // 上
                scroll.ScrollToVerticalOffset(_playerSprite.PhysicsBox.Body.Position.Y - App.UpOffset);

            map.Refresh();
        }

        /// <summary>
        /// 清除场景，显示交互界面
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
        /// 隐藏交互界面，生成用户操作的飞船和敌军
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
