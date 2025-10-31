using Beamable.Common.Content;
using Beamable.Common.Content.Validation;

[ContentType("gameConfigContent")]
public class GameConfigContent : ContentObject
{
    [MustBeNonNegative]
    public float ScoreMultiplier;
    [MustBeNonNegative]
    public float ScoreThresholdForBonus;
    [MustBeNonNegative]
    public float CurrencyPerScore;
}
