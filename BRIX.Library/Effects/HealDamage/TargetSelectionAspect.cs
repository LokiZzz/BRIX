﻿using BRIX.Library.Effects.Base;
using BRIX.Library.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Effects.HealDamage
{
    public class TargetSelectionAspect : AspectBase
    {
        public ETargetType Strategy { get; set; }

        public override double GetCoefficient()
        {
            switch (Strategy)
            {
                case ETargetType.Area:
                    return GetAreaCoeficient();
                case ETargetType.NTargetsAtDistanсeL:
                    return GetNTADCoeficient();
                default:
                    return 1;
            }
        }

        public bool IsChainEnabled { get; set; }
        public int MaxDistanceBetweenTargets { get; set; }
        public int MaxTargetsCount { get; set; }

        public double GetChainCoefficient()
        {
            if (IsChainEnabled)
            {
                return ((75 + MaxDistanceBetweenTargets) * MaxTargetsCount).ToCoeficient();
            }
            else
            {
                return 1;
            }    
        }

        public NTADSettings NTAD { get; set; } = new NTADSettings();

        private double GetNTADCoeficient()
        {
            double countCoef = new ThrasholdCoefConverter((1, 0), (2, 100), (6, 50), (11, 10), (101, 5), (101, 1))
                .Convert(NTAD.TargetsCount)
                .ToCoeficient();

            return GetDistanceCoef(NTAD.DistanceInMeters) 
                * countCoef 
                * GetChainCoefficient();
        }

        public AreaSettings Area { get; set; } = new AreaSettings();

        private double GetAreaCoeficient()
        {
            int areaPercents = Area.Shape.GetVolume() * 5;

            return GetDistanceCoef(Area.DistanceToAreaInMeters) 
                * areaPercents.ToCoeficient() 
                * GetChainCoefficient();
        }

        private double GetDistanceCoef(int distance)
        {
            return new ThrasholdCoefConverter((1, 0), (2, 20), (3, 10), (21, 5), (101, 2), (1001, 1))
                .Convert(distance)
                .ToCoeficient();
        }
    }

    public class NTADSettings
    {
        public int TargetsCount { get; set; }

        public int DistanceInMeters { get; set; }
    }

    public class AreaSettings
    {
        public int DistanceToAreaInMeters { get; set; }

        private EAreaType _areaType;
        public EAreaType AreaType
        {
            get => _areaType;
            set
            {
                _areaType = value;
                switch (_areaType)
                {
                    case EAreaType.Brick:
                        Shape = new Brick();
                        break;
                    case EAreaType.Sphere:
                        Shape = new Sphere();
                        break;
                    case EAreaType.Cylinder:
                        Shape = new Cylinder();
                        break;
                    case EAreaType.Cone:
                        Shape = new Cone();
                        break;
                    case EAreaType.Arbitrary:
                        Shape = new VoxelArray();
                        break;
                }
            }
        }

        public IShape Shape { get; private set; }

        public enum EAreaType
        {
            Brick = 0,
            Sphere = 1,
            Cylinder = 2,
            Cone = 3,
            Arbitrary = 4
        }
    }

    public enum ETargetType
    {
        None = 0,
        Area = 1,
        NTargetsAtDistanсeL = 2
    }
}