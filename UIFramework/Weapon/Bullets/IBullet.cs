using ControlLib;
using ProtoRender.Object;
using RayTracingLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramework.Weapon.Bullets;
public interface IBullet
{
    float Damage { get; set; }
    Task FlightAsync(IUnit owner);
    void Flight(IUnit owner);

    IBullet GetCopy();
}
