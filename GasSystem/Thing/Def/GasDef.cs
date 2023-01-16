using System;
using Verse;
using RimWorld;

namespace AK_Industry
{
	public class GasDef : ThingDef
	{
		public HediffDef addHediff;

		public float hediffAddChance = 1f;

		public float hediffSeverity = 0.05f;

		public int interval = 1;

		public bool ignoreAnimals;

		public bool ignoreNormalFlesh;

		public bool ignoreInsectFlesh;

		public bool ignoreMechanoidFlesh;

		//public bool ignoreToxicSensitivity;

		public StatDef resistedBy = StatDefOf.ToxicResistance;
	}
}
