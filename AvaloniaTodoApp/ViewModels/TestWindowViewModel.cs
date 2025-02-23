﻿using System;
using System.Drawing;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using System.Collections.Generic;
using AvaloniaTodoApp.App;
using AvaloniaTodoAPp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Color = Avalonia.Media.Color;
using CommunityToolkit.Mvvm.Input;
using Supabase.Postgrest;

namespace AvaloniaTodoAPp.ViewModels;

public partial class TestWindowViewModel : ViewModelBase
{
    public TestWindowViewModel()
    {
        //Save();
        Select();
    }

    private void Save()
    {
        var me = new SProfile { Id = AppState.Instance.Supabase.Auth.CurrentUser!.Id! };
        AppState.Instance.Supabase.From<SCollection>()
            .Insert(new SCollection
            {
                Name = "Gonzalo 1",
                CreatedAt = DateTime.Now,
                OwnerId = me.Id,
            } /*, new QueryOptions{Returning = QueryOptions.ReturnType.Minimal}*/).ContinueWith(task =>
            {
                var col = task.Result;
                Console.WriteLine($"Inserted: {col.Models.Count}");
            });
    }

    [RelayCommand]
    private void Select()
    {
        var me = new SProfile { Id = AppState.Instance.Supabase.Auth.CurrentUser!.Id! };
        AppState.Instance.Supabase.From<SCollection>()
            // RLS already does this job but filter helps query plan improving performance. RLS is security not filtering
            // https://supabase.com/docs/guides/database/postgres/row-level-security#add-filters-to-every-query
            .Filter("profiles_collections.profile_id", Constants.Operator.Equals, me.Id)
            .Get().ContinueWith(task =>
            {
                var col = task.Result;
                Console.WriteLine($"Found: {col.Models.Count}");
            });
    }

    [ObservableProperty]
    private string _text = string.Empty;

    [ObservableProperty]
    private Brush _color;

    [RelayCommand]
    private void Convert()
    {
        var a = Text.ToCharArray().Select(c => (byte)c)
            .Aggregate(0, (b, b1) => b + b1);
        var index = a % Colors.Length;
        if (Application.Current?.Resources[$"AvatarColor{index}"] is Color color)
        {
            Color = new SolidColorBrush(color);
        }

        Console.WriteLine(a);
    }

    //#e0e0e0
    private static readonly string[] Colors =
    [
        "#f44336",
        "#e91e63",
        "#9c27b0",
        "#673ab7",
        "#3f51b5",
        "#2196f3",
        "#009688",
        "#00bcd4",
        "#4caf50",
        "#cddc39",
        "#ffc107",
        "#ff9800",
        "#ff5722",
        "#795548",
        "#607d8b",
    ];
}