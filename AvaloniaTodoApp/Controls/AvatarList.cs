using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using AvaloniaTodoAPp.Converters;

namespace AvaloniaTodoAPp.Controls;

public class AvatarList : TemplatedControl
{
    public class AvatarData
    {
        public string? Username { get; set; }
        public Bitmap? Image { get; set; }
    }

    public enum AvatarListFlow
    {
        LeftRight,
        RightLeft,
        TopBottom,
        BottomTop
    }

    public AvatarList()
    {
        Items = Avatars.Select(ToAvatar);
    }

    public static readonly StyledProperty<int> SizeProperty =
        AvaloniaProperty.Register<Avatar, int>(nameof(Size), defaultValue: 32);

    public int Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public static readonly StyledProperty<AvatarListFlow> FlowProperty =
        AvaloniaProperty.Register<Avatar, AvatarListFlow>(nameof(Color), defaultValue: AvatarListFlow.LeftRight);

    public AvatarListFlow Flow
    {
        get => GetValue(FlowProperty);
        set => SetValue(FlowProperty, value);
    }

    public static readonly DirectProperty<AvatarList, IEnumerable<AvatarData>> AvatarsProperty =
        AvaloniaProperty.RegisterDirect<AvatarList, IEnumerable<AvatarData>>(
            nameof(Avatars), list => list.Avatars, (list, v) => list.Avatars = v);

    private IEnumerable<AvatarData> _avatars = [];

    public IEnumerable<AvatarData> Avatars
    {
        get => _avatars;
        set => SetAndRaise(AvatarsProperty, ref _avatars, value);
    }

    // Holds the local avatar items. Updated when Avatars change
    private static readonly DirectProperty<AvatarList, IEnumerable<Avatar>> ItemsProperty =
        AvaloniaProperty.RegisterDirect<AvatarList, IEnumerable<Avatar>>(
            nameof(Items), list => list.Items, (list, v) => list.Items = v);

    private IEnumerable<Avatar> _items = [];

    public IEnumerable<Avatar> Items
    {
        get => _items;
        private set => SetAndRaise(ItemsProperty, ref _items, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == AvatarsProperty)
        {
            Items = Avatars.Select(ToAvatar);
        }
    }

    public Orientation Orientation => Flow is AvatarListFlow.LeftRight or AvatarListFlow.RightLeft
        ? Orientation.Horizontal
        : Orientation.Vertical;

    public bool ReverseOrder => Flow is AvatarListFlow.RightLeft or AvatarListFlow.BottomTop;

    private Thickness Thickness =>
        (Thickness)SizeToBorderThicknessConverter.Instance.Convert(Size, typeof(object), null, null);

    private Avatar ToAvatar(AvatarData data)
    {
        return new Avatar
        {
            Username = data.Username,
            Image = data.Image,
            Size = Size,
            BorderThickness = Thickness,
            BorderBrush = GetValue(BorderBrushProperty)
        };
    }
}