﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using GUITextState = System.Tuple<string, CombatAI.Gui.GUIFontSize, System.Tuple<float, float>, System.Tuple<int, int, int, int>, System.Tuple<UnityEngine.FontStyle, UnityEngine.FontStyle, UnityEngine.FontStyle, UnityEngine.FontStyle>>;

namespace CombatAI.Gui
{
	public static partial class GUIUtility
	{
		private const int MAX_CACHE_SIZE = 2000;

		private static readonly Dictionary<GUITextState, float> textHeightCache = new Dictionary<GUITextState, float>(512);

		private static void Cleanup()
		{
			if (textHeightCache.Count > MAX_CACHE_SIZE)
			{
				textHeightCache.Clear();
			}
		}

		public static string Fit(this string text, Rect rect)
		{
			Cleanup();
			float height = GetTextHeight(text, rect.width);
			if (height <= rect.height)
			{
				return text;
			}
			return text.Substring(0, (int)(text.Length * height / rect.height)) + "...";
		}

		public static float GetTextHeight(this string text, Rect rect)
		{
			return text != null ? CalcTextHeight(text, rect.width) : 0;
		}

		public static float GetTextHeight(this string text, float width)
		{
			return text != null ? CalcTextHeight(text, width) : 0;
		}

		public static float GetTextHeight(this TaggedString text, float width)
		{
			return text != null ? CalcTextHeight(text, width) : 0;
		}

		public static float CalcTextHeight(string text, float width)
		{
			Cleanup();
			GUITextState key = GetGUIState(text, width);
			if (textHeightCache.TryGetValue(key, out float height))
			{
				return height;
			}
			return textHeightCache[key] = Text.CalcHeight(text, width);
		}

		private static GUITextState GetGUIState(string text, float width)
		{
			return new GUITextState(
				text,
				GUIFont.Font,
				new Tuple<float, float>(
					width,
					Prefs.UIScale),
				new Tuple<int, int, int, int>(
					GUIFont.CurFontStyle.fontSize,
					Text.CurTextAreaReadOnlyStyle.fontSize,
					Text.CurTextAreaStyle.fontSize,
					Text.CurTextFieldStyle.fontSize),
				new Tuple<FontStyle, FontStyle, FontStyle, FontStyle>(
					GUIFont.CurFontStyle.fontStyle,
					Text.CurTextAreaReadOnlyStyle.fontStyle,
					Text.CurTextAreaStyle.fontStyle,
					Text.CurTextFieldStyle.fontStyle));
		}
	}
}
