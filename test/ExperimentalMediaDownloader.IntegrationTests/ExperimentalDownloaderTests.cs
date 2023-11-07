namespace ExperimentalMediaDownloader.IntegrationTests;

using Xunit;

using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

public class ExperimentalDownloaderTests
{
    private const string MediaUrl = @"https://www.youtube.com/watch?v=kmGGsSael0U&ab_channel=PartyInBackyard";

    private const string SaveFileName = "C:\\Users\\Valera Deviatkin\\Downloads\\Eee_BOiiii.mp3";

    [Fact]
    public async Task Test_0000()
    {
        var youtube = new YoutubeClient();
        var video = await youtube.Videos.GetAsync(MediaUrl);

        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
        var audioStreamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

        await using var audioStream = await youtube.Videos.Streams.GetAsync(audioStreamInfo);
        await using var output = new FileStream(SaveFileName, FileMode.Create);
        await audioStream.CopyToAsync(output);
    }
}