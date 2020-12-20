using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using MvvmCross.Plugin.PictureChooser;
using MvvmCross;
using System.IO;

namespace testpicturechooser.core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private IMvxLog mvxLog; 
        public MainViewModel(IMvxLogProvider mvxLogProvider)
        {
            this.mvxLog = mvxLogProvider.GetLogFor(this.GetType().Name);
        }


        private byte[] pictureBytes = null;
        public byte[] PictureBytes {
            get => this.pictureBytes;
            private set => SetProperty(ref this.pictureBytes, value);
        }


        private MvxAsyncCommand takePictureCommand;
        public IMvxAsyncCommand TakePictureCommand
        {
            get
            {
                this.takePictureCommand = this.takePictureCommand ?? new MvxAsyncCommand(ExecuteTakePicture);
                return this.takePictureCommand;
            }
        }


        private MvxAsyncCommand choosePictureCommand;
        public IMvxAsyncCommand ChoosePictureCommand
        {
            get
            {
                this.choosePictureCommand = this.choosePictureCommand ?? new MvxAsyncCommand(ExecuteChoosePicture);
                return this.choosePictureCommand;
            }
        }

        private async Task InternalPicture(Func<IMvxPictureChooserTask, Task<Stream>> takePicture)
        {
            this.PictureBytes = null;
            if (Mvx.IoCProvider.TryResolve(out IMvxPictureChooserTask pictureChooser))
            {
                Stream stream = null;
                try
                {
                    await this.AsyncDispatcher.ExecuteOnMainThreadAsync(async () => stream = await takePicture(pictureChooser));
                    
                    if (stream != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await stream.CopyToAsync(ms);
                            this.PictureBytes = ms.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.mvxLog.DebugException("During ", ex);
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }
            }
            else
                this.mvxLog.Error("Can't resolve IMvxPictureChooserTask");
        }

        private async Task ExecuteTakePicture()
        {
            await this.InternalPicture(async(pc) => await pc.TakePictureAsync(1000, 1000));
        }

        private async Task ExecuteChoosePicture()
        {
            await this.InternalPicture(async (pc) => await pc.ChoosePictureFromLibraryAsync(1000, 1000));
        }
    }
}
