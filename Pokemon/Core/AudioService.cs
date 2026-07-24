using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Pokemon.Core;

// Assets/Audio의 mp3를 재생. WPF Content 리소스는 exe 기준 상대경로이므로 siteoforigin 팩 URI를 사용.
internal static class AudioService
{
    private static readonly MediaPlayer BgmPlayer = new();
    private static readonly List<MediaPlayer> ActiveSfx = new();
    private static string? currentBgmFile;

    static AudioService()
    {
        BgmPlayer.MediaEnded += (_, _) =>
        {
            BgmPlayer.Position = TimeSpan.Zero;
            BgmPlayer.Play();
        };
    }

    private static Uri AudioUri(string fileName) =>
        new($"pack://siteoforigin:,,,/Assets/Audio/{fileName}");

    /// <summary>
    /// 배경음악을 무한 반복 재생. 같은 파일이 이미 재생 중이면 아무 것도 하지 않음.
    /// </summary>
    public static void PlayBgm(string fileName)
    {
        if (currentBgmFile == fileName)
        {
            return;
        }

        currentBgmFile = fileName;
        BgmPlayer.Open(AudioUri(fileName));
        BgmPlayer.Play();
    }

    public static void StopBgm()
    {
        currentBgmFile = null;
        BgmPlayer.Stop();
    }

    /// <summary>
    /// 효과음을 한 번 재생. BGM과 겹쳐도 상관없도록 매번 새 MediaPlayer를 사용.
    /// </summary>
    public static void PlaySfx(string fileName)
    {
        var player = new MediaPlayer();
        ActiveSfx.Add(player);
        player.MediaEnded += (_, _) =>
        {
            ActiveSfx.Remove(player);
            player.Close();
        };
        player.Open(AudioUri(fileName));
        player.Play();
    }
}
