using BRIX.Lexica;
using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Effects;
using BRIX.Library.Enums;
using BRIX.Library.Mathematics;
using BRIX.Utility.Extensions;
using System.Diagnostics;

TargetSelectionAspectModel model = new(new VolumeShapeModel());
model.AreaShape.MyProperty = 1;
TargetSelectionAspectModel? copy = model.Copy();
copy.AreaShape.MyProperty = 2;
copy.AreaShape.MyProperty = 3;
copy.AreaShape.MyProperty = 4;

Console.WriteLine();


public partial class TargetSelectionAspectModel
{
    public TargetSelectionAspectModel(VolumeShapeModel shapeModel)
    {
        AreaShape = shapeModel;
    }

    public VolumeShapeModel? _areaShape;
    public VolumeShapeModel? AreaShape
    {
        get
        {
            if (_areaShape != null)
            {
                _areaShape.VolumeShapeChanged -= CheckFireEvent;
                _areaShape.VolumeShapeChanged += CheckFireEvent;
            }

            return _areaShape;
        }
        set => _areaShape = value;
    }

    private void CheckFireEvent(object? sender, EventArgs e)
    {
        Debug.WriteLine("Fire!");
    }
}

public class VolumeShapeModel
{
    public event EventHandler? VolumeShapeChanged;

    private int myVar;
    public int MyProperty
    {
        get => myVar;
        set 
        { 
            myVar = value;
            VolumeShapeChanged?.Invoke(this, new EventArgs());
        }
    }

}