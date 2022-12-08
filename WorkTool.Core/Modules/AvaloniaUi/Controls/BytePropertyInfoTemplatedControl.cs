﻿namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class BytePropertyInfoTemplatedControl : PropertyInfoTemplatedControl<byte, NumericUpDown>
{
    static BytePropertyInfoTemplatedControl()
    {
        IObjectValue.ObjectProperty.AddOwner<BytePropertyInfoTemplatedControl>(x => x.Object);
    }

    public BytePropertyInfoTemplatedControl()
        : base(
            (property, _, control, _) =>
            {
                control
                    .GetObservable(NumericUpDown.ValueProperty)
                    .Subscribe(x => property.Value = x.HasValue ? (byte)x.Value : (byte)0);

                property.GetObservable(ValueProperty).Subscribe(x => control.Value = x);
            }
        ) { }
}
