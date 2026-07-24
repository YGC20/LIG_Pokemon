namespace Pokemon.Core;

internal static class KoreanParticle
{
    public static string Subject(string word) =>
        HasFinalConsonant(word) ? "이" : "가";

    public static string Topic(string word) =>
        HasFinalConsonant(word) ? "은" : "는";

    public static string Object(string word) =>
        HasFinalConsonant(word) ? "을" : "를";

    private static bool HasFinalConsonant(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
        {
            return false;
        }

        char lastCharacter = word[^1];

        return lastCharacter is >= '가' and <= '힣' &&
               (lastCharacter - '가') % 28 != 0;
    }
}
