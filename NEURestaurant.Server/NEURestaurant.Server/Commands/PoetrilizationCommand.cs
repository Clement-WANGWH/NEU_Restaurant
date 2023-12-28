using System.ComponentModel.DataAnnotations;

namespace DailyPoetryH.Server.Commands;

public class PoetrilizationCommand {
    [Required]
    public IFormFile File { get; set; }
}