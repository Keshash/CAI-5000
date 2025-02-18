﻿using System;
using Verse;
namespace CombatAI
{
	public class ISGrid<T> where T : struct, IComparable<T>
	{
		private readonly Cell[]      cells;
		private readonly CellIndices indices;

		private int sig = 1;

		public ISGrid(Map map)
		{
			indices = map.cellIndices;
			cells   = new Cell[indices.NumGridCells];
		}

		public T this[IntVec3 cell]
		{
			get => this[indices.CellToIndex(cell)];
			set => this[indices.CellToIndex(cell)] = value;
		}

		public T this[int index]
		{
			get
			{
				Cell cell = cells[index];
				return cell.sig == sig ? cell.value : default(T);
			}
			set => cells[index] = new Cell
			{
				sig   = sig,
				value = value
			};
		}

		public void Reset()
		{
			sig++;
		}

		public bool IsSet(IntVec3 cell)
		{
			return IsSet(indices.CellToIndex(cell));
		}
		public bool IsSet(int index)
		{
			return cells[index].sig == sig;
		}

		private struct Cell
		{
			public T   value;
			public int sig;
		}
	}
}
