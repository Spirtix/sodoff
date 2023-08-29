﻿using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;
public class User {
    [Key]
    public string Id { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    public virtual ICollection<Session> Sessions { get; set; } = null!;
    public virtual ICollection<Viking> Vikings { get; set; } = null!;
    public virtual ICollection<PairData> PairData { get; set; } = null!;
}
