using System;
using AvaloniaTodoApp.Models;
using DialogHostAvalonia;

namespace AvaloniaTodoApp.Dialogs;

public class DeleteDialogModel(string name)
{
    public string Name { get; set; } = name;

    public void Delete()
    {
        DialogHost.GetDialogSession("CollectionsDialogHost")?.Close(true);
    }
}