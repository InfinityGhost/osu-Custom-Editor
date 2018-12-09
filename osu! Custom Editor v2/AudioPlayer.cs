using System.Threading.Tasks;
using System.Windows.Media;

namespace osu__Custom_Editor_v2
{
    public class AudioPlayer : MediaPlayer
    {
        public bool IsPlaying { private set; get; } = false;

        public new Task Play()
        {
            IsPlaying = true;
            base.Play();
            return Task.CompletedTask;
        }

        public new Task Pause()
        {
            IsPlaying = false;
            base.Pause();
            return Task.CompletedTask;
        }

        public new Task Stop()
        {
            IsPlaying = false;
            base.Pause();
            return Task.CompletedTask;
        }
    }
}
