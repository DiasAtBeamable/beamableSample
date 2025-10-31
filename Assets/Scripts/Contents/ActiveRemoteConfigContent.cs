using Beamable.Common.Content;

[ContentType("activeRemoteConfigContent")]
public class ActiveRemoteConfigContent : ContentObject
{
    public ContentRef<GameConfigContent> gameConfig;
}
