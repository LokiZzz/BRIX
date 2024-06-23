using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Mathematics;
using BRIX.Mobile.ViewModel.Abilities;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public class VolumeShapeModel(VolumeShape volumeShape, AbilityCostMonitorPanelVM? costMonitor = null) 
        : ObservableObject
    {
        public AbilityCostMonitorPanelVM? CostMonitor { get; set; } = costMonitor;

        public VolumeShape Internal { get; set; } = volumeShape;

        public EAreaType AreaType
        {
            get => Internal.ShapeType;
            set
            {
                SetProperty(Internal.ShapeType, value, Internal, (model, prop) => {
                    model.ShapeType = prop;
                    CostMonitor?.UpdateCost();
                });
                OnShapeChanged(value);
            }
        }

        public int R
        {
            get
            {
                return AreaType switch
                {
                    EAreaType.Sphere => Internal.GetConcreteShape<Sphere>().R,
                    EAreaType.Cylinder => Internal.GetConcreteShape<Cylinder>().R,
                    EAreaType.Cone => Internal.GetConcreteShape<Cone>().R,
                    _ => 1,
                };
            }
            set
            {
                switch (AreaType)
                {
                    case EAreaType.Sphere:
                        Sphere sphere = Internal.GetConcreteShape<Sphere>();
                        SetProperty(sphere.R, value, sphere, (model, prop) => {
                            model.R = prop;
                            CostMonitor?.UpdateCost();
                        });
                        break;
                    case EAreaType.Cylinder:
                        Cylinder cylinder = Internal.GetConcreteShape<Cylinder>();
                        SetProperty(cylinder.R, value, cylinder, (model, prop) => {
                            model.R = prop;
                            CostMonitor?.UpdateCost();
                        });
                        break;
                    case EAreaType.Cone:
                        Cone cone = Internal.GetConcreteShape<Cone>();
                        SetProperty(cone.R, value, cone, (model, prop) => {
                            model.R = prop;
                            CostMonitor?.UpdateCost();
                        });
                        break;
                }
            }
        }

        public int H
        {
            get
            {
                return AreaType switch
                {
                    EAreaType.Cylinder => Internal.GetConcreteShape<Cylinder>().H,
                    EAreaType.Cone => Internal.GetConcreteShape<Cone>().H,
                    _ => 1,
                };
            }
            set
            {
                switch (AreaType)
                {
                    case EAreaType.Cylinder:
                        Cylinder cylinder = Internal.GetConcreteShape<Cylinder>();
                        SetProperty(cylinder.H, value, cylinder, (model, prop) => {
                            model.H = prop;
                            CostMonitor?.UpdateCost();
                        });
                        break;
                    case EAreaType.Cone:
                        Cone cone = Internal.GetConcreteShape<Cone>();
                        SetProperty(cone.H, value, cone, (model, prop) => {
                            model.H = prop;
                            CostMonitor?.UpdateCost();
                        });
                        break;
                }
            }
        }

        public int A
        {
            get => Internal.Shape is Brick brick ? brick.A : 1;
            set
            {
                if (Internal.Shape is Brick brick)
                {
                    brick.A = value;
                    CostMonitor?.UpdateCost();
                }
            }
        }

        public int B
        {
            get => Internal.Shape is Brick brick ? brick.B : 1;
            set
            {
                if (Internal.Shape is Brick brick)
                {
                    brick.B = value;
                    CostMonitor?.UpdateCost();
                }
            }
        }

        public int C
        {
            get => Internal.Shape is Brick brick ? brick.C : 1;
            set
            {
                if (Internal.Shape is Brick brick)
                {
                    brick.C = value;
                    CostMonitor?.UpdateCost();
                }
            }
        }

        public int N
        {
            get => Internal.Shape is VoxelArray voxels ? voxels.N : 1;
            set
            {
                if (Internal.Shape is VoxelArray voxels)
                {
                    voxels.N = value;
                    CostMonitor?.UpdateCost();
                }
            }
        }

        public bool IsArbitrary
        {
            get => Internal.Shape is VoxelArray voxels && voxels.IsArbitrary;
            set
            {
                if (Internal.Shape is VoxelArray voxels)
                {
                    voxels.IsArbitrary = value;
                    CostMonitor?.UpdateCost();
                }
            }
        }

        private void OnShapeChanged(EAreaType shape)
        {
            switch (shape)
            {
                case EAreaType.Brick:
                    OnPropertyChanged(nameof(A));
                    OnPropertyChanged(nameof(B));
                    OnPropertyChanged(nameof(C));
                    break;
                case EAreaType.Sphere:
                    OnPropertyChanged(nameof(R));
                    break;
                case EAreaType.Cone:
                case EAreaType.Cylinder:
                    OnPropertyChanged(nameof(H));
                    OnPropertyChanged(nameof(R));
                    break;
                case EAreaType.VoxelArray:
                    OnPropertyChanged(nameof(N));
                    OnPropertyChanged(nameof(IsArbitrary));
                    break;
            }
        }
    }
}