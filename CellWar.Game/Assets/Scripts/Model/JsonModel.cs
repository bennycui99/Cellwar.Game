using System.Collections.Generic;


namespace CellWar.Model.Json {
    public class RegulartoryGeneJsonModel {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Length { get; set; }
        public List<RegulatoryCondition> Condition { get; set; }
        public string Description { get; set; }
    }

    public class RegulatoryCondition {
        /// <summary>
        /// Chemical Name
        /// </summary>
        public string Chemical { get; set; }
        public int Count { get; set; }
    }

    public class RaceJsonModel {
        public string Name { get; set; }
        /// <summary>
        /// 基因长度上限
        /// </summary>
        public int MaxLength { get; set; }
        /// <summary>
        /// 该种族携带的基因组
        /// </summary>
        public string CodingGeneNames { get; set; }
    }
    public class StrainJsonModel {
        public Gamer.Player Owner { get; set; } = new Gamer.Player();
        public string Name { get; set; }
        public int Population { get; set; }
        public string BasicRaceName { get; set; }
        public string PlayerSelectedGenesName { get; set; }
    }
}
