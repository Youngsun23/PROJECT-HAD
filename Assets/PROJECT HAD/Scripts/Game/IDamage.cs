using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public interface IActor
    {
        GameObject GetActor();
    }

    public interface IDamage
    {
        void TakeDamage(IActor actor, float damage);
    }
}
