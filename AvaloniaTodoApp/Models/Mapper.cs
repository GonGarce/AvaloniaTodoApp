using System.Linq;
using AvaloniaTodoAPp.ViewModels;
using AvaloniaTodoApp.ViewModels.Controls;

namespace AvaloniaTodoAPp.Models;

public class Mapper
{
    public static STask ToModel(TodoTaskViewModel viewModel)
    {
        return new STask
        {
            Id = viewModel.Id,
            CollectionId = viewModel.CollectionId,
            Description = viewModel.Description,
            Critical = viewModel.Important,
            CreatedAt = viewModel.CreationDate,
            CompletedAt = viewModel.CompletedAt,
            DueDate = null,
        };
    }

    public static CollectionItemViewModel ToViewModel(SCollection col)
    {
        return new CollectionItemViewModel(col.Id, col.Name, col.Order, col.CreatedAt, col.IsOwner())
        {
            Profiles = col.Users?.Select(pc => pc.User) ?? []
        };
    }
}