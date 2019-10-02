// FrameLord
using FrameLord;

public enum SoundId : int
{
    SOUND_HIT			   = 0,
    SOUND_SERVE            = 1,
    SOUND_STEPS            = 2,
    SOUND_BOUNCE           = 3,
}

/// <summary>
/// SoundList of the Game
/// </summary>
public class TenisGameSoundList : SoundList
{
    SoundProp[] sounds =
    {
        new SoundProp((int) SoundId.SOUND_HIT,         "HitSound",         2, 100),
        new SoundProp((int) SoundId.SOUND_SERVE,       "ServeSound",         2, 100),
        new SoundProp((int) SoundId.SOUND_STEPS,       "StepsSound",         1, 50),
        new SoundProp((int) SoundId.SOUND_BOUNCE,       "BounceBallSound",         1, 50),
        new SoundProp((int) SoundId.SOUND_BOUNCE,       "ClappingSound",         1, 50),

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