using System;
using CoreGraphics;
using MvvmCross.Platforms.Ios.Views;
using testpicturechooser.core.ViewModels;
using UIKit;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Plugin.PictureChooser.Platforms.Ios;
using Foundation;

namespace testpicturechooser.iOS
{
    public class MainViewController : MvxViewController<MainViewModel>
    {
        private UIButton takePictureButton;
        private UIButton choosePictureButton;
        private UIImageView imageView;

        byte[] imageBytes = null;

        public byte[] ImageBytes
        {
            get => this.imageBytes;
            set 
            {
                if (value == null)
                    this.imageView.Image = null;
                else
                {
                    var imageData = NSData.FromArray(value);
                    this.imageView.Image = UIImage.LoadFromData(imageData);

                }
            }
        }

        public MainViewController()
        {
            this.EdgesForExtendedLayout = UIRectEdge.None;

            this.takePictureButton = new UIButton(UIButtonType.System);
            this.takePictureButton.SetTitle("Take Picture", UIControlState.Normal);

            this.choosePictureButton = new UIButton(UIButtonType.System);
            this.choosePictureButton.SetTitle("Choose Picture", UIControlState.Normal);

            this.imageView = new UIImageView();
            this.imageView.ClipsToBounds = true;
            this.imageView.ContentMode = UIViewContentMode.ScaleAspectFit;

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.View.BackgroundColor = UIColor.White;

            this.View.AddSubviews(
                this.takePictureButton,
                this.choosePictureButton,
                this.imageView
                );

            var bindSet = this.CreateBindingSet<MainViewController, MainViewModel>();
            bindSet.Bind(this.takePictureButton).To(vm => vm.TakePictureCommand);
            bindSet.Bind(this.choosePictureButton).To(vm => vm.ChoosePictureCommand);
            bindSet.Bind(this).For(v => v.ImageBytes).To(vm => vm.PictureBytes);

            bindSet.Apply();
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            var width = this.View.Bounds.Width;
            var height = this.View.Bounds.Height;

            this.takePictureButton.Frame = new CGRect(10, 10, width - 20, 40);
            this.choosePictureButton.Frame = new CGRect(10, 60, width - 20, 40);
            this.imageView.Frame = new CGRect(10, 110, width - 20, height - 120);
        }
    }
}
