using Godot;
using System;

public partial class TorsoCrawler_Death : IHE_Death
{

    public override EnemyState Enter(Enemy enemy)
    {
        
        base.Enter(enemy);
        ((TorsoCrawler)enem).BackBreak();
        return this;
    }
}
