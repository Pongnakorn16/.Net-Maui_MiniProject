// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using MauiMiniProject.Model;
//
//    var student = Student.FromJson(jsonString);

namespace MauiMiniProject.Model
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Student
    {
        [JsonProperty("sid")]
        public long Sid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("year")]
        public List<Year> Year { get; set; }
    }

    public partial class Year
    {
        [JsonProperty("year_number")]
        public string YearNumber { get; set; }

        [JsonProperty("courses_year")]
        public List<CoursesYear> CoursesYear { get; set; }
    }

    public partial class CoursesYear
    {
        [JsonProperty("Registered_term3")]
        public List<RegisteredTerm> RegisteredTerm3 { get; set; }

        [JsonProperty("Registered_term2")]
        public List<RegisteredTerm> RegisteredTerm2 { get; set; }

        [JsonProperty("Registered_term1")]
        public List<RegisteredTerm> RegisteredTerm1 { get; set; }
    }

    public partial class RegisteredTerm
    {
        [JsonProperty("cid")]
        public string Cid { get; set; }

        [JsonProperty("c_name")]
        public string C_name { get; set; }
    }

    public partial class Student
    {
        public static List<Student> FromJson(string json) => JsonConvert.DeserializeObject<List<Student>>(json, MauiMiniProject.Model.Converter.Settings);
    }
}
