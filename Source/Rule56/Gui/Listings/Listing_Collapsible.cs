﻿using System;
using System.Collections.Generic;
using CombatAI.R;
using UnityEngine;
using Verse;
using GUILambda = System.Action<UnityEngine.Rect>;

namespace CombatAI.Gui
{
	public class Listing_Collapsible : IListing_Custom
	{

		private bool              expanded;
		private Group_Collapsible group;

		public Listing_Collapsible(bool expanded = false, bool scrollViewOnOverflow = true) : base(scrollViewOnOverflow)
		{
			this.expanded = expanded;
			group         = new Group_Collapsible();
		}

		public Listing_Collapsible(Group_Collapsible group, bool expanded = false, bool scrollViewOnOverflow = true) : base(scrollViewOnOverflow)
		{
			this.expanded = expanded;
			this.group    = group;
			this.group.Register(this);
		}

		public Group_Collapsible Group
		{
			get => group;
			set
			{
				group.AllCollapsibles.RemoveAll(c => c == this);
				group = value;
				group.Register(this);
			}
		}

		public bool Expanded
		{
			get => expanded;
			set
			{
				group.CollapseAll();
				expanded = value;
			}
		}

		public virtual void Begin(Rect inRect, TaggedString title, bool drawInfo = true, bool drawIcon = true, bool hightlightIfMouseOver = true)
		{
			base.Begin(inRect);
			GUIUtility.ExecuteSafeGUIAction(() =>
			{
				GUIFont.Font   = GUIFontSize.Tiny;
				GUIFont.Anchor = TextAnchor.MiddleLeft;
				RectSlice slice = Slice(title.GetTextHeight(insideWidth - 30f));
				if (hightlightIfMouseOver)
				{
					Widgets.DrawHighlightIfMouseover(slice.outside);
				}
				GUI.color = CollapsibleBGBorderColor;
				GUI.color = Color.gray;
				Rect titleRect = slice.inside;
				if (drawInfo)
				{
					GUIFont.Font   = GUIFontSize.Tiny;
					GUIFont.Anchor = TextAnchor.MiddleRight;
					Widgets.Label(titleRect, expanded ? Keyed.CombatAI_Hide : Keyed.CombatAI_Expand);
				}
				GUIFont.Font                   = GUIFontSize.Smaller;
				GUIFont.CurFontStyle.fontStyle = FontStyle.Normal;
				GUIFont.Anchor                 = TextAnchor.MiddleLeft;
				if (drawIcon)
				{
					Widgets.DrawTextureFitted(titleRect.LeftPartPixels(25), expanded ? TexButton.Collapse : TexButton.Reveal, 0.65f);
					titleRect.xMin += 35;
				}
				GUI.color = Color.white;
				Widgets.Label(titleRect, title);
				if (Widgets.ButtonInvisible(slice.outside))
				{
					Expanded = !Expanded;
				}
				GUI.color = CollapsibleBGBorderColor;
				Widgets.DrawBox(slice.outside);
			});
			if (Expanded)
			{
				Gap(2);
			}
			base.Start();
		}

		public void Label(TaggedString text, string tooltip = null, bool invert = false, bool hightlightIfMouseOver = true, GUIFontSize fontSize = GUIFontSize.Tiny, FontStyle fontStyle = FontStyle.Normal)
		{
			if (invert == expanded)
			{
				return;
			}
			base.Label(text, tooltip, hightlightIfMouseOver, fontSize, fontStyle);
		}

		public bool CheckboxLabeled(TaggedString text, ref bool checkOn, string tooltip = null, bool invert = false, bool disabled = false, bool hightlightIfMouseOver = true, GUIFontSize fontSize = GUIFontSize.Tiny, FontStyle fontStyle = FontStyle.Normal)
		{
			if (invert == expanded)
			{
				return false;
			}
			return base.CheckboxLabeled(text, ref checkOn, tooltip, disabled, hightlightIfMouseOver, fontSize, fontStyle);
		}


		public void DropDownMenu<T>(string text, T selection, Func<T, string> labelLambda, Action<T> selectedLambda, IEnumerable<T> options, bool invert = false, bool disabled = false, GUIFontSize fontSize = GUIFontSize.Tiny, FontStyle fontStyle = FontStyle.Normal)
		{
			if (invert == expanded)
			{
				return;
			}
			base.DropDownMenu(text, selection, labelLambda, selectedLambda, options, disabled, fontSize, fontStyle);
		}

		public void Columns(float height, IEnumerable<GUILambda> lambdas, float gap = 5, bool invert = false, bool useMargins = false, Action fallback = null)
		{
			if (invert == expanded)
			{
				return;
			}
			base.Columns(height, lambdas, gap, useMargins, fallback);
		}

		public void Lambda(float height, GUILambda contentLambda, bool invert = false, bool useMargins = false, Action fallback = null)
		{
			if (invert == expanded)
			{
				return;
			}
			base.Lambda(height, contentLambda, useMargins, fallback);
		}

		public void Gap(float height = 9f, bool invert = false)
		{
			if (expanded != invert)
			{
				base.Gap(height);
			}
		}

		public void Line(float thickness, bool invert = false)
		{
			if (expanded != invert)
			{
				base.Line(thickness);
			}
		}

		public override void End(ref Rect inRect)
		{
			base.End(ref inRect);
		}

		protected override RectSlice Slice(float height, bool includeMargins = true)
		{
			return base.Slice(height, includeMargins);
		}

		public class Group_Collapsible
		{
			private List<Listing_Collapsible> collapsibles;

			public List<Listing_Collapsible> AllCollapsibles
			{
				get => collapsibles != null ? collapsibles : collapsibles = new List<Listing_Collapsible>();
			}

			public bool AnyExpanded
			{
				get => collapsibles.Any(c => c.expanded);
			}

			public void CollapseAll()
			{
				foreach (Listing_Collapsible collapsible in AllCollapsibles)
				{
					collapsible.expanded = false;
				}
			}

			public void Register(Listing_Collapsible collapsible)
			{
				AllCollapsibles.Add(collapsible);

				collapsible.expanded = false;
			}
		}
	}
}
