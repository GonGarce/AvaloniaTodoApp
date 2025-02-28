using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace AvaloniaTodoApp.Controls;

public class Avatar : TemplatedControl
{
    private static readonly int ColorsCount = 15;
    private static readonly Color DefaultColor = Colors.Gray;

    public static readonly StyledProperty<string?> UsernameProperty =
        AvaloniaProperty.Register<Avatar, string?>(nameof(Username), defaultValue: null);

    public string? Username
    {
        get => GetValue(UsernameProperty);
        set => SetValue(UsernameProperty, value);
    }

    public static readonly StyledProperty<int> SizeProperty =
        AvaloniaProperty.Register<Avatar, int>(nameof(Size), defaultValue: 32);

    public int Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public static readonly StyledProperty<Bitmap?> ImageProperty =
        AvaloniaProperty.Register<Avatar, Bitmap?>(nameof(Image));

    public Bitmap? Image
    {
        get => GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public static readonly StyledProperty<IBrush?> ColorProperty =
        AvaloniaProperty.Register<Avatar, IBrush?>(nameof(Color), defaultValue: null);

    public IBrush? Color
    {
        get => GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly DirectProperty<Avatar, string?> TextProperty =
        AvaloniaProperty.RegisterDirect<Avatar, string?>(nameof(Text), avatar => avatar.Text,
            (avatar, s) => avatar.Text = s, defaultBindingMode: BindingMode.TwoWay);

    private string? _text;

    public string? Text
    {
        get => _text ?? Letter;
        set => SetAndRaise(TextProperty, ref _text, value);
    }

    // Holds the local color. Updated when color or username change.
    private static readonly StyledProperty<IBrush> AvatarColorProperty =
        AvaloniaProperty.Register<AvatarList, IBrush>(nameof(AvatarColor),
            defaultValue: new SolidColorBrush(DefaultColor));

    public IBrush AvatarColor
    {
        get => GetValue(AvatarColorProperty);
        private set => SetValue(AvatarColorProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == UsernameProperty || change.Property == ColorProperty)
        {
            AvatarColor = Color ?? GetUserColor(change.NewValue?.ToString());
        }
    }

    private IBrush GetUserColor(string? username)
    {
        if (username is null) return new SolidColorBrush(DefaultColor);

        var a = username.ToCharArray().Select(c => (byte)c).Aggregate(0, (b, b1) => b + b1);
        var index = a % ColorsCount;
        if (Application.Current?.Resources[$"AvatarColor{index}"] is Color color)
        {
            return new SolidColorBrush(color);
        }

        return new SolidColorBrush(DefaultColor);
    }

    private string? Letter => Username?.First().ToString().ToUpper();

    public CornerRadius Radius => new(Size);
}