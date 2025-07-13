﻿using ControlLib;
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
    IUnit? Owner { get; }
    float Damage { get; set; }

    public static float InfinityFlightDistance { get; } = -1;
    public static float BaseDamage { get; } = 1;

    float FlightDistance { get; set; }
    bool IsActive { get; }

    void Flight(IUnit owner);
    void Update();
    IBullet GetCopy();
}
