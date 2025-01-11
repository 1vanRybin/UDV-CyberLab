﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ShortTestDto
    {
        public Guid TestId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string Theme { get; set; }
        public string Difficulty { get; set; }
        public Guid OwnerId { get; set; }

        public TestState State { get; set; }
        public int AttemptNumber { get; set; }
        public int LeftAttemptsCount { get; set; }
        public float ScoredPoints { get; set; }
        public DateTime LeftTestTime { get; set; }
        public bool IsChecked { get; set; }
    }
}
