﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
namespace CombatAI.Gui
{
	public abstract class ISelector_GenericSelection<T> : ISelector
	{
		public IEnumerable<T> items;

		public string searchString = "";

		public Action<T> selectionAction;

		public bool useSearchBar = true;

		public ISelector_GenericSelection(IEnumerable<T> defs, Action<T> selectionAction, bool integrated = false,
			Action                                       closeAction = null) : base(integrated, closeAction)
		{
			items                = defs;
			this.selectionAction = selectionAction;
		}

		public virtual float RowHeight
		{
			get => 28f;
		}

		protected abstract void DoSingleItem(Rect       rect, T item);
		protected abstract bool ItemMatchSearchString(T item);

		public override void DoContent(Rect inRect)
		{
			if (useSearchBar)
			{
				Rect searchRect = inRect.TopPartPixels(25);
				GUIFont.Font = GUIFontSize.Tiny;
				if (Widgets.ButtonImage(searchRect.LeftPartPixels(25), TexButton.OpenInspector))
				{
				}
				searchRect.xMin += 25;
				searchString    =  Widgets.TextField(searchRect, searchString).ToLower();
				inRect.yMin     += 30;
			}
			try
			{
				GUIUtility.ScrollView(inRect, ref scrollPosition, items,
				                      item => !searchString.NullOrEmpty() ? ItemMatchSearchString(item) ? RowHeight : -1f : RowHeight,
				                      (rect, item) =>
				                      {
					                      DoSingleItem(rect, item);
					                      if (Widgets.ButtonInvisible(rect))
					                      {
						                      selectionAction.Invoke(item);
						                      if (!integrated)
						                      {
							                      Close();
						                      }
					                      }
				                      });
			}
			catch (Exception er)
			{
				Log.Error(er.ToString());
			}
		}
	}
}
