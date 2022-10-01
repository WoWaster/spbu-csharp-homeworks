namespace Task2;

using System.Text;

/// <summary>
///     Implements BWT encoding and decoding without EOF symbol.
/// </summary>
public static class Bwt
{
    /// <summary>
    ///     Encodes string using Burrows-Wheeler transform.
    /// </summary>
    /// <param name="text">String to transform.</param>
    /// <returns>
    ///     A BWT encoded string and a value required to decode.
    /// </returns>
    public static (string BwtText, int EndPosition) ForwardBwt(string text)
    {
        var doubledText = text + text;
        var suffixArray = ConstructSuffixArray(doubledText);
        var bwtText = new StringBuilder();

        var endPosition = 0;
        for (var i = 0; i < doubledText.Length; i++)
        {
            if (suffixArray[i] >= text.Length)
            {
                continue;
            }

            var index = suffixArray[i] - 1;
            if (index == -1)
            {
                index = text.Length - 1;
                endPosition = i / 2;
            }

            bwtText.Append(text[index]);
        }


        return (bwtText.ToString(), endPosition);
    }

    /// <summary>
    ///     Decodes BWT string to it's original state.
    /// </summary>
    /// <param name="bwtText">BWT encoded text.</param>
    /// <param name="initialPosition">Value required to restore string.</param>
    /// <returns>Original text.</returns>
    public static string ReverseBwt(string bwtText, int initialPosition)
    {
        var bwtTextList = bwtText.ToList();
        bwtTextList.Sort();

        var occurenceIndices = new Dictionary<char, int>();
        occurenceIndices.Add(bwtTextList[0], 0);
        for (var i = 1; i < bwtTextList.Count; i++)
        {
            if (bwtTextList[i] == bwtTextList[i - 1])
            {
                continue;
            }

            occurenceIndices.Add(bwtTextList[i], i);
        }

        var transitionIndices = new List<int>();
        for (var i = 0; i < bwtText.Length; i++)
        {
            transitionIndices.Add(occurenceIndices[bwtText[i]]);
            occurenceIndices[bwtText[i]]++;
        }

        var text = new StringBuilder();
        var currentPosition = initialPosition;

        for (var i = 0; i < bwtText.Length; i++)
        {
            text.Insert(0, bwtText[currentPosition]);
            currentPosition = transitionIndices[currentPosition];
        }

        return text.ToString();
    }

    private static List<int> ConstructSuffixArray(string text)
    {
        var suffixArray = new List<int>();
        for (var i = 0; i < text.Length; i++)
        {
            suffixArray.Add(i);
        }

        suffixArray.Sort((i, j) => string.Compare(text, i, text, j, text.Length, StringComparison.Ordinal));
        return suffixArray;
    }
}