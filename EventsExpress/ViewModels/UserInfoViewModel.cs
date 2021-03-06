﻿using System;
using System.Collections.Generic;

namespace EventsExpress.ViewModels
{
    public class UserInfoViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime Birthday { get; set; }

        public byte Gender { get; set; }

        public string Role { get; set; }

        public string PhotoUrl { get; set; }

        public string Token { get; set; }

        public bool AfterEmailConfirmation { get; set; } = false;

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public double Rating { get; set; }

        public bool CanChangePassword { get; set; }
    }
}
