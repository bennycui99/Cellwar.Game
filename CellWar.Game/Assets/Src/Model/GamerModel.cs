using System;
using System.Collections;
using System.Collections.Generic;
using CellWar.Model.Substance;
using UnityEngine;


/// <summary>
/// 玩家相关Model
/// </summary>
namespace CellWar.Model.Gamer {
    public class Player {
        public string Name { get; set; }
        public Guid Guid { get; set; }
    }
}