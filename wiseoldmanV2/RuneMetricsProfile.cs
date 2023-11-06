using System;
using System.Collections.Generic;

namespace wiseoldmanV2
{
    internal class RuneMetricsProfile
    {
        public string Name { get; set; }
        public string Rank { get; set; }
        public int TotalSkill { get; set; }
        public long TotalXP { get; set; }
        public int CombatLevel { get; set; }
        public long Magic { get; set; }
        public long Melee { get; set; }
        public long Ranged { get; set; }
        public int QuestsStarted { get; set; }
        public int QuestsComplete { get; set; }
        public int QuestsNotStarted { get; set; }
        public List<RuneMetricsActivity> Activities { get; set; }
        public List<RuneMetricsSkill> SkillValues { get; set; } 
        public string LoggedIn { get; set; } 
    }

    internal class RuneMetricsActivity
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }

    internal class RuneMetricsSkill
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public long Experience { get; set; }
        public string Rank { get; set; }
    }
}
