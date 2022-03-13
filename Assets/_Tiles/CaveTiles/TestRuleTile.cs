using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TestRuleTile : RuleTile<TestRuleTile.Neighbor> {
    public bool customField;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Null = 3;
        public const int NotNull = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        TestRuleTile myTile = tile as TestRuleTile;
        switch (neighbor) {
            case Neighbor.Null: return false;
            case Neighbor.NotNull: return true;
        }
        return base.RuleMatch(neighbor, tile);
    }
}