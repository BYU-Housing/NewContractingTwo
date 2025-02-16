﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace ReslifeFiveBackEnd.Model
{
    public class User
    {
        public int Id { get; set; }
        public int? PersonId { get; set; }
        public string Surname { get; set; } = string.Empty;
        public string RestOfName { get; set; } = string.Empty;
        public string PreferredFirstName { get; set; } = string.Empty;
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MaritalStatus { get; set; } = string.Empty;
        public string ByuId { get; set; } = string.Empty;
        public string NetId { get; set; } = string.Empty;
        public bool? IsRestricted { get; set; }
        public string CitizenshipCountry { get; set; } = string.Empty;
        public int RoleId { get; set; }
        [Ignore]
        public virtual Role? Role { get; set; }
        [NotMapped]
        public string PreferredFullName => PreferredFirstName + " " + Surname;
        [NotMapped]
        public string SearchBarInformation => $"{PreferredFullName}\n{NetId} {ByuId}\n{DateOfBirth:MM-dd-yyyy}";
        [NotMapped]
        public string RoleName => Role?.Name ?? "Role Not Loaded";

    }
}
