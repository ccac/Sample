using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using YYMatch.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using YYMatch.Controls;

namespace YYMatch.ViewModels
{
    public class GameViewModel
    {
        CardControl _prevCardControl;
        Core _core;

        public GameViewModel()
        {
            _core = new Core();
        }

        public void Start(int level)
        {
            // 容器的大小为 16 * 12
            // 因为容器最外层的卡片都必须是空的（避免阻挡消去路径），所以卡片集合实际的最大大小为 14 * 10
            switch (level)
            {
                case 1:
                    _core.Start(2, 4);
                    break;
                case 2:
                    _core.Start(4, 8);
                    break;
                case 3:
                    _core.Start(6, 10);
                    break;
                case 4:
                    _core.Start(8, 12);
                    break;
                case 5:
                    _core.Start(10, 14);
                    break;
                default:
                    break;
            }
        }

        public void CheckedCard(CardControl currentCardControl)
        {
            CardModel prevCardModel, currentCardModel;

            if (_prevCardControl == null)
            {
                _prevCardControl = currentCardControl;
                _prevCardControl.Checked();
                return;
            }

            prevCardModel = new CardModel(_prevCardControl.ImageName, (int)_prevCardControl.Tag);
            currentCardModel = new CardModel(currentCardControl.ImageName, (int)currentCardControl.Tag);

            if (prevCardModel.Position != currentCardModel.Position)
            {
                bool result = _core.Match(prevCardModel, currentCardModel, true);
                _prevCardControl.Unchecked();

                if (result)
                {
                    if (!_core.IsActive())
                        _core.Replace();

                    _prevCardControl = null;

                    OnRemoved();

                    if (_core.CardCount == 0)
                        OnFinished();
                }
                else
                {
                    _prevCardControl = currentCardControl;
                    currentCardControl.Checked();

                    OnFailed();
                }
            }
            else
            {
                _prevCardControl.Unchecked();
                _prevCardControl = null;
            }
        }

        public void Replace()
        {
            _core.Replace();
        }

        public event EventHandler Removed;
        public void OnRemoved()
        {
            if (Removed != null)
                Removed(this, EventArgs.Empty);
        }

        public event EventHandler Failed;
        public void OnFailed()
        {
            if (Failed != null)
                Failed(this, EventArgs.Empty);
        }

        public event EventHandler Finished;
        public void OnFinished()
        {
            if (Finished != null)
                Finished(this, EventArgs.Empty);
        }

        public ObservableCollection<CardModel> Cards
        {
            get { return _core.Cards; }
        }

        public PointCollection Points
        {
            get { return _core.Points; }
        }
    }
}
