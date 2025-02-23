using System;
using AvaloniaTodoAPp.Models;
using DialogHostAvalonia;

namespace AvaloniaTodoAPp.Dialogs;

public class DeleteDialogModel(string name)
{
    public string Name { get; set; } = name;

    public void Delete()
    {
        DialogHost.GetDialogSession("CollectionsDialogHost")?.Close(true);
    }
}