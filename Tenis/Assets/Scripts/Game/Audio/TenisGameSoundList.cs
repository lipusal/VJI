// FrameLord
using FrameLord;

public enum SoundId : int
{
    SOUND_HIT			   = 0,
    SOUND_SERVE            = 1,
   
}

/// <summary>
/// SoundList of the Game
/// </summary>
public class TenisGameSoundList : SoundList
{
    SoundProp[] sounds =
    {
        new SoundProp((int) SoundId.SOUND_HIT,         "HitSound",         1, 100),
        new SoundProp((int) SoundId.SOUND_SERVE,       "ServeSound",         1, 100),
    };

    new void Start()
    {
        base.Start();
    }

    protected override SoundProp[] GetSoundProps()
    {
        return sounds;
    }
}