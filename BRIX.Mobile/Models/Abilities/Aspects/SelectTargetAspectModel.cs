using BRIX.Library.Aspects;
using BRIX.Library.Mathematics;
using static BRIX.Library.Aspects.AreaSettings;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class TargetSelectionAspectModel : AspectModelBase
    {
        public TargetSelectionAspectModel(AspectBase model) : base(model) { }

        public TargetSelectionAspect Internal => GetSpecificAspect<TargetSelectionAspect>();

        public ETargetSelectionStrategy Strategy
        {
            get => Internal.Strategy;
            set 
            { 
                SetProperty(Internal.Strategy, value, Internal, (model, prop) => model.Strategy = prop);
                UpdateCost();
            }
        }

        public int NTADCount
        {
            get => Internal.NTAD.TargetsCount;
            set
            {
                SetProperty(Internal.NTAD.TargetsCount, value, Internal, (model, prop) => model.NTAD.TargetsCount = prop); 
                UpdateCost();
            }
        }

        public int NTADistance
        {
            get => Internal.NTAD.DistanceInMeters;
            set 
            { 
                SetProperty(Internal.NTAD.DistanceInMeters, value, Internal, (model, prop) => model.NTAD.DistanceInMeters = prop); 
                UpdateCost();
            }
        }

        public EAreaType AreaType
        {
            get => Internal.Area.AreaType;
            set
            {
                SetProperty(Internal.Area.AreaType, value, Internal, (model, prop) => model.Area.AreaType = prop);
                UpdateCost();
            }
        }

        public int AreaDistance
        {
            get => Internal.Area.DistanceToAreaInMeters;
            set
            {
                SetProperty(Internal.Area.DistanceToAreaInMeters, value, Internal, (model, prop) => model.Area.DistanceToAreaInMeters = prop);
                UpdateCost();
            }
        }

        public int R
        {
            get
            {
                switch (AreaType)
                {
                    case EAreaType.Sphere:
                        return ((Sphere)Internal.Area.Shape).R;
                    case EAreaType.Cylinder:
                        return ((Cylinder)Internal.Area.Shape).R;
                    case EAreaType.Cone:
                        return ((Cone)Internal.Area.Shape).R;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (AreaType)
                {
                    case EAreaType.Sphere:
                        Sphere sphere = (Sphere)Internal.Area.Shape;
                        SetProperty(sphere.R, value, sphere, (model, prop) => model.R = prop);
                        break;
                    case EAreaType.Cylinder:
                        Cylinder cylinder = (Cylinder)Internal.Area.Shape;
                        SetProperty(cylinder.R, value, cylinder, (model, prop) => model.R = prop);
                        break;
                    case EAreaType.Cone:
                        Cone cone = (Cone)Internal.Area.Shape;
                        SetProperty(cone.R, value, cone, (model, prop) => model.R = prop);
                        break;
                }
                
                UpdateCost();
            }
        }

        public int H
        {
            get
            {
                switch (AreaType)
                {
                    case EAreaType.Cylinder:
                        return ((Cylinder)Internal.Area.Shape).H;
                    case EAreaType.Cone:
                        return ((Cone)Internal.Area.Shape).H;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (AreaType)
                {
                    case EAreaType.Cylinder:
                        Cylinder cylinder = (Cylinder)Internal.Area.Shape;
                        SetProperty(cylinder.H, value, cylinder, (model, prop) => model.H = prop);
                        break;
                    case EAreaType.Cone:
                        Cone cone = (Cone)Internal.Area.Shape;
                        SetProperty(cone.H, value, cone, (model, prop) => model.H = prop);
                        break;
                }

                UpdateCost();
            }
        }

        public int A
        {
            get => Internal.Area.Shape is Brick brick ? brick.A : 0;
            set
            {
                if(Internal.Area.Shape is Brick brick)
                {
                    brick.A = value;
                    UpdateCost();
                }
            }
        }

        public int B
        {
            get => Internal.Area.Shape is Brick brick ? brick.B : 0;
            set
            {
                if (Internal.Area.Shape is Brick brick)
                {
                    brick.B = value;
                    UpdateCost();
                }
            }
        }

        public int C
        {
            get => Internal.Area.Shape is Brick brick ? brick.C : 0;
            set
            {
                if (Internal.Area.Shape is Brick brick)
                {
                    brick.C = value;
                    UpdateCost();
                }
            }
        }

        public int N
        {
            get => Internal.Area.Shape is VoxelArray voxels ? voxels.N : 0;
            set
            {
                if (Internal.Area.Shape is VoxelArray voxels)
                {
                    voxels.N = value;
                    UpdateCost();
                }
            }
        }
    }
}