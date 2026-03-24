using System.Collections.Generic;
using UnityEngine;

public static class TextAnimationUtils
{
	public enum TextAnimatorType
	{
		Shake
	}
	
	
	private static readonly Dictionary<TextAnimatorType, (string openTag, string closeTag)> TypeAnimatorMap = new()
    {
        { TextAnimatorType.Shake, ("<shake>", "</shake>") },
        
    };	
	
	private static readonly Dictionary<Color, (string openTag, string closeTag)> ColorTextMap = new()
    {
        { Color.red, ("<color=red>", "</color>") },
        { Color.green, ("<color=green>", "</color>") },
        
    };

	public static string ApplyTextAnimator(this string input, TextAnimatorType type)
	{
		if (TypeAnimatorMap.TryGetValue(type, out var tags))
		{
			return string.Concat(tags.openTag, input, tags.closeTag);
		}

		return input;
	}

	public static string ApplyTextColor(this string input, Color color)
	{
		if (ColorTextMap.TryGetValue(color, out var tags))
		{
			return string.Concat(tags.openTag, input, tags.closeTag);
		}

		return input;
	}
}
