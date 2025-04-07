using ControlLib;
using ProtoRender.Object;
using RayTracingLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLib;

namespace UIFramework.Weapon.Patron;
public interface IBullet
{
    void Flight(Entity owner);
}
